using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DSA.Database.Model.Entities;
using Excel = Microsoft.Office.Interop.Excel;
namespace DSA.FileUpload.ExcelFile
{
    public static class ExcelExporter
    {
        public static void ExportExcelFile(List<InterventionDetails> interventions)
        {
            try
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;
                Excel.Range chartRange;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);                
                xlWorkSheet.Cells[1, 1] = "Data";
                xlWorkSheet.Cells[1, 2] = "Nume Prenume";
                xlWorkSheet.Cells[1, 3] = "Locatie";
                xlWorkSheet.Cells[1, 4] = "Manopera";
                xlWorkSheet.Cells[1, 5] = "Ora";
                xlWorkSheet.Cells[1, 6] = "Durata";
                xlWorkSheet.Cells[1, 7] = "Starsit";
                xlWorkSheet.Cells[1, 8] = "Incasare";                
                int row = 2;
                int col = 0;
                double total = 0;
                foreach (var interventionDetailse in interventions)
                {
                    xlWorkSheet.Cells[row, 1] = interventionDetailse.Date;
                    xlWorkSheet.Cells[row, 2] = interventionDetailse.PacientName;
                    xlWorkSheet.Cells[row, 3] = interventionDetailse.Location;
                    xlWorkSheet.Cells[row, 4] = interventionDetailse.Work;
                    xlWorkSheet.Cells[row, 5] = interventionDetailse.StartH;
                    xlWorkSheet.Cells[row, 6] = interventionDetailse.Durata;//interventionDetailse.Durata.Hours+":"+interventionDetailse.Durata.Minutes;
                    xlWorkSheet.Cells[row, 7] = interventionDetailse.EndH;
                    xlWorkSheet.Cells[row, 8] = interventionDetailse.Revenue;
                    total += interventionDetailse.Revenue;
                    row++;
                }
                xlWorkSheet.Cells[row, 8] = total;
                var currentData = DateTime.Now;
//                xlWorkBook.SaveAs("E:\\Dental Tracking System\\exportedFiles\\csharp.net-informations.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\DTS\Plati\xls\";
                string fileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\DTS\Plati\xls\" + currentData.Year + "_" + currentData.Month+"_"+currentData.Day +".xls";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                xlWorkBook.SaveAs(fileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlApp);
                releaseObject(xlWorkBook);
                releaseObject(xlWorkSheet);
                MessageBox.Show("Fisierul a fost salvat !");

            }
            catch (Exception)
            {
                MessageBox.Show("A intervenit o Eroare.Inchideti Fisierul Excel!");
            }                   
        }

        private static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
