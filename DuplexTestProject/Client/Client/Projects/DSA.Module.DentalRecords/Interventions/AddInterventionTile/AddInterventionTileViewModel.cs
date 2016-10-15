using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Controls.Buttons;
using DSA.Common.Infrastructure;
using DSA.Common.Infrastructure.Entities;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.EfSQlLite;
using DSA.Database.Model.Entities;
using DSA.Database.Model.Entities.SettingsEntities;
using DSA.Database.Model.Helpers;
using DSA.Database.Model.Repositories;
using DSA.Module.DentalRecords.DentalRecordsScreen;
using DSA.Module.DentalRecords.Helpers;
using log4net;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;

namespace DSA.Module.DentalRecords.Interventions.AddInterventionTile
{
    public class AddInterventionTileViewModel : ViewModelBase
    {
        #region Constructor

        public AddInterventionTileViewModel(DentalRecordsScreeViewModel parent)
        {
            Parent = parent;
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            SaveIntevertionButton = new ActionButtonViewModel("Salvează  ", new DelegateCommand(Save), ImagePath.SaveIconPath);
            CancelIntevertionButton = new ActionButtonViewModel("Anulează ", new DelegateCommand(OnCancel), ImagePath.CancelIconPath);
            InitList();
            IsPatientComboFocused = true;
            InitPercentage();
        }

        #endregion Constructor

        #region Lists

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

