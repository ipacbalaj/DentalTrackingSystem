using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Xpo.Logger;
using DSA.Financial.Entities;
using log4net;
using Novacode;
using LogManager = log4net.LogManager;

namespace DSA.Financial.WordDoc
{
    public class WordDocGenerator
    {
        #region Constructor

        public WordDocGenerator()
        {

        }
        #endregion Constructor

        #region Properties

        private List<string> headerList = new List<string>() { "Servicii prestate", "Numar", "PretCabinet", "Pret conform contractului(40%)", "Total prestari servicii", "Total conform contract(%)" };
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion Properties

        #region Methods

        public void CreateTable(List<FinancialInfo> financialInfos, FinancialInfoTotal financialInfoTotal, int an, string luna, string prestatator, string beneficiar)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\DTS\financiar\";
                string fileName =Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\DTS\financiar\"+an+"_"+luna+".docx";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                // Create a document in memory:
                var doc = DocX.Create(fileName);
                //titlu
                string headlineText = "Desfasurator servicii efectuate conform contractului de prestari servicii nr. 4/" + an;
                var headLineFormat = new Formatting();
                headLineFormat.FontFamily = new System.Drawing.FontFamily("Times New Roman");
                headLineFormat.Size = 22;
                headLineFormat.Bold = true;
                //           headLineFormat.Position = 15;
                var titleParagraph = doc.InsertParagraph(headlineText, false, headLineFormat);
                doc.InsertParagraph("", false, headLineFormat);
                titleParagraph.Alignment = Alignment.center;
                //Luna
                string monthLine = "Luna: " + luna + " " + an;
                var monthLineFormat = new Formatting();
                headLineFormat.FontFamily = new System.Drawing.FontFamily("Times New Roman");
                headLineFormat.Size = 14;
                headLineFormat.Bold = true;
                //headLineFormat.Position = 15;
                var paragraph = doc.InsertParagraph(monthLine, false, monthLineFormat);
                doc.InsertParagraph("", false, monthLineFormat);

                Table table = doc.AddTable(financialInfos.Count + 2, headerList.Count);
                for (int i = 0; i < headerList.Count; i++)
                {
                    Paragraph currParagraph = table.Rows[0].Cells[i].Paragraphs.First().Append(headerList[i]);
                    currParagraph.Bold();
                }

                int rowCount = 1;
                foreach (var financialInfo in financialInfos)
                {
                    int colCount = 0;
                    foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(financialInfo))
                    {
                        table.Rows[rowCount].Cells[colCount].Paragraphs.First().Append(prop.Converter.ConvertToString(prop.GetValue(financialInfo)));
                        colCount++;
                    }
                    rowCount++;
                }
                //last table line

                int colCountTotal = 0;
                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(financialInfoTotal))
                {
                    Paragraph currParagraph = table.Rows[rowCount].Cells[colCountTotal].Paragraphs.First().Append(prop.Converter.ConvertToString(prop.GetValue(financialInfoTotal)));
                    currParagraph.Bold();
                    colCountTotal++;
                }

                doc.InsertTable(table);
                var bottomFormat = new Formatting();
                bottomFormat.FontFamily = new System.Drawing.FontFamily("Times New Roman");
                bottomFormat.Size = 14;
                doc.InsertParagraph(" ", false, bottomFormat);
                doc.InsertParagraph(" ", false, bottomFormat);
                var prestator = doc.InsertParagraph("Prestator 							Beneficiar", false, bottomFormat);
                doc.InsertParagraph(prestatator + "					       		" + beneficiar);
                // Save to the output directory:

                doc.Save();
                MessageBox.Show("Documentul word a fost creat:" + fileName.Replace("\\\\", "\\"));
                // Open in Word:
                //Process.Start("WINWORD.EXE", fileName);
            }
            catch (Exception ex)
            {
                Log.Error("Save Financial Word Document "+ex.Message);
                MessageBox.Show("Inchideti documentul word pentru a putea sa il creati din nou");
            }

        }

        #endregion Methods

    }
}
