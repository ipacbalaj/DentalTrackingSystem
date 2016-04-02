using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ClientShell.Views.Shell;
using ClientShell.Views.Tabs.ListView.Patient;
using DSA.Common.Controls.Buttons;
using DSA.Common.Controls.Buttons.SymbolButton;
using DSA.Common.Infrastructure.Entities;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.Entities;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;

namespace ClientShell.Views.Tabs.ListView
{
    public class LeftListViewModel : ViewModelBase
    {
        #region Constructor

        public LeftListViewModel(ClientShellViewModel parent)
        {
            AddPatientButton = new ActionButtonViewModel("+", new DelegateCommand(OnPatientAdd), "");
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<PatientNameChangedEvent>().Subscribe(OnPatientNameChanged);
            eventAggregator.GetEvent<PatientAddedEvent>().Subscribe(OnPatientAdded);
            eventAggregator.GetEvent<PatientDeletedEvent>().Subscribe(OnPatientDeleted);
            Parent = parent;
            ImagePath = DSA.Common.Infrastructure.ImagePath.SigleIconPath;
            SearchImagePath = DSA.Common.Infrastructure.ImagePath.PatientSeadch;
            OnMergePatientsButton = new ActionButtonViewModel("Îmbinare pacienți", new DelegateCommand(OnPatientsMerge), DSA.Common.Infrastructure.ImagePath.PatientEditPath);
            MergePatientsButton = new ActionButtonViewModel("Imbină    ", new DelegateCommand(MergePatients), DSA.Common.Infrastructure.ImagePath.PatientEditPath);
            CancelMergePatientsButton = new ActionButtonViewModel("Anulează ", new DelegateCommand(OnCancelMerge), DSA.Common.Infrastructure.ImagePath.CancelIconPath);
        }

        #endregion Constructor

        #region Properties

        public ClientShellViewModel Parent { get; set; }

        private readonly IEventAggregator eventAggregator;

