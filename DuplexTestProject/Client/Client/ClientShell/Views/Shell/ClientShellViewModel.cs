using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ClientShell.Bootstrapper;
using ClientShell.Helpers;
using ClientShell.Views.PatientsList;
using ClientShell.Views.Tabs.HorizontalTabs;
using ClientShell.Views.Tabs.ListView;
using DSA.Common.Controls.Buttons;
using DSA.Common.Controls.Buttons.SymbolButton;
using DSA.Common.Controls.Loading.MetroLoading;
using DSA.Common.Controls.LoginControls.ChangePassword;
using DSA.Common.Infrastructure.Entities;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.EfSQlLite;
using DSA.Database.Model.Entities.SettingsEntities;
using DSA.FileUpload.ExcelFile;
using DSA.Module.DentalRecords.Interventions.InterventionsGeneralDetails;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace ClientShell.Views.Shell
{
    public class ClientShellViewModel : ViewModelBase
    {
        #region Construcor

        public ClientShellViewModel(IEventAggregator eventAggregator, IUnityContainer unityContainer, IRegionManager regionManager)
        {
            this.eventAggregator = eventAggregator;
            ListViewModel = new LeftListViewModel(this);
            HorizontalTabsViewModel = new HorizontalTabsViewModel();
            RegionHandlers = new RegionHandlers(eventAggregator, unityContainer, regionManager);
            RegionHandlers.Parent = this;
            eventAggregator.GetEvent<UserLoginEvent>().Subscribe(ConnectToServerAndLogin);
            ChangeCredentialsButton = new ActionButtonViewModel("", new DelegateCommand(OnChangeCredentials), DSA.Common.Infrastructure.ImagePath.DentistProfile);
            this.eventAggregator.GetEvent<TotalsModifiedEvent>().Subscribe(OnChangeTotalInfo);
            InterventionsGeneralDetailsViewModel=new InterventionsGeneralDetailsViewModel();
        }

        #endregion Constructor

        #region Properties

        private string userName;

        public string UserName
        {
            get { return userName; }
            set
            {
                if (value == userName)
                    return;
                userName = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel changeCredentialsButton;
        public ActionButtonViewModel ChangeCredentialsButton
        {
            get { return changeCredentialsButton; }
            set
            {
                if (value == changeCredentialsButton)
                    return;
                changeCredentialsButton = value;
                OnPropertyChanged();
            }
        }

        private RegionHandlers regionHandlers;
        public RegionHandlers RegionHandlers
        {
            get { return regionHandlers; }
            set
            {
                if (regionHandlers != value)
                {
                    regionHandlers = value;
                    OnPropertyChanged();
                }
            }
        }

        private readonly IEventAggregator eventAggregator;

        private LeftListViewModel listViewModel;
        public LeftListViewModel ListViewModel
        {
            get { return listViewModel; }
            set
            {
                if (listViewModel == value)
                    return;
                listViewModel = value;
                OnPropertyChanged();
            }
        }

        private PatientListViewModel patientListViewModel;
        public PatientListViewModel PatientListViewModel
        {
            get { return patientListViewModel; }
            set
            {
                if (value == patientListViewModel)
                    return;
                patientListViewModel = value;
                OnPropertyChanged();
            }
        }

        private HorizontalTabsViewModel horizontalTabsViewModel;
        public HorizontalTabsViewModel HorizontalTabsViewModel
        {
            get { return horizontalTabsViewModel; }
            set
            {
                if (horizontalTabsViewModel != value)
                {
                    horizontalTabsViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        private MetroLoadingViewModel metroLoadingViewModel;
        public MetroLoadingViewModel MetroLoadingViewModel
        {
            get { return metroLoadingViewModel; }
            set
            {
                if (value == metroLoadingViewModel)
                    return;
                metroLoadingViewModel = value;
                OnPropertyChanged();
            }
        }

        private InterventionsGeneralDetailsViewModel interventionsGeneralDetailsViewModel;
        public InterventionsGeneralDetailsViewModel InterventionsGeneralDetailsViewModel
        {
            get { return interventionsGeneralDetailsViewModel; }
            set
            {
                if (value == interventionsGeneralDetailsViewModel)
                    return;
                interventionsGeneralDetailsViewModel = value;
                OnPropertyChanged();
            }
        }

        #region Visibility

        private Visibility isLoginVisible = Visibility.Visible;
        public Visibility IsLoginVisible
        {
            get { return isLoginVisible; }
            set
            {
                if (isLoginVisible != value)
                {
                    isLoginVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility isContentVisible = Visibility.Collapsed;
        public Visibility IsContentVisible
        {
            get { return isContentVisible; }
            set
            {
                if (isContentVisible != value)
                {
                    isContentVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion Visibility

        #endregion Properties

        #region Methods

        private async void OnUserLoggedIn()
        {
            //PatientListViewModel = new PatientListViewModel();
            var patients = await DatabaseHandler.Instance.GetPatients();
            new Task(() => LocalCache.Instance.PatientsRepository.SetPatient(patients)).Start();
            //   new Task(()=>LocalCache.Instance.PopulateInterventiosDictionary()).Start();
            //LocalCache.Instance.AddYearToInterventionsDictionary();
            new Task(() => ListViewModel.PopulatePatientsList(LocalCache.Instance.PatientsRepository.Patients)).Start();
            // new Task(() => PatientListViewModel.PopulatePatientsList(patients)).Start();   
            PopulateSettingsItems();
            eventAggregator.GetEvent<DataLoadedEvent>().Publish(null);
            InterventionsGeneralDetailsViewModel.InitNames();
        }

        private void ConnectToServerAndLogin(string loggedUser)
        {
            IsLoginVisible = Visibility.Collapsed;
            IsContentVisible = Visibility.Visible;
            UserName = loggedUser;
            MetroLoadingViewModel = new MetroLoadingViewModel(true);
            MetroLoadingViewModel.UserName = UserName;
            AnimateWhenLoading();          
        }

        public async void AnimateWhenLoading()
        {
            TaskScheduler _uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
//            LocalCache.Instance.ProgramInfo = DatabaseHandler.Instance.GetProgramInfo();
//            var programInfo = DatabaseHandler.Instance.GetProgramInfo();
//            if (programInfo != null)
//            {
//                LocalCache.Instance.ProgramInfo = programInfo;
//                await LoadDynamicHelper.IterateThroughFiles(programInfo.ExportFilePath);
//            }
            
            Action DoInBackground = new Action(() =>
            {
                var programInfo = DatabaseHandler.Instance.GetProgramInfo();
//                if (programInfo != null)
//                {
//                    LocalCache.Instance.ProgramInfo = programInfo;
//                    LoadDynamicHelper.IterateThroughFiles(programInfo.ExportFilePath);
//                }
                LocalCache.Instance.Initialize();
                HorizontalTabsViewModel.OnSelectReportsTab("Pacienti");
                OnUserLoggedIn();
            });

            Action DoOnUiThread = new Action(() =>
            {
                MetroLoadingViewModel.ShouldContinueAnimation = false;
            });

            // start the background task
            var t1 = Task.Factory.StartNew(() => DoInBackground());
            // when t1 is done run t1..on the Ui thread.
            var t2 = t1.ContinueWith(t => DoOnUiThread(), _uiScheduler);
        }

        private async void PopulateSettingsItems()
        {

            //            LocalCache.Instance.SettingsItems = new Dictionary<LocalCache.SettingsItemEnum, List<SettingsItem>>();
            //            List<SettingsItem> areas = new List<SettingsItem>(await DatabaseHandler.Instance.GetAreas());
            //            LocalCache.Instance.SettingsItems.Add(LocalCache.SettingsItemEnum.Zona, areas);
            //            List<SettingsItem> works = new List<SettingsItem>(await DatabaseHandler.Instance.GetWorks());
            //            LocalCache.Instance.SettingsItems.Add(LocalCache.SettingsItemEnum.Manopera, works);
            //            List<SettingsItem> locations = new List<SettingsItem>(await DatabaseHandler.Instance.GetlLocations());
            //            LocalCache.Instance.SettingsItems.Add(LocalCache.SettingsItemEnum.Locatie, locations);
            //            List<SettingsItem> materials = new List<SettingsItem>(await DatabaseHandler.Instance.GetMaterials());
            //            LocalCache.Instance.SettingsItems.Add(LocalCache.SettingsItemEnum.Material, materials);
            var monthsList = new List<SettingsItem>();
            for (int i = 1; i <= 12; i++)
            {
                monthsList.Add(new SettingsItem()
                {
                    Id = i,
                    Name = LocalCache.Instance.months[i-1]//DateTimeFormatInfo.CurrentInfo.GetMonthName(i)
                });
            }
            LocalCache.Instance.SettingsItems.Add(LocalCache.SettingsItemEnum.Luna, monthsList);
        }

        private void OnFiltersApplied(Object anObj)
        {
            HorizontalTabsViewModel.OnSelectReportsTab("Rapoarte");
        }

        public void OnPatientDoubleCLicked()
        {
            HorizontalTabsViewModel.OnSelectReportsTab("Fisa Pacient");
        }

        private void OnChangeCredentials()
        {
            ChangePasswordView changePasswordView=new ChangePasswordView(UserName);
            changePasswordView.ShowDialog();
        }

        public void OnChangeTotalInfo(TotalsIfo totalsIfo)
        {
            InterventionsGeneralDetailsViewModel.TotalHours += totalsIfo.TotalHours;
            InterventionsGeneralDetailsViewModel.TotalProfit += totalsIfo.TotalProfit;
            InterventionsGeneralDetailsViewModel.TotalRevenue += totalsIfo.TotalRevenue;
            InterventionsGeneralDetailsViewModel.TotalInverventions += totalsIfo.TotalInverventions;
            InterventionsGeneralDetailsViewModel.TotalPatients += totalsIfo.TotalPatients;
        }

        #endregion Methods
    }
}
