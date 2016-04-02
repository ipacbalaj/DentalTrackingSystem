using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DSA.Common.Controls.Buttons;
using DSA.Common.Controls.Buttons.SymbolButton;
using DSA.Common.Controls.InputBox;
using DSA.Common.Infrastructure;
using DSA.Common.Infrastructure.ViewModel;
using DSA.FileUpload.ExcelFile;
using DSA.Module.DentalRecords.DentalRecordsScreen;
using log4net;
using Microsoft.Practices.Prism.Commands;

namespace DSA.Module.DentalRecords.Options.UploadInterventionsFIle
{
   public class UploadInterventionsFileViewModel:ViewModelBase
   {
       private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

       #region Constructor

       public UploadInterventionsFileViewModel(DentalRecordsScreeViewModel parent)
       {
           Parent = parent;
           UploadSettingsButton = new SymbolIconButtonViewModel(new DelegateCommand(OnSettingsExcelFile), "Încarcă Setări din Excel");//new ActionButtonViewModel("Incarca Setari din Excel", new DelegateCommand(OnSettingsExcelFile), ImagePath.AddPatientPath);
           UploadPatientsButton = new SymbolIconButtonViewModel(new DelegateCommand(OnPatientsExcelFile), "Încarcă Intervenții din Excel");
           UploadMonthIntervention = new SymbolIconButtonViewModel(new DelegateCommand(OnMonthInterventions), "Încarcă Intervenții din Excel(Lună)");
               //new ActionButtonViewModel("Incarca Interventii din Excel", new DelegateCommand(OnPatientsExcelFile), ImagePath.AddPatientPath);
       }
       #endregion Constructor

       #region Properties

       public DentalRecordsScreeViewModel Parent { get; set; }

       private SymbolIconButtonViewModel uploadSettingsButton;
       public SymbolIconButtonViewModel UploadSettingsButton
       {
           get { return uploadSettingsButton; }
           set
           {
               if (uploadSettingsButton == value)
                   return;
               uploadSettingsButton = value;
               OnPropertyChanged();
           }
       }

       private SymbolIconButtonViewModel uploadMonthIntervention;

       public SymbolIconButtonViewModel UploadMonthIntervention
       {
           get { return uploadMonthIntervention; }
           set
           {
               if (value == uploadMonthIntervention)
                   return;
               uploadMonthIntervention = value;
               OnPropertyChanged();
           }
       }

       private SymbolIconButtonViewModel uploadPatientsButton;
       public SymbolIconButtonViewModel UploadPatientsButton
       {
           get { return uploadPatientsButton; }
           set
           {
               if (uploadPatientsButton == value)
                   return;
               uploadPatientsButton = value;
               OnPropertyChanged();
           }
       }

       private string path;

       public string Path
       {
           get { return path; }
           set
           {
               if (value == path)
                   return;
               path = value;
               OnPropertyChanged();
           }
       }

       #endregion Properties

       #region Methods

       private void OnPatientsExcelFile()
       {
           try
           {
               ExcelParser.Instance.GetConstants(@Path);
//               Log.Error("Load ERROR:" + ex.Message);
           }
           catch (Exception ex)
           {

           }
           MessageBox.Show("Interventii Incarcate");
           // ExcelParser.Instance.GetConstants(@"C:\Users\ionut\Dropbox\Poli\DTS\finalProject\Resources\GOL.xlsx");
           //ExcelParser.Instance.GetConstants(@"C:\Users\Pac-Balaj ionutzela2\Dropbox\Poli\DTS\finalProject\Resources\GOL.xlsx");
       }

       private void OnSettingsExcelFile()
       {
           ExcelParser.Instance.SetSettings(@Path);
           MessageBox.Show("Setari Incarcate");
         //  ExcelParser.Instance.SetSettings(@"C:\Users\ionut\Dropbox\Poli\DTS\finalProject\Resources\GOL.xlsx");
           //ExcelParser.Instance.SetSettings(@"C:\Users\Pac-Balaj ionutzela2\Dropbox\Poli\DTS\finalProject\Resources\GOL.xlsx");
       }

       private void OnMonthInterventions()
       {
           var dialog = new MyDialog();

           if (dialog.ShowDialog() == true)
           {
                ExcelParser.Instance.SetFileName(@Path);    
               ExcelParser.Instance.GetPatientsValues(dialog.ResponseText);
           }
       }

       #endregion Methods

    }
}
