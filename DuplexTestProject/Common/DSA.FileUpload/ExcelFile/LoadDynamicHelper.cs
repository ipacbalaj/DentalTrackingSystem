using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using DSA.Database.Model;
using DSA.Database.Model.EfSQlLite;
using DSA.Database.Model.Entities;
using DateHourDetail = DSA.Database.Model.EfSQlLite.DateHourDetail;
using Excel = Microsoft.Office.Interop.Excel;
namespace DSA.FileUpload.ExcelFile
{
    public static class LoadDynamicHelper
    {
        public static void IterateThroughFiles(string folderPath)
        {

            string[] fileEntries = Directory.GetFiles(folderPath);
            var biggestCreationTime = new DateTime(2000, 1, 1);
            var lastFileUpdateTime = LocalCache.Instance.ProgramInfo.LastImportDateFile;

            foreach (string fileName in fileEntries)
            {
                var createDate = File.GetCreationTime(@fileName);
                if (createDate > biggestCreationTime)
                {
                    biggestCreationTime = createDate;
                }
                if (createDate > lastFileUpdateTime)
                {
                    ReadFile(@fileName);
                }
            }
            DatabaseHandler.Instance.EditPrInfoLastFIleDate(biggestCreationTime);
        }

        public static void ReadFile(String filepath)
        {
            var excelApp = new Excel.Application();
            Excel.Workbook theWorkbook = excelApp.Workbooks.Open
                (
                    filepath, 0, true, 5, "", "", true,
                    Excel.XlPlatform.xlWindows, "\t", false, false, 0,
                    true
                );

            Dictionary<String, List<string>> rows = new Dictionary<string, List<string>>();

            Excel.Worksheet worksheet = (Excel.Worksheet)theWorkbook.Worksheets["Sheet1"];
            Excel.Range rowRange = worksheet.UsedRange;
            //            List<Intervention> interventions = new List<Intervention>();
            try
            {
                if (filepath.Contains("patients"))
                {
                    SavePatientFromFile(worksheet);
                }
                else
                {
                    SaveInterventionFromXls(worksheet);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("A intervenit o eroare in procesarea fișierului:" + filepath);

            }
            theWorkbook.Close();
            excelApp.Quit();
            Marshal.ReleaseComObject(theWorkbook);
            Marshal.ReleaseComObject(excelApp);

        }

        private static void SaveInterventionFromXls(Excel.Worksheet worksheet)
        {
            int index = 0;
            using (dsaEntities model = new dsaEntities())
            {
                DateTime biggerDate = LocalCache.Instance.ProgramInfo.LastInterventionAddedTime;
                foreach (Excel.Range row in worksheet.UsedRange.Rows)
                {
                    var intervention = new Intervention();
                    if (index == 0)
                    {
                        index++;
                        continue;
                    }

                    intervention.DateHourDetail = new DateHourDetail();
                    if (worksheet.Cells[row.Row, 1].Value2 == null)
                    {
                        break;
                    }
                    DateTime interventionDate;
                    var dateValue = (worksheet.Cells[row.Row, 1].Value2).ToString();
                    int pointApps = 0;
                    foreach (char c in dateValue)
                        if (c == '.') pointApps++;
                    interventionDate = pointApps > 1 ? DateTime.ParseExact(dateValue.Replace(".", "/"), "d/M/yyyy", CultureInfo.InvariantCulture) : DateTime.FromOADate(Convert.ToDouble(dateValue));

                    DateTime interventionStartHour = DateTime.FromOADate(worksheet.Cells[row.Row, 6].Value2);
                    if (LocalCache.Instance.ProgramInfo.LastInterventionAddedTime < interventionStartHour)
                    {
                        var inteventionYear = model.Years.FirstOrDefault(item => item.YearNb == interventionDate.Year);
                        if (inteventionYear == null)
                        {
                            inteventionYear = new Year()
                            {
                                YearNb = interventionDate.Year
                            };
                            model.Years.AddObject(inteventionYear);
                            model.SaveChanges();
                        }
                        intervention.DateHourDetail.Date = interventionDate;
                        if (interventionStartHour > biggerDate)
                        {
                            biggerDate = interventionStartHour;
                        }
                        string allPatientName = (worksheet.Cells[row.Row, 2].Value2).ToString();
                        int spaceOcc = allPatientName.IndexOf(" ");
                        var firstName = spaceOcc > 0
                            ? allPatientName.Substring(spaceOcc + 1, allPatientName.Length - spaceOcc - 1)
                            : allPatientName;
                        var lastName = spaceOcc > 0 ? allPatientName.Substring(0, spaceOcc) : "";

                        var patientToAddTo =
                            model.Patients.FirstOrDefault(item => item.name == firstName && item.surname == lastName);

                        if (patientToAddTo == null)
                        {
                            patientToAddTo = new Patient()
                            {
                                name = firstName,
                                surname = lastName
                            };
                            model.Patients.AddObject(patientToAddTo);
                        }

                        intervention.Patient = patientToAddTo;
                        if ((worksheet.Cells[row.Row, 3].Value2) != null)
                        {
                            string location = (worksheet.Cells[row.Row, 3].Value2).ToString();
                            intervention.Location = model.Locations.FirstOrDefault(item => item.Name == location);
                        }

                        if ((worksheet.Cells[row.Row, 4].Value2) != null)
                        {
                            string work = (worksheet.Cells[row.Row, 4].Value2).ToString();
                            intervention.Work = model.Works.FirstOrDefault(item => item.Name == work);
                        }
                        if ((worksheet.Cells[row.Row, 5].Value2) != null)
                        {
                            string area = (worksheet.Cells[row.Row, 5].Value2).ToString();
                            var areaToMapTo = model.Areas.FirstOrDefault(item => item.Name == area);
                            intervention.Area = areaToMapTo;
                        }
                        intervention.DateHourDetail.StartHour = interventionStartHour;
                        TimeSpan timespan = TimeSpan.FromMinutes(Convert.ToInt64(worksheet.Cells[row.Row, 7].Value2));
                        intervention.DateHourDetail.Duration = timespan.Ticks;
                        intervention.DateHourDetail.EndingHour = DateTime.FromOADate(worksheet.Cells[row.Row, 8].Value2);
                        intervention.Month = model.Months.FirstOrDefault(item => item.MOnthNumber == interventionDate.Month);
                        intervention.Year = inteventionYear;
                        intervention.Revenue = Convert.ToDouble(worksheet.Cells[row.Row, 9].Value2);
                        intervention.Percent =
                            Convert.ToDouble((intervention.Revenue) *
                                             (intervention.Work != null && intervention.Work.Percent != 0
                                                 ? intervention.Work.Percent / 100
                                                 : 1));
                        model.Interventions.AddObject(intervention);
                    }
                }
                LocalCache.Instance.ProgramInfo.LastInterventionAddedTime = biggerDate;
                var progInfo = model.ProgramInfoes.FirstOrDefault();
                progInfo.LastAddedInterventionDate = biggerDate;
                model.SaveChanges();
            }
        }

        private static void SavePatientFromFile(Excel.Worksheet worksheet)
        {
            int index = 0;
            using (dsaEntities model = new dsaEntities())
            {
                foreach (Excel.Range row in worksheet.UsedRange.Rows)
                {
                    if (index == 0)
                    {
                        index++;
                        continue;
                    }

                    string lastName = (worksheet.Cells[row.Row, 1].Value2 ?? "").ToString();
                    string firstName = (worksheet.Cells[row.Row, 2].Value2 ?? "").ToString();

                    var patient = model.Patients.FirstOrDefault(item => item.name == firstName && item.surname == lastName);
                    if (patient == null)
                    {
                        patient = new Patient()
                        {
                            name = firstName,
                            surname = lastName
                        };
                        model.Patients.AddObject(patient);
                    }
                    var birthValue = worksheet.Cells[row.Row, 3].Value2;
                    if (birthValue != null)
                    {
                        patient.BirthDate = DateTime.FromOADate(birthValue);
                    }
                    if (worksheet.Cells[row.Row, 4].Value2 != null)
                    {
                        patient.City = (worksheet.Cells[row.Row, 4].Value2).ToString();
                    }
                    patient.Street = worksheet.Cells[row.Row, 5].Value2 != null ? (worksheet.Cells[row.Row, 5].Value2).ToString() : "";
                    patient.StreetNumber = (worksheet.Cells[row.Row, 6].Value2) != null ? (worksheet.Cells[row.Row, 6].Value2).ToString() : "";
                    patient.Ocupation = (worksheet.Cells[row.Row, 7].Value2) != null ? (worksheet.Cells[row.Row, 7].Value2).ToString() : "";
                    patient.Phone = (worksheet.Cells[row.Row, 8].Value2) != null ? (worksheet.Cells[row.Row, 8].Value2).ToString() : "";
                    patient.Email = (worksheet.Cells[row.Row, 9].Value2) != null ? (worksheet.Cells[row.Row, 9].Value2).ToString() : "";
                }
                model.SaveChanges();
            }
        }
    }
}
