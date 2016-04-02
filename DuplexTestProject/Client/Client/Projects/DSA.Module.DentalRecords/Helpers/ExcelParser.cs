using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
namespace DSA.Module.DentalRecords.Helpers
{
   public class ExcelParser
   {
       #region Constructor

       public ExcelParser()
       {
           
       }
       #endregion Constructor

       public enum Constants_Models
       {
           All,
           PolyCatReg,
           PolyCatRegTerm,
           PolyCatRegHome,
           PolyCatRegPurpose,
           PolyCatRegState,
           PolyCatRegEmployment
       };

       #region Properties

       public Dictionary<Constants_Models, Dictionary<String, List<double>>> ConstantsValues;

       private static readonly ExcelParser instance = new ExcelParser();
       public static ExcelParser Instance
       {
           get
           {
               return instance;
           }
       }

       public Dictionary<String, List<string>> PatientsValues;

       private string FileName;

       #endregion Properties

       #region Methods

       public void GetConstants(String filePath)
       {
           FileName = filePath;

           new Task(GetFieldInValues).Start();
       }
       private void GetFieldInValues()
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

               PatientsValues = GetDataFromExcel(theWorkbook);

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

       private Dictionary<String, List<string>> GetDataFromExcel(Excel.Workbook theWorkbook)
       {
           Dictionary<String, List<string>> rows = new Dictionary<string, List<string>>();

           Excel.Worksheet worksheet = (Excel.Worksheet)theWorkbook.Worksheets["IAN"];
           Excel.Range rowRange = worksheet.UsedRange;

           for (int row = 6; row <= rowRange.Rows.Count; row++)
           {
               List<string> rowValues = new List<string>();

               Excel.Range range = worksheet.Range["A" + row, "M" + row];
               System.Array values = (System.Array)range.Cells.Value;
               var enumerator = values.GetEnumerator();
               if (enumerator.MoveNext())
               {
                   string key = enumerator.Current.ToString();
                   while (enumerator.MoveNext())
                   {
                       var currentvalue = enumerator.Current;
                       if (currentvalue == null || currentvalue.ToString().Equals(""))
                           break;

                       rowValues.Add(currentvalue.ToString());
                   }
                   rows.Add(key.ToLower(), rowValues);
               }
           }

           return rows;
       }

       #endregion Methods

    }
}
