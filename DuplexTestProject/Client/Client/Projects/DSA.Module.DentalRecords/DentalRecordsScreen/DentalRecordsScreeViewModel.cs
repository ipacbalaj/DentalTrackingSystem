using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Controls.Options;
using DSA.Common.Infrastructure.ObjectList;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.Entities;
using DSA.Database.Model.Helpers;
using DSA.Module.DentalRecords.Filters.FilterItem.SelectedItemTile;
using DSA.Module.DentalRecords.Helpers;
using DSA.Module.DentalRecords.Interventions.AddInterventionTile;
using DSA.Module.DentalRecords.Interventions.InterventionsList.InterventionMonthControl;
using DSA.Module.DentalRecords.Interventions.InterventionsList.InterventionYearControl;
using DSA.Module.DentalRecords.Interventions.InterventionsList.InterventionYearControl.YearList;
using DSA.Module.DentalRecords.Options.DisplayOptions;
using DSA.Module.DentalRecords.Options.OperationOptions;
using DSA.Module.DentalRecords.Options.UploadInterventionsFIle;
using DSA.Module.DentalRecords.Tooth.ToothTileList;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace DSA.Module.DentalRecords.DentalRecordsScreen
{
    public class DentalRecordsScreeViewModel:ViewModelBase
    {
        #region Constructor
        public DentalRecordsScreeViewModel()
        {
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            unityContainer = ServiceLocator.Current.GetInstance<IUnityContainer>();
            eventAggregator.GetEvent<UpdateDentalRecordsEvent>().Subscribe(UpdateDentalRecordsScreen);
            eventAggregator.GetEvent<ReportsInterventionsEvent>().Subscribe(OnShowReports);
            eventAggregator.GetEvent<IntervalSetEvent>().Subscribe(OnPaymentChanged);                  
            //ToothTileListViewModel=new ToothTileListViewModel();
        }
        #endregion Constructor

        #region Properties

        private bool initialized;

        public List<InterventionDetails> intervalInterventions;

        private readonly IEventAggregator eventAggregator;
        private readonly IUnityContainer unityContainer;

        private Brush contentBackground=BackgroundColors.JurnalColor;
        public Brush ContentBackground
        {
            get { return contentBackground; }
            set
            {
                if (value == contentBackground)
                    return;
                contentBackground = value;
                OnPropertyChanged();
            }
        }

        private ToothTileListViewModel toothTileListViewModel;
        public ToothTileListViewModel ToothTileListViewModel
        {
            get { return toothTileListViewModel; }
            set
            {
                if (toothTileListViewModel != value)
                {
                    toothTileListViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        private AddInterventionTileViewModel addInterventionTileViewModel;
        public AddInterventionTileViewModel AddInterventionTileViewModel
        {
            get { return addInterventionTileViewModel; }
            set
            {
                if (addInterventionTileViewModel == value)
                    return;
                addInterventionTileViewModel = value;
                OnPropertyChanged();
            }
        }

        private Brush backgroundColor=BackgroundColors.orange;

        public Brush BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                if (value == backgroundColor)
                    return;
                backgroundColor = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<InterventionDetails> interventions;
        public ObservableCollection<InterventionDetails> Interventions
        {
            get { return interventions; }
            set
            {
                if (interventions != value)
                {
                    interventions = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<InterventionDetails> seenInterventions;
        public ObservableCollection<InterventionDetails> SeenInterventions
        {
            get { return seenInterventions; }
            set
            {
                if (seenInterventions != value)
                {
                    seenInterventions = value;
                    OnPropertyChanged();
                }
            }
        }

        private OptionsViewModel optionsViewModel;
        public OptionsViewModel OptionsViewModel
        {
            get { return optionsViewModel; }
            set
            {
                if (optionsViewModel != value)
                {
                    optionsViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        private DisplayOptionsViewModel displayOptionsViewModel;
        public DisplayOptionsViewModel DisplayOptionsViewModel
        {
            get { return displayOptionsViewModel; }
            set
            {
                if (displayOptionsViewModel != value)
                {
                    displayOptionsViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        //this will be used -we have a list of years
        private YearListViewModel yearListViewModel;
        public YearListViewModel YearListViewModel
        {
            get { return yearListViewModel; }
            set
            {
                if (yearListViewModel != value)
                {
                    yearListViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

//        private YearInterventionsViewModel yearInterventionsViewModel;
//        public YearInterventionsViewModel YearInterventionsViewModel
//        {
//            get { return yearInterventionsViewModel; }
//            set
//            {
//                if (yearInterventionsViewModel != value)
//                {
//                    yearInterventionsViewModel = value;
//                    OnPropertyChanged();
//                }
//            }
//        }
        #endregion Properties

        #region Methods

        private void UpdateDentalRecordsScreen(Object anObj)
        {
            if (!initialized)
            {
                PopulateYearsList();               
                OptionsViewModel = CreateOptionsPopuUp();
                initialized = true;
            }
            AddInterventionTileViewModel = new AddInterventionTileViewModel(this);
          //  PopulateInterventions();
          //  ToothTileListViewModel.PopulateTeethList();
        }

        private void PopulateInterventions()
        {
            List<LocalIntervention> tempInterventions = new List<LocalIntervention>();
            foreach (var patient in LocalCache.Instance.PatientsRepository.Patients)
            {
                tempInterventions.AddRange(patient.Interventions);
            }
            Interventions = new ObservableCollection<InterventionDetails>();
            foreach (var localIntervention in tempInterventions)
            {
                Interventions.Add(localIntervention.ToInterventionDetails());
            }
            SeenInterventions=new ObservableCollection<InterventionDetails>(Interventions);
        }

        private OperationOptionsViewModel operationOptionsViewModel;
        private OptionsViewModel CreateOptionsPopuUp()
        {
            OptionsViewModel optionsViewModel = new OptionsViewModel();
            var dict = new Dictionary<string, UIElement>();
            
//            DisplayOptionsViewModel = new DisplayOptionsViewModel(this);
//            var content = new DisplayOptionsView() { DataContext =  DisplayOptionsViewModel};
//            dict.Add("Filtre", content);

            UploadInterventionsFileViewModel fileOptions = new UploadInterventionsFileViewModel(this);            
            var contentFile = new UploadInterventionsFileView() { DataContext = fileOptions };
            dict.Add("IncarcaFisier", contentFile);

            operationOptionsViewModel =new OperationOptionsViewModel(this);
            var operationOptionsContent = new OperationOptionsView() {DataContext = operationOptionsViewModel};
            dict.Add("Suplimentare", operationOptionsContent);

            optionsViewModel.SetContent(dict);
            return optionsViewModel;
        }

        public void FilterInterventions()
        {
//            var selectedFilters = DisplayOptionsViewModel.FiltersViewModel.FilterItemList.Where(item => item.IsChecked);
//            List<InterventionDetails> tempInterventions=new List<InterventionDetails>();
//            foreach (var interventionDetailse in Interventions)
//            {
//                tempInterventions.Add(interventionDetailse);
//            }
//            foreach (var filterItemViewModel in selectedFilters)
//            {
//                var selectedItems = GetSelectedItemsNames(filterItemViewModel.SelectedItemTileViewModels);
//                switch (filterItemViewModel.SettingsItemName)
//                {                
//                      case "Zona:":
//                            tempInterventions = tempInterventions.Where(item => selectedItems.Contains(item.Area)).ToList();                                              
//                        break;
//                     case "Manopera:":
//                        tempInterventions = tempInterventions.Where(item => selectedItems.Contains(item.Work)).ToList();                        
//                        break;
//                     case "Material:":
//                        tempInterventions = tempInterventions.Where(item => selectedItems.Contains(item.Material)).ToList();                          
//                        break;
//                    case "Locatie:":
//                        tempInterventions = tempInterventions.Where(item => selectedItems.Contains(item.Location)).ToList();   
//                        break;
//                    case "Luna:":
//                       // tempInterventions =tempInterventions.Where(item => item.Date. == filterItemViewModel.SelectedSettingsItem.Name).ToList();
//                        break;
//                    case "An:":
//                        break;
//
//                }
//            }
//            SeenInterventions=new ObservableCollection<InterventionDetails>(tempInterventions);
        }

        private void OnPaymentChanged(bool isIntervalSet)
        {
           //var intervention = (InterventionDetails) anObj;            
           
        }

        private List<string> GetSelectedItemsNames(ObservableCollection<SelectedItemTileViewModel> settingsItems)
        {
            return settingsItems.Select(item => item.Name).ToList();
        }

//        private void PopulateForTesting()
//        {
//            YearInterventionsViewModel=new YearInterventionsViewModel();
//            YearInterventionsViewModel.Year = 2015;
//            YearInterventionsViewModel.MonthList=new ObjectList<MonthInterventionsViewModel>(true);
//            var currentYearvalues = LocalCache.Instance.InterventionsDictionary[2015];
//
//            foreach (var key in currentYearvalues.Keys)
//            {
//                YearInterventionsViewModel.MonthList.Add(new MonthInterventionsViewModel(currentYearvalues[key])
//                {
//                    MonthName = LocalCache.Instance.MonthsNames[key],
//                });
//            }
//        }

        public void PopulateYearsList()
        {
            YearListViewModel=new YearListViewModel(this);
            YearListViewModel.PopulateYears(LocalCache.Instance.InterventionsDictionary);
        }

        private void OnShowReports(Object anoB)
        {
            YearListViewModel.FIlter();
        }

        public void SetPaymentInterval(bool payed)
        {
            var startDate = LocalCache.Instance.PaymentInterval.StartDate;
            var endDate = LocalCache.Instance.PaymentInterval.EndDate;
            long startMili = LocalCache.Instance.PaymentInterval.StartMili;
            long endMili = LocalCache.Instance.PaymentInterval.EndMili;
            intervalInterventions=new List<InterventionDetails>();
           // List<int> interventionsIds=new List<int>();
            for (int i = startDate.Year; i <= endDate.Year; i++)
            {
                if (i == endDate.Year)
                {
//                    interventionsIds.AddRange(YearListViewModel.YearList.List.FirstOrDefault(item => item.Year == startDate.Year).SetPaymentInterval(startDate, endDate, false,payed));
                    intervalInterventions.AddRange(YearListViewModel.YearList.List.FirstOrDefault(item => item.Year == startDate.Year).SetPaymentInterval(startDate, endDate, false, payed,startMili,endMili));
                }
                else
                {
                    intervalInterventions.AddRange(YearListViewModel.YearList.List.FirstOrDefault(item => item.Year == startDate.Year).SetPaymentInterval(startDate, endDate, true, payed,startMili,endMili));
//                    interventionsIds.AddRange(YearListViewModel.YearList.List.FirstOrDefault(item => item.Year == startDate.Year).SetPaymentInterval(startDate, endDate, true,payed));
                }
            } 
            DatabaseHandler.Instance.SetInterventionsAsPayed(intervalInterventions.Select(item=>item.Id).ToList(), true);
        }

        public void SetPayedUnpayed()
        {
            operationOptionsViewModel.SetPayedUnPayed();
        }

        #endregion Methods
    }
}