        private LocalPatient selectedPatient;
        public LocalPatient SelectedPatient
        {
            get { return selectedPatient; }
            set
            {
                if (selectedPatient != value)
                {
                    selectedPatient = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<SettingsItem> localLocations;
        public ObservableCollection<SettingsItem> LocalLocations
        {
            get { return localLocations; }
            set
            {
                if (localLocations != value)
                {
                    localLocations = value;
                    OnPropertyChanged();
                }
            }
        }

        private SettingsItem selectedLocation;
        public SettingsItem SelectedLocation
        {
            get { return selectedLocation; }
            set
            {
                if (selectedLocation != value)
                {
                    selectedLocation = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<LocalTechnician> technicians;

        public ObservableCollection<LocalTechnician> Technicians
        {
            get { return technicians; }
            set
            {
                if (value == technicians)
                    return;
                technicians = value;
                OnPropertyChanged();
            }
        }

        private LocalTechnician selectedTechnician;
        public LocalTechnician SelectedTechnician
        {
            get { return selectedTechnician; }
            set
            {
                if (value == selectedTechnician)
                    return;
                selectedTechnician = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SettingsItem> materials;
        public ObservableCollection<SettingsItem> Materials
        {
            get { return materials; }
            set
            {
                if (materials != value)
                {
                    materials = value;
                    OnPropertyChanged();
                }
            }
        }

        private SettingsItem selectedMaterial;
        public SettingsItem SelectedMaterial
        {
            get { return selectedMaterial; }
            set
            {
                if (selectedMaterial != value)
                {
                    selectedMaterial = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<LocalWork> works;
        public ObservableCollection<LocalWork> Works
        {
            get { return works; }
            set
            {
                if (works != value)
                {
                    works = value;
                    OnPropertyChanged();
                }
            }
        }

        private LocalWork selectedWork;
        public LocalWork SelectedWork
        {
            get { return selectedWork; }
            set
            {
                if (selectedWork != value)
                {
                    selectedWork = value;
                    Revenue = selectedWork != null && selectedWork.Cost != 0 ? selectedWork.Cost : (double?)null;
                    if (selectedWork != null)
                    {
                        WorkTypes = new ObservableCollection<LocalWorkType>(selectedWork.WorkTypes);
                        if (WorkTypes.Count > 0)
                        {
                            WorkTypeColWidth = ViewConstants.DimenstionStar;
                            WorkTypeVisibility = Visibility.Visible;
                        }
                        else
                        {
                            WorkTypeVisibility = Visibility.Collapsed;
                            WorkTypeColWidth = ViewConstants.DimenstionAuto;
                        }
                    }

                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<LocalWorkType> workTypes;
        public ObservableCollection<LocalWorkType> WorkTypes
        {
            get { return workTypes; }
            set
            {
                if (value == workTypes)
                    return;
                workTypes = value;
                OnPropertyChanged();
            }
        }

        private LocalWorkType selectedWorkType;
        public LocalWorkType SelectedWorkType
        {
            get { return selectedWorkType; }
            set
            {
                if (value == selectedWorkType)
                    return;
                selectedWorkType = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SettingsType> materialTypes;
        public ObservableCollection<SettingsType> MaterialTypes
        {
            get { return materialTypes; }
            set
            {
                if (value == materialTypes)
                    return;
                materialTypes = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SettingsItem> areas;
        public ObservableCollection<SettingsItem> Areas
        {
            get { return areas; }
            set
            {
                if (areas != value)
                {
                    areas = value;
                    OnPropertyChanged();
                }
            }
        }

        private SettingsItem selectedArea;
        public SettingsItem SelectedArea
        {
            get { return selectedArea; }
            set
            {
                if (selectedArea != value)
                {
                    selectedArea = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<Percent> percentages;
        public ObservableCollection<Percent> Percentages
        {
            get { return percentages; }
            set
            {
                if (value == percentages)
                    return;
                percentages = value;
                OnPropertyChanged();
            }
        }

        private Percent selectedPercentage;
        public Percent SelectedPercentage
        {
            get { return selectedPercentage; }
            set
            {
                if (value == selectedPercentage)
                    return;
                selectedPercentage = value;
                OnPropertyChanged();
            }
        }

        #endregion Lists

        #region Properties

        private readonly IEventAggregator eventAggregator;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TotalsIfo totalsIfoSelectedEl;
        public TotalsIfo TotalInfoSelectedEl
        {
            get { return totalsIfoSelectedEl; }
            set
            {
                if (value == totalsIfoSelectedEl)
                    return;
                totalsIfoSelectedEl = value;
                OnPropertyChanged();
            }
        }

        private Brush contentBackground = BackgroundColors.AddInterventionTileColor;

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

        private bool isPatientComboFocused;
        public bool IsPatientComboFocused
        {
            get { return isPatientComboFocused; }
            set
            {
                isPatientComboFocused = value;
                OnPropertyChanged();
            }
        }

        public DentalRecordsScreeViewModel Parent { get; set; }

        private InterventionDetails lastAddedInterventionDetails;
        public InterventionDetails LastaddedInteventiondetails
        {
            get { return lastAddedInterventionDetails; }
            set
            {
                if (value == lastAddedInterventionDetails)
                    return;
                lastAddedInterventionDetails = value;
                OnPropertyChanged();
            }
        }

        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (value == id)
                    return;
                id = value;
                OnPropertyChanged();
            }
        }

        private LocalIntervention localIntervention;
        public LocalIntervention LocalIntervention
        {
            get { return localIntervention; }
            set
            {
                if (value == localIntervention)
                    return;
                localIntervention = value;
                OnPropertyChanged();
            }
        }

        private InterventionDetails selectedInterventionDetails;
        public InterventionDetails SelectedInterventionDetails
        {
            get { return selectedInterventionDetails; }
            set
            {
                if (value == selectedInterventionDetails)
                    return;
                selectedInterventionDetails = value;
                LocalCache.Instance.IntervalIntervention.SetIntervention(selectedInterventionDetails);
                OnPropertyChanged();
            }
        }

        private bool isInEditMode;
        public bool IsInEditMode
        {
            get { return isInEditMode; }
            set
            {
                if (value == isInEditMode)
                    return;
                isInEditMode = value;
                if (!isInEditMode) Id = 0;
                OnPropertyChanged();
            }
        }

        private string observation;
        public string Observation
        {
            get { return observation; }
            set
            {
                if (observation != value)
                {
                    observation = value;
                    OnPropertyChanged();
                }
            }
        }

        private double? revenue;
        public double? Revenue
        {
            get { return revenue; }
            set
            {
                if (revenue != value)
                {
                    revenue = value;
                    OnPropertyChanged();
                }
            }
        }

        private double? materialCost;

        public double? MaterialCost
        {
            get { return materialCost; }
            set
            {
                if (value == materialCost)
                    return;
                materialCost = value;
                OnPropertyChanged();
            }
        }

        private string durata;

        public string Durata
        {
            get
            {
                try
                {
                    if (Convert.ToDouble(durata) == 0)
                        return "";
                    else
                    {
                        return durata;
                    }
                }
                catch (Exception)
                {
                    return "";
                }
            }
            set
            {
                try
                {
                    durata = Convert.ToString(value);
                }
                catch (Exception)
                {
                    MessageBox.Show("Inserati o valoare numerica pentru Durata");
                }
                OnPropertyChanged();
            }
        }

        private LocalPatient editedPatient;

        public LocalPatient EditedPatient
        {
            get { return editedPatient; }
            set
            {
                if (value == editedPatient)
                    return;
                editedPatient = value;
                OnPropertyChanged();
            }
        }

        private string materialTypeName = "Tip Material";
        public string MaterialTypeName
        {
            get { return materialTypeName; }
            set
            {
                if (value == materialTypeName)
                    return;
                materialTypeName = value;
                OnPropertyChanged();
            }
        }

        private SettingsType selectedMaterialType;

        public SettingsType SelectedMaterialType
        {
            get { return selectedMaterialType; }
            set
            {
                if (value == selectedMaterialType)
                    return;
                selectedMaterialType = value;
                OnPropertyChanged();
            }
        }

        private string materialTypeColWidth = ViewConstants.DimenstionAuto;
        public string MaterialTypeColWidth
        {
            get { return materialTypeColWidth; }
            set
            {
                if (value == materialTypeColWidth)
                    return;
                materialTypeColWidth = value;
                AuxColWidth = materialTypeColWidth == ViewConstants.DimenstionStar
                    ? ViewConstants.DimenstionAuto
                    : ViewConstants.DimenstionStar;
                OnPropertyChanged();
            }
        }

        private string workTypeColWidth = ViewConstants.DimenstionAuto;

        public string WorkTypeColWidth
        {
            get { return workTypeColWidth; }
            set
            {
                if (value == workTypeColWidth)
                    return;
                workTypeColWidth = value;
                AuxColWidthRow2 = workTypeColWidth == ViewConstants.DimenstionStar
                ? ViewConstants.DimenstionAuto
                : ViewConstants.DimenstionStar;
                OnPropertyChanged();
            }
        }

        private string auxColWidth = ViewConstants.DimenstionStar;

        public string AuxColWidth
        {
            get { return auxColWidth; }
            set
            {
                if (value == auxColWidth)
                    return;
                auxColWidth = value;
                OnPropertyChanged();
            }
        }

        private string auxColWidthRow2 = ViewConstants.DimenstionStar;

        public string AuxColWidthRow2
        {
            get { return auxColWidthRow2; }
            set
            {
                if (value == auxColWidthRow2)
                    return;
                auxColWidthRow2 = value;
                OnPropertyChanged();
            }
        }

        private bool wasPayedByDental;

        public bool WasPayedByDental
        {
            get { return wasPayedByDental; }
            set
            {
                if (value == wasPayedByDental)
                    return;
                wasPayedByDental = value;
                OnPropertyChanged();
            }
        }

        private Visibility materialTypeVisibility = Visibility.Collapsed;
        public Visibility MaterialTypeVisibility
        {
            get { return materialTypeVisibility; }
            set
            {
                if (value == materialTypeVisibility)
                    return;
                materialTypeVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility workTypeVisibility = Visibility.Collapsed;
        public Visibility WorkTypeVisibility
        {
            get { return workTypeVisibility; }
            set
            {
                if (value == workTypeVisibility)
                    return;
                workTypeVisibility = value;
                OnPropertyChanged();
            }
        }

        private DateTime date = DateTime.Now;
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (date != value)
                {
                    date = value;
                    StartingHour = value.Date;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime startTime;
        public DateTime StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime endtime;
        public DateTime EndTIme
        {
            get { return endtime; }
            set
            {
                if (endtime != value)
                {
                    endtime = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime startingHour = DateTime.Today;
        public DateTime StartingHour
        {
            get { return startingHour; }
            set
            {
                startingHour = value;
                OnPropertyChanged();
            }
        }

        private AddInterventionTileViewModel previouslyAddedIntervenstion;
        public AddInterventionTileViewModel PreviouslyAddedIntervenstion
        {
            get { return previouslyAddedIntervenstion; }
            set
            {
                if (previouslyAddedIntervenstion != value)
                {
                    previouslyAddedIntervenstion = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion Properties

        #region Buttons

        private ActionButtonViewModel saveIntevertionButton;
        public ActionButtonViewModel SaveIntevertionButton
        {
            get { return saveIntevertionButton; }
            set
            {
                if (saveIntevertionButton == value)
                    return;
                saveIntevertionButton = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel cancelIntevertionButton;
        public ActionButtonViewModel CancelIntevertionButton
        {
            get { return cancelIntevertionButton; }
            set
            {
                if (value == cancelIntevertionButton)
                    return;
                cancelIntevertionButton = value;
                OnPropertyChanged();
            }
        }

        #endregion Buttons

        #region Methods

        private void ChangeTotalInfo(LocalIntervention intervention, bool wasDeleted)
        {
            if (wasDeleted)
            {
                eventAggregator.GetEvent<TotalsModifiedEvent>().Publish(TotalInfoSelectedEl);
            }

            eventAggregator.GetEvent<TotalsModifiedEvent>().Publish(new TotalsIfo
            {
                TotalHours = intervention.DateHourDetail.Duration,
                TotalInverventions = 1,
                TotalRevenue = intervention.Revenue,
                TotalProfit = intervention.Percent

            });
        }

        private string GetNotAddedInformation(out bool shouldDisplayMessage)
        {
            String itemsNotSelected = " ";
            shouldDisplayMessage = false;
            if (SelectedPatient == null)
            {
                itemsNotSelected += "Pacient,";
                shouldDisplayMessage = true;
            }
            if (SelectedWork == null)
            {
                itemsNotSelected += "Manopera,";
                shouldDisplayMessage = true;
            }
            //            if (Revenue == 0)
            //            {
            //                itemsNotSelected += "Incasari,";
            //                shouldDisplayMessage = true;
            //            }
            if (StartingHour == null)
            {
                itemsNotSelected += "Ora inceput,";
                shouldDisplayMessage = true;
            }
            if (Durata == "")
            {
                itemsNotSelected += "Durata,";
                shouldDisplayMessage = true;
            }
            //            if (SelectedDuration == null)
            //            {
            //                itemsNotSelected += "Durata,";
            //                shouldDisplayMessage = true;
            //            }
            return itemsNotSelected.Substring(0, itemsNotSelected.Length - 1);
        }

        private void SetNewIntervenionDetails(LocalIntervention intervention)
        {
            var addedInterventionDetails = intervention.ToInterventionDetails();
            addedInterventionDetails.NewlyAdded = true;
            addedInterventionDetails.Brush = BackgroundColors.BackgroundFilled;
            if (IsInEditMode)
            {
                Parent.YearListViewModel.RemoveInterventionFromYear(SelectedInterventionDetails);
            }
            Parent.YearListViewModel.AddYear(addedInterventionDetails);
            if (LastaddedInteventiondetails != null)
                LastaddedInteventiondetails.Brush = BackgroundColors.GridInterventionRowColor;
            LastaddedInteventiondetails = addedInterventionDetails;
        }

        private void InitData()
        {
            SelectedArea = null;
            Revenue = null;
            SelectedWorkType = new LocalWorkType();
            SelectedWork = new LocalWork();
            SelectedMaterialType = new SettingsType();
            SelectedMaterial = new LocalMaterial();
            StartingHour = EndTIme;
            SelectedPatient = new LocalPatient();
            Observation = "";
            SelectedLocation = new SettingsItem();
            SelectedTechnician = new LocalTechnician();
            Durata = "";
            IsInEditMode = false;
            WasPayedByDental = false;
            SelectedPercentage = null;
            MaterialCost = null;
        }

        private void OnCancel()
        {
            InitData();

        }

        private void InitList()
        {
            Areas = new ObservableCollection<SettingsItem>(LocalCache.Instance.SettingsItems[LocalCache.SettingsItemEnum.Zona]);
            Works = new ObservableCollection<LocalWork>(LocalCache.Instance.Works);
            LocalLocations = new ObservableCollection<SettingsItem>(LocalCache.Instance.SettingsItems[LocalCache.SettingsItemEnum.Locatie]);
            Materials = new ObservableCollection<SettingsItem>(LocalCache.Instance.SettingsItems[LocalCache.SettingsItemEnum.Material]);
            Technicians = new ObservableCollection<LocalTechnician>(LocalCache.Instance.Technicians);

            PopulatePatients();
        }

        private void PopulatePatients()
        {
            PatientsList = new ObservableCollection<LocalPatient>(LocalCache.Instance.PatientsRepository.Patients);
            foreach (var localPatient in PatientsList)
            {
                localPatient.AllName = localPatient.Surname + (!string.IsNullOrEmpty(localPatient.Surname) ? " " : "") + localPatient.Name;
            }
            if (LocalCache.Instance.SelectedPatient != null)
                SelectedPatient = PatientsList.FirstOrDefault(item => item.Id == LocalCache.Instance.SelectedPatient.Id);
        }

        public void SetInterventionToEdit()
        {
            TotalInfoSelectedEl = new TotalsIfo()
            {
                TotalHours = -SelectedInterventionDetails.LocalIntervention.DateHourDetail.Duration,
                TotalInverventions = -1,
                TotalRevenue = -SelectedInterventionDetails.LocalIntervention.Revenue,
                TotalProfit = -SelectedInterventionDetails.LocalIntervention.Percent
            };
            SelectedArea = SelectedInterventionDetails.LocalIntervention.Area != null ? Areas.FirstOrDefault(item => item.Id == SelectedInterventionDetails.LocalIntervention.Area.Id) : null;
            if (SelectedInterventionDetails.LocalIntervention.Location != null)
                SelectedLocation = LocalLocations.FirstOrDefault(item => item.Id == SelectedInterventionDetails.LocalIntervention.Location.Id);
            if (SelectedInterventionDetails.LocalIntervention.Material != null && Materials != null)
            {
                SelectedMaterial = Materials.FirstOrDefault(item => item.Id == SelectedInterventionDetails.LocalIntervention.Material.Id);
            }
            SelectedPatient = PatientsList.FirstOrDefault(item => item.Id == SelectedInterventionDetails.LocalIntervention.PatientId);
            SelectedWork = SelectedInterventionDetails.LocalIntervention.Lucrare != null ? Works.FirstOrDefault(item => item.Id == SelectedInterventionDetails.LocalIntervention.Lucrare.Id) : new LocalWork();
            Durata = SelectedInterventionDetails.Durata.TotalMinutes.ToString();
            Date = SelectedInterventionDetails.LocalIntervention.DateHourDetail.Date;
            StartingHour = SelectedInterventionDetails.LocalIntervention.DateHourDetail.StartHour;
            Revenue = SelectedInterventionDetails.LocalIntervention.Revenue;
            Id = SelectedInterventionDetails.LocalIntervention.Id;
            LocalIntervention = SelectedInterventionDetails.LocalIntervention;
            Observation = SelectedInterventionDetails.Observations;
            IsInEditMode = true;
            WasPayedByDental = SelectedInterventionDetails.WasPayedByDental;
            SelectedTechnician =
                Technicians.FirstOrDefault(item => item.Id == SelectedInterventionDetails.LocalIntervention.TechnicianId);
            MaterialCost = SelectedInterventionDetails.MaterialCost;

        }

        private void InitPercentage()
        {
            var listOfP = new ObservableCollection<Percent>();
            for (int i = 0; i < 11; i++)
            {
                listOfP.Add(new Percent()
                {
                    Percentage = i * 10
                });
            }
            Percentages = listOfP;
        }

        public int count = 0;
        public async void AddNewPatient(string PatientName)
        {
            int spaceOcc = PatientName.IndexOf(" ");
            var patient = new LocalPatient()
            {
                Name = spaceOcc > 0 ? PatientName.Substring(spaceOcc + 1, PatientName.Length - spaceOcc - 1) : PatientName,
                Surname = spaceOcc > 0 ? PatientName.Substring(0, spaceOcc) : "",
                AllName = PatientName,
                Brush = BackgroundColors.BackgroundFilled,
            };
            if (!PatientsList.Any(item => item.Surname.Trim() == patient.Surname.Trim() && item.Name.Trim() == patient.Name.Trim()))
            {
                patient.Name = patient.Name.TrimEnd();
                patient.Surname = patient.Surname.TrimEnd();
                patient.AllName = patient.Surname + " " + patient.Name;
                SelectedPatient = patient;
                try
                {
                    SelectedPatient.Id = await DatabaseHandler.Instance.AddPatient(SelectedPatient, LocalCache.Instance.CurrentUser.Id);
                    PatientsList.Add(SelectedPatient);
                    eventAggregator.GetEvent<PatientAddedEvent>().Publish(SelectedPatient);
                    LocalCache.Instance.PatientsRepository.Patients.Add(SelectedPatient);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la salvare pacient");
                }

            }
        }

        public void Save()
        {
            if (SelectedPatient != null && SelectedPatient.NewlyAdded)
            {
                PatientsList.Add(SelectedPatient);
                SelectedPatient.NewlyAdded = false;
            }
            try
            {
                bool shouldDisplayMessage;
                double cost = MaterialCost.HasValue ? MaterialCost.Value : 0;
                var itemsNotSelected = GetNotAddedInformation(out shouldDisplayMessage);
                if (shouldDisplayMessage)
                {
                    MessageBox.Show("Inainte de a salva adaugati :" + itemsNotSelected);
                    return;
                }
                var duration = TimeSpan.FromMinutes(Convert.ToDouble(Durata));
                EndTIme = StartingHour.Add(duration);
                double percentage = SelectedPercentage != null
                    ? SelectedPercentage.Percentage / 100
                    : (SelectedWork != null && SelectedWork.Percent != 0 ? SelectedWork.Percent / 100 : 1);

                var intervention = new LocalIntervention()
                {
                    Id = Id,
                    Area = SelectedArea != null ? (SelectedArea.SettingTOarea()) : null,
                    Location = SelectedLocation != null ? SelectedLocation.SettingToLocation() : null,
                    DateHourDetail = new LocalDateHourDetail()
                    {
                        Date = Date,
                        StartHour = StartingHour,
                        Mili = Convert.ToInt64(DateTime.Now.TimeOfDay.TotalMilliseconds),
                        EndingHour = EndTIme,
                        Duration = duration
                    },
                    Lucrare = SelectedWork,
                    PatientName = SelectedPatient.AllName,
                    Material = SelectedMaterial,
                    Observation = Observation,
                    Revenue = Revenue.HasValue ? Revenue.Value : 0,
                    PatientId = SelectedPatient.Id,
                    Percent = Convert.ToDouble((Revenue - cost) * percentage),
                    WasPayedByDental = WasPayedByDental,
                    TechnicianId = SelectedTechnician != null ? SelectedTechnician.Id : (int?)null,
                    MaterialCost = MaterialCost,
                    Technician = SelectedTechnician
                };

                ChangeTotalInfo(intervention, IsInEditMode);
                intervention.Id = DatabaseHandler.Instance.AddIntervention(intervention, LocalCache.Instance.CurrentUser.Id);
                if (IsInEditMode)
                {
                    LocalIntervention = intervention;
                }
                else
                {
                    LocalCache.Instance.AddIntervention(intervention);
                    var patient = LocalCache.Instance.PatientsRepository.Patients.FirstOrDefault(item => item.Id == SelectedPatient.Id);
                    if (patient != null)
                        patient.Interventions.Add(intervention);
                }
                SetNewIntervenionDetails(intervention);
                IsPatientComboFocused = false;
                IsPatientComboFocused = true;
                InitData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("A intervenit o eroare la salvare.");
                Log.Error("Error inserting intervention --> Save");
            }
        }

        #endregion Methods
    }
}
