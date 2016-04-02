using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DSA.Database.Model;
using DSA.Database.Model.Helpers;
using log4net;
using Excel = Microsoft.Office.Interop.Excel;
namespace DSA.FileUpload.ExcelFile
{
    public class ExcelParser
    {
        #region Constructor

        public ExcelParser()
        {

        }

        #endregion Constructor

        #region Properties

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum Months
        {
            IAN, FEB, MAR, APR, MAI, IUN,
        }

        private static readonly ExcelParser instance = new ExcelParser();

        public static ExcelParser Instance
        {
            get
            {
                return instance;
            }
        }

        List<string> luni = new List<string>() { "IAN", "FEB", "MAR", "APR", "MAI", "IUN", "IUL", "AUG", "SEP", "OCT", "NOI", "DEC", }; 

        public Dictionary<String, List<string>> PatientsValues;
        public Dictionary<String, List<string>> SettingsValues;

        private string FileName;

        #endregion Properties

        #region Methods

        public void GetConstants(String filePath)
        {
            FileName = filePath;
            foreach (var luna in luni)
            {
                GetPatientsValues(luna);
            }
        }

        public void SetFileName(string newFileName)
        {
            FileName = newFileName;
        }

        public void SetSettings(String filePath)
        {
            FileName = filePath;
            GetSettingsValues();
            // new Task(GetSettingsValues).Start();
        }
        public void GetPatientsValues(string luna)
        {
            try
            {
                var excelApp = new Excel.Application();
                Excel.Workbook theWorkbook = excelApp.Workbooks.Open
                    (
                        FileName, 0, true, 5, "", "", true,
                        Excel.XlPlatform.xlWindows, "\t", false, false, 0,
                        true
                    );
                PatientsValues = GetDataFromExcel(theWorkbook,luna);
                theWorkbook.Close();
                excelApp.Quit();
                Marshal.ReleaseComObject(theWorkbook);
                Marshal.ReleaseComObject(excelApp);
                //  Log.Info(FieldInValues.Count + " FieldIn variables loaded successfully.");
            }
            catch (Exception ex)
            {
                 Log.Error("ERROR loading:", ex);
            }
        }

        private void GetSettingsValues()
        {
            try
            {
                var excelApp = new Excel.Application();
                Excel.Workbook theWorkbook = excelApp.Workbooks.Open
                    (
                        FileName, 0, true, 5, "", "", true,
                        Excel.XlPlatform.xlWindows, "\t", false, false, 0,
                        true
                    );
                GetSaveLucrare(theWorkbook);
                GetSaveAreas(theWorkbook);
                GetSaveMaterials(theWorkbook);
                theWorkbook.Close();
                excelApp.Quit();
                Marshal.ReleaseComObject(theWorkbook);
                Marshal.ReleaseComObject(excelApp);
                //  Log.Info(FieldInValues.Count + " FieldIn variables loaded successfully.");
            }
            catch (Exception ex)
            {
                // Log.Error("Could not get the FieldIn variables", ex);
            }
        }

        private Dictionary<String, List<string>> GetDataFromExcel(Excel.Workbook theWorkbook,string luna)
        {
            Dictionary<String, List<string>> rows = new Dictionary<string, List<string>>();

            Excel.Worksheet worksheet = (Excel.Worksheet)theWorkbook.Worksheets[luna];
            Excel.Range rowRange = worksheet.UsedRange;
            bool shouldBreak = false;
            for (int row = 7; row <= rowRange.Rows.Count; row++)
            {
                List<string> rowValues = new List<string>();

                Excel.Range range = worksheet.Range["A" + row, "P" + row];
                System.Array values = (System.Array)range.Cells.Value;
                var enumerator = values.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    string key = enumerator.Current.ToString();
                    int i = 0;
                    while (enumerator.MoveNext())
                    {

                        var currentvalue = enumerator.Current;
                        if (currentvalue == null || currentvalue.ToString().Equals(""))
                        //                            break;
                        {
                            if (i == 3)
                            {
                                shouldBreak = true;
                                break;
                            }
                            rowValues.Add("");
                        }
                        else
                        {
                            rowValues.Add(currentvalue.ToString());
                        }
                        i++;
                    }
                    rows.Add(key.ToLower(), rowValues);
                }
                if (shouldBreak)
                    break;
            }
            Log.Error("LUNA:"+ luna);
            DatabaseHandler.Instance.SavePatients(rows.ExcelToPatient());

            return rows;
        }

        private void GetSaveLucrare(Excel.Workbook theWorkbook)
        {
            List<string> workValues = new List<string>();

            Excel.Worksheet worksheet = (Excel.Worksheet)theWorkbook.Worksheets["SETTINGS"];
            Excel.Range rowRange = worksheet.UsedRange;
            bool shouldBreak = false;
            for (int row = 2; row <= rowRange.Rows.Count; row++)
            {
                Excel.Range range = worksheet.Range["K" + row, "K" + row];
                if (range.Cells.Value == null || range.Cells.Value.ToString().Equals("")) break;
                workValues.Add(range.Cells.Value.ToString());
            }
            DatabaseHandler.Instance.SaveWorks(workValues);
        }

        private void GetSaveAreas(Excel.Workbook theWorkbook)
        {
            List<string> workValues = new List<string>();
            Excel.Worksheet worksheet = (Excel.Worksheet)theWorkbook.Worksheets["SETTINGS"];
            Excel.Range rowRange = worksheet.UsedRange;
            bool shouldBreak = false;
            for (int row = 2; row <= rowRange.Rows.Count; row++)
            {
                Excel.Range range = worksheet.Range["H" + row, "H" + row];
                if (range.Cells.Value == null || range.Cells.Value.ToString().Equals("")) break;
                workValues.Add(range.Cells.Value.ToString());
            }
            DatabaseHandler.Instance.SaveAreas(workValues);
        }

        private void GetSaveMaterials(Excel.Workbook theWorkbook)
        {
            List<string> workValues = new List<string>();
            Excel.Worksheet worksheet = (Excel.Worksheet)theWorkbook.Worksheets["SETTINGS"];
            Excel.Range rowRange = worksheet.UsedRange;
            bool shouldBreak = false;
            for (int row = 3; row <= rowRange.Rows.Count; row++)
            {
                Excel.Range range = worksheet.Range["O" + row, "O" + row];
                if (range.Cells.Value == null || range.Cells.Value.ToString().Equals("")) break;
                workValues.Add(range.Cells.Value.ToString());
            }
            DatabaseHandler.Instance.SaveMaterials(workValues);
        }

        #endregion Methods
    }
}
