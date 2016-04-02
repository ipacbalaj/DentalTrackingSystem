using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DSA.Common.Controls.Buttons.IconButtons;
using DSA.Common.Controls.Buttons.SymbolButton;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.Entities;
using DSA.FileUpload.ExcelFile;
using DSA.Module.DentalRecords.DentalRecordsScreen;
using Microsoft.Practices.Prism.Commands;

namespace DSA.Module.DentalRecords.Options.OperationOptions
{
    public class OperationOptionsViewModel:ViewModelBase
    {
        #region Constructor

        public OperationOptionsViewModel(DentalRecordsScreeViewModel parent)
        {
            Parent = parent;
            SeePaymentDetailsButton = new SymbolIconButtonViewModel(new DelegateCommand(OnSeeInfo), "Vezi informații selectate");
            SetPayedButton = new SymbolIconButtonViewModel(new DelegateCommand(OnSetPayed), "Setează Plătite");
            SetAsNotPayedButton = new SymbolIconButtonViewModel(new DelegateCommand(OnSetUnPayed), "Setează Neplătite");
            SaveIntervalDetailsButton=new SymbolIconButtonViewModel(new DelegateCommand(OnSaveIntervalDetails), "Exportă Xls");
//            OnMergePatientsButton = new SymbolIconButtonViewModel(new DelegateCommand(OnMergePatients), "Imbină pacienți");
        }

        #endregion Constructor

        #region Properties

        public DentalRecordsScreeViewModel Parent { get; set; }

        private bool shouldSetTillSelected;

        public bool ShouldSetTillSelected
        {
            get { return shouldSetTillSelected; }
            set
            {
                if (value == shouldSetTillSelected)
                    return;
                shouldSetTillSelected = value;
                LocalCache.Instance.PaymentInterval.ShouldSetInterval = value;
                OnPropertyChanged();
                if (shouldSetTillSelected)
                {
                    if(Parent.intervalInterventions!=null)
                    foreach (var intervalIntervention in Parent.intervalInterventions)
                    {
                        intervalIntervention.IncludedInLastPaymentInterval = false;
                    }
                    MessageBox.Show("Selectati intervalul de inteventii");
                }
            }
        }

        private bool payedUnpayed;

        public bool PayedUnPayed
        {
            get { return payedUnpayed; }
            set
            {
                if (value == payedUnpayed)
                    return;
                payedUnpayed = value;
                OnPropertyChanged();
            }
        }

        private LocalDentalPaymentInfo paymentInfo=new LocalDentalPaymentInfo();

        public LocalDentalPaymentInfo PaymentInfo
        {
            get { return paymentInfo; }
            set
            {
                if (value == paymentInfo)
                    return;
                paymentInfo = value;
                OnPropertyChanged();
            }
        }

        private SymbolIconButtonViewModel seePaymentDetailsButton;

        public SymbolIconButtonViewModel SeePaymentDetailsButton
        {
            get { return seePaymentDetailsButton; }
            set
            {
                if (value == seePaymentDetailsButton)
                    return;
                seePaymentDetailsButton = value;
                OnPropertyChanged();
            }
        }

        private SymbolIconButtonViewModel setPayedButton;

        public SymbolIconButtonViewModel SetPayedButton
        {
            get { return setPayedButton; }
            set
            {
                if (value == setPayedButton)
                    return;
                setPayedButton = value;
                OnPropertyChanged();
            }
        }

        private SymbolIconButtonViewModel setAsNotPayedButton;

        public SymbolIconButtonViewModel SetAsNotPayedButton
        {
            get { return setAsNotPayedButton; }
            set
            {
                if (value == setAsNotPayedButton)
                    return;
                setAsNotPayedButton = value;
                OnPropertyChanged();
            }
        }

        private SymbolIconButtonViewModel saveIntervalDetailsButton;

        public SymbolIconButtonViewModel SaveIntervalDetailsButton
        {
            get { return saveIntervalDetailsButton; }
            set
            {
                if (value == saveIntervalDetailsButton)
                    return;
                saveIntervalDetailsButton = value;
                OnPropertyChanged();
            }
        }

        public void SetPayedUnPayed()
        {
            PayedUnPayed = !PayedUnPayed;
            if (PayedUnPayed)
            {
                OnSetPayed();
            }
            else
            {
                OnSetUnPayed();
            }
        }

        #endregion Properties

        #region Methods

        private async void OnSeeInfo()
        {
          //  PaymentInfo = await DatabaseHandler.Instance.getLastDentalPaymentInfo();            
//            LocalDentalPaymentInfo localDentalPaymentInfo=new LocalDentalPaymentInfo();
//            localDentalPaymentInfo.Date = DateTime.Now;
//            localDentalPaymentInfo.Name = DateTime.Now.ToShortDateString();
//            if(Parent.intervalInterventions!=null)
//            foreach (var intervalIntervention in Parent.intervalInterventions)
//            {
//                if (intervalIntervention.WasPayedByDental)
//                {
//                    localDentalPaymentInfo.TotalPercent += intervalIntervention.Percent;
//                    localDentalPaymentInfo.TotalRevenue += intervalIntervention.Revenue;
//                }
//            }
//        //    ShouldSetTillSelected = false;
////            foreach (var intervalIntervention in Parent.intervalInterventions)
////            {
////                intervalIntervention.IncludedInLastPaymentInterval = false;
////            }
//            PaymentInfo = localDentalPaymentInfo;

            var info=new LocalDentalPaymentInfo();

            foreach (var selectedIntervention in LocalCache.Instance.SelectedInterventions)
            {
                foreach (var interventionDetailse in selectedIntervention.Value)
                {
                    info.TotalPercent += interventionDetailse.Percent;
                    info.TotalRevenue += interventionDetailse.Revenue;
                }
            }
            PaymentInfo = info;
        }

        private async void OnSetPayed()
        {
            Parent.YearListViewModel.ChangePaymentStatus(true);

            //ShouldSetTillSelected = false;
            //DatabaseHandler.Instance.AddIntervalInfo(PaymentInfo);
        }

        private void OnSetUnPayed()
        {
            Parent.YearListViewModel.ChangePaymentStatus(false);
        }

        private void OnSaveIntervalDetails()
        {
            var interventionDetailses = GetInterventionDetailses();

            ExcelExporter.ExportExcelFile(interventionDetailses);
        }

        private static List<InterventionDetails> GetInterventionDetailses()
        {
            List<InterventionDetails> interventionDetailses = new List<InterventionDetails>();

            foreach (var selectedIntervention in LocalCache.Instance.SelectedInterventions)
            {
                foreach (var interventionDetailse in selectedIntervention.Value)
                {
                    interventionDetailses.Add(interventionDetailse);
                }
            }
            return interventionDetailses;
        }

        private void OnMergePatients()
        {
            
        }

        #endregion Methods
    }
}