        private string imagePath;

        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                if (value == imagePath)
                    return;
                imagePath = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel onMergePatientsButton;
        public ActionButtonViewModel OnMergePatientsButton
        {
            get { return onMergePatientsButton; }
            set
            {
                if (value == onMergePatientsButton)
                    return;
                onMergePatientsButton = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel mergePatientsButton;
        public ActionButtonViewModel MergePatientsButton
        {
            get { return mergePatientsButton; }
            set
            {
                if (value == mergePatientsButton)
                    return;
                mergePatientsButton = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel cancelMergePatientsButton;

        public ActionButtonViewModel CancelMergePatientsButton
        {
            get { return cancelMergePatientsButton; }
            set
            {
                if (value == cancelMergePatientsButton)
                    return;
                cancelMergePatientsButton = value;
                OnPropertyChanged();
            }
        }

        private Visibility mergePatientsVisibility = Visibility.Collapsed;
        public Visibility MergePatientsVisibility
        {
            get { return mergePatientsVisibility; }
            set
            {
                if (value == mergePatientsVisibility)
                    return;
                mergePatientsVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility mergeButtonVisibility = Visibility.Visible;

        public Visibility MergeButtonVisibility
        {
            get { return mergeButtonVisibility; }
            set
            {
                if (value == mergeButtonVisibility)
                    return;
                mergeButtonVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool patientsMergeOn;
        public bool PatientsMergeOn
        {
            get { return patientsMergeOn; }
            set
            {
                if (value == patientsMergeOn)
                    return;
                patientsMergeOn = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<LocalPatient> patientsList;
        public ObservableCollection<LocalPatient> PatientsList
        {
            get { return patientsList; }
            set
            {
                if (patientsList != value)
                {
                    patientsList = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<LocalPatient> currentPatients;
        public ObservableCollection<LocalPatient> CurrentPatients
        {
            get { return currentPatients; }
            set
            {
                if (currentPatients != value)
                {
                    currentPatients = value;
                    OnPropertyChanged();
                }
            }
        }

        public UIElement SearchIcon { get; set; }
        private String searchedItem;
        public String SearchedItem
        {
            get { return searchedItem; }
            set
            {
                if (searchedItem != value)
                {
                    searchedItem = value;
                    Search(searchedItem);
                    OnPropertyChanged();
                }
            }
        }

        private PatientViewModel patientViewModel;
        public PatientViewModel PatientViewModel
        {
            get { return patientViewModel; }
            set
            {
                if (patientViewModel != value)
                {
                    patientViewModel = value;

                    OnPropertyChanged();
                }
            }
        }

        private string searchImagePath;

        public string SearchImagePath
        {
            get { return searchImagePath; }
            set
            {
                if (value == searchImagePath)
                    return;
                searchImagePath = value;
                OnPropertyChanged();
            }
        }

        private LocalPatient selectedPatient;
        public LocalPatient SelectedPatient
        {
            get { return selectedPatient; }
            set
            {
                if (selectedPatient != value)
                {
                    selectedPatient = value;
                    if (selectedPatient != null)
                    {
                        SelectedPatientName = selectedPatient.Surname + " " + selectedPatient.Name;
                        LocalCache.Instance.SelectedPatient = selectedPatient;
                        eventAggregator.GetEvent<PatientSelectedEvent>().Publish(selectedPatient);
                        //       SelectedPatient.Brush = BackgroundColors.PatientsListColorOver;
                        if (PatientsMergeOn)
                        {
                            SetPatientForMerge();
                        }
                    }
                    OnPropertyChanged();
                }
            }
        }

        private LocalPatient selectedPatientMerge;

        public LocalPatient SelectedPatientMerge
        {
            get { return selectedPatientMerge; }
            set
            {
                if (value == selectedPatientMerge)
                    return;
                selectedPatientMerge = value;
                OnPropertyChanged();
            }
        }

        private void SetPatientForMerge()
        {
            SelectedPatient.Brush = BackgroundColors.BackgroundFilled;
            if (!PatientsToMerge.Contains(SelectedPatient))
            {
                PatientsToMerge.Add(SelectedPatient);
            }
        }

        private ObservableCollection<LocalPatient> patientsToMerge = new ObservableCollection<LocalPatient>();

        public ObservableCollection<LocalPatient> PatientsToMerge
        {
            get { return patientsToMerge; }
            set
            {
                if (value == patientsToMerge)
                    return;
                patientsToMerge = value;
                OnPropertyChanged();
            }
        }

        private string selectedPatientName;
        public string SelectedPatientName
        {
            get { return selectedPatientName; }
            set
            {
                if (selectedPatientName != value)
                {
                    selectedPatientName = value;
                    OnPropertyChanged();
                }
            }
        }

        private ActionButtonViewModel addPatientButton;
        public ActionButtonViewModel AddPatientButton
        {
            get { return addPatientButton; }
            set
            {
                if (addPatientButton == value)
                    return;
                addPatientButton = value;
                OnPropertyChanged();
            }
        }

        private Visibility addPatientControlVisibility = Visibility.Collapsed;
        public Visibility AddPatientControlVisibility
        {
            get { return addPatientControlVisibility; }
            set
            {
                if (addPatientControlVisibility != value)
                {
                    addPatientControlVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion Properties

        #region Methods

        public void PopulatePatientsList(List<LocalPatient> localPatients)
        {
            PatientsList = new ObservableCollection<LocalPatient>(localPatients);
            CurrentPatients = PatientsList;
            eventAggregator.GetEvent<TotalsModifiedEvent>().Publish(new TotalsIfo
            {
                TotalHours = new TimeSpan(0),
                TotalInverventions = 0,
                TotalRevenue = 0,
                TotalProfit = 0,
                TotalPatients = localPatients.Count

            });
        }

        private void AddPatient()
        {

        }

        private void Search(string searchedWord)
        {
            searchedWord = searchedWord.ToLower();
            int wordLength = searchedWord.Length;
            // CurrentPatients = new ObservableCollection<PatientViewModel>(PatientsList.Where(item => item.Name.ToLower().Contains(searchedWord)));
            CurrentPatients = new ObservableCollection<LocalPatient>(PatientsList.Where(item => (item.Name != null && item.Name.Length >= wordLength && item.Name.ToLower().Substring(0, wordLength) == searchedWord) ||
                              item.Surname != null && item.Surname.Length >= wordLength && item.Surname.ToLower().Substring(0, wordLength) == searchedWord));
            SelectedPatient = CurrentPatients.FirstOrDefault();
        }

        private void OnPatientAdd()
        {
            AddPatientControlVisibility = Visibility.Visible;
            PatientViewModel = new PatientViewModel(this);
        }

        public async void SavePatient(LocalPatient addedPatient)
        {
            if (!string.IsNullOrEmpty(SearchedItem))
            {
                Search(SearchedItem);
            }
            AddPatientControlVisibility = Visibility.Collapsed;
            PatientsList.Add(addedPatient);
            //SelectedPatient = addedPatient;
        }

        private void OnPatientNameChanged(object patient)
        {
            var patientToEdit = (LocalPatient)patient;
            var editedPatient = PatientsList.FirstOrDefault(item => item.Id == patientToEdit.Id);
            editedPatient.AllName = patientToEdit.AllName;
            var currentDate = DateTime.Now;
            //            if (currentDate.Day == ((LocalPatient)patient).BirthDate.Day && currentDate.Month == ((LocalPatient)patient).BirthDate.Month)
            {
                var poEdit =
                    LocalCache.Instance.PatientsRepository.Patients.FirstOrDefault(item => item.Id == editedPatient.Id);
                poEdit = patientToEdit;
                // LocalCache.Instance.PatientsRepository.BirthDays.Add(ed);
                LocalCache.Instance.PatientsRepository.SetBirthDays();
                Parent.InterventionsGeneralDetailsViewModel.InitNames();
            }

            //PatientsList.Add(editedPatient);
        }

        private void OnPatientAdded(object patient)
        {
            var patientSelected = (LocalPatient)patient;
            PatientsList.Add(patientSelected);
            CurrentPatients = PatientsList;
            SelectedPatient = patientSelected;
            eventAggregator.GetEvent<TotalsModifiedEvent>().Publish(new TotalsIfo
            {
                TotalHours = new TimeSpan(0),
                TotalInverventions = 0,
                TotalRevenue = 0,
                TotalProfit = 0,
                TotalPatients = 1

            });
            LocalCache.Instance.PatientsRepository.Patients.Add(patientSelected);
            LocalCache.Instance.PatientsRepository.SetBirthDays();
            Parent.InterventionsGeneralDetailsViewModel.InitNames();
        }

        public void OnPatientDoubleClick()
        {
            Parent.OnPatientDoubleCLicked();
        }

        private void OnPatientDeleted(int id)
        {
            double deletedRevenue = 0;
            double deletedProfit = 0;
            double time = 0;
            int interventionsNb = 0;
            foreach (var localIntervention in SelectedPatient.Interventions)
            {
                deletedRevenue += localIntervention.Revenue;
                deletedProfit += localIntervention.Percent;
                interventionsNb++;
                time += localIntervention.DateHourDetail.Duration.TotalMinutes;
            }
            Parent.OnChangeTotalInfo(new TotalsIfo
            {
                TotalHours = new TimeSpan(-(long)time),
                TotalInverventions = -interventionsNb,
                TotalRevenue = -deletedRevenue,
                TotalProfit = -deletedProfit,
                TotalPatients = -1

            });
            PatientsList.Remove(SelectedPatient);
            SelectedPatient = PatientsList.FirstOrDefault();
        }

        private void OnPatientsMerge()
        {
            PatientsMergeOn = true;
            MergePatientsVisibility = Visibility.Visible;
            MergeButtonVisibility = Visibility.Collapsed;
        }

        private async void MergePatients()
        {
            PatientsMergeOn = false;            
            await DatabaseHandler.Instance.MergePatients(PatientsToMerge.Select(item => item.Id).ToList());
            PatientsToMerge = new ObservableCollection<LocalPatient>();
            MergeButtonVisibility = Visibility.Visible;
            MergePatientsVisibility = Visibility.Collapsed;
        }

        private void OnCancelMerge()
        {
            PatientsToMerge = new ObservableCollection<LocalPatient>();
            MergePatientsVisibility = Visibility.Collapsed;
            MergeButtonVisibility = Visibility.Visible;
        }


        public void RemovePatientFromMergeList()
        {
            PatientsToMerge.Remove(SelectedPatientMerge);
        }

        #endregion Methods

    }
}
