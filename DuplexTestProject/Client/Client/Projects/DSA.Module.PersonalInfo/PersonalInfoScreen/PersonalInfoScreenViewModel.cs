using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Xpf.Core.MvvmSample;
using DSA.Common.Controls.Buttons;
using DSA.Common.Controls.Buttons.OptionButton;
using DSA.Common.Controls.Loading.MetroLoading;
using DSA.Common.Infrastructure;
using DSA.Common.Infrastructure.Helpers;
using DSA.Common.Infrastructure.Icos;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.Entities;
using DSA.Database.Model.Helpers;
using DSA.Module.PersonalInfo.PatientInformation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using TULIP.Common.Controls.Icons;
using Brush = System.Windows.Media.Brush;

namespace DSA.Module.PersonalInfo.PersonalInfoScreen
{
    public class PersonalInfoScreenViewModel : ViewModelBase
    {

        #region Constructor

        public PersonalInfoScreenViewModel()
        {
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            unityContainer = ServiceLocator.Current.GetInstance<IUnityContainer>();
            eventAggregator.GetEvent<UpdatePersonalInfoEvent>().Subscribe(UpdatePersonalInfoScreen);
            eventAggregator.GetEvent<PatientSelectedEvent>().Subscribe(OnPatientSelected);
            eventAggregator.GetEvent<PatientDoubleClickedEvent>().Subscribe(OnPatientSelected);
            SaveBtn = new ActionButtonViewModel("Salvează  ", new DelegateCommand(OnSave), ImagePath.SaveIconPath);
            EditButton = new ActionButtonViewModel("Editează  ", new DelegateCommand(OnEdit), ImagePath.PatientEditPath);
            CancelButton = new ActionButtonViewModel("Renunță   ", new DelegateCommand(OnCancel), ImagePath.CancelIconPath);
            DeletePatientButton = new ActionButtonViewModel("Șterge Pacient", new DelegateCommand(OnDelete), ImagePath.CancelIconPath);
        }

        #endregion Constructor

        #region Properties

        private Task backgroundTask;
        private Task uiTask;

        public PersonalInfoScreenView ViewReference { get; set; }

        private bool initialized;

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

        private Brush contentBackground = BackgroundColors.PersonalInfoColor;

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

        private Visibility editVisibility = Visibility.Collapsed;

        public Visibility EditVisibility
        {
            get { return editVisibility; }
            set
            {
                if (value == editVisibility)
                    return;
                editVisibility = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel editButton;
        public ActionButtonViewModel EditButton
        {
            get { return editButton; }
            set
            {
                if (value == editButton)
                    return;
                editButton = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel cancelButton;

        public ActionButtonViewModel CancelButton
        {
            get { return cancelButton; }
            set
            {
                if (value == cancelButton)
                    return;
                cancelButton = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel saveBtn;

        public ActionButtonViewModel SaveBtn
        {
            get { return saveBtn; }
            set
            {
                if (value == saveBtn)
                    return;
                saveBtn = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel _deletePatientButton;

        public ActionButtonViewModel DeletePatientButton
        {
            get { return _deletePatientButton; }
            set
            {
                if (value == _deletePatientButton)
                    return;
                _deletePatientButton = value;
                OnPropertyChanged();
            }
        }

        private Visibility regularVisibility = Visibility.Visible;

        public Visibility RegularVisibility
        {
            get { return regularVisibility; }
            set
            {
                if (value == regularVisibility)
                    return;
                regularVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool isEdited;

        public bool IsEdit
        {
            get { return isEdited; }
            set
            {
                if (value == isEdited)
                    return;
                isEdited = value;
                if (isEdited)
                {
                    EditVisibility = Visibility.Visible;
                    RegularVisibility = Visibility.Collapsed;
                }
                else
                {
                    EditVisibility = Visibility.Collapsed;
                    RegularVisibility = Visibility.Visible;
                }
                OnPropertyChanged();
            }
        }

        private readonly IEventAggregator eventAggregator;
        private readonly IUnityContainer unityContainer;

        private PatientInformationViewModel patientInformationViewModel;
        public PatientInformationViewModel PatientInformationViewModel
        {
            get { return patientInformationViewModel; }
            set
            {
                if (value == patientInformationViewModel)
                    return;
                patientInformationViewModel = value;
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

        private ActionButtonViewModel _editSaveButton;
        public ActionButtonViewModel EditSaveButton
        {
            get { return _editSaveButton; }
            set
            {
                if (_editSaveButton == value)
                    return;
                _editSaveButton = value;
                OnPropertyChanged();
            }
        }

        private int patientId;
        public int PatientId
        {
            get { return patientId; }
            set
            {
                if (value == patientId)
                    return;
                patientId = value;
                OnPropertyChanged();
            }
        }

        private string expanderRotation = "90";
        public string ExpanderRotation
        {
            get { return expanderRotation; }
            set
            {
                if (expanderRotation != value)
                {
                    expanderRotation = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility interventionsVisibility = Visibility.Visible;
        public Visibility InterventionsVisibility
        {
            get { return interventionsVisibility; }
            set
            {
                if (value == interventionsVisibility)
                    return;
                interventionsVisibility = value;
                OnPropertyChanged();
            }
        }

        private Brush interventionTabColor = BackgroundColors.PatientsListColor;
        public Brush InterventionTabColor
        {
            get { return interventionTabColor; }
            set
            {
                if (interventionTabColor != value)
                {
                    interventionTabColor = value;
                    OnPropertyChanged();
                }
            }
        }

        private UIElement editIcon;

        public UIElement EditICon
        {
            get { return editIcon; }
            set
            {
                if (value == editIcon)
                    return;
                editIcon = value;
                OnPropertyChanged();
            }
        }

        private UIElement saveIcon;

        public UIElement SaveIcon
        {
            get { return saveIcon; }
            set
            {
                if (value == saveIcon)
                    return;
                saveIcon = value;
                OnPropertyChanged();
            }
        }

        private LocalPatient currentPatine;

        public LocalPatient CurrentPatinent
        {
            get { return currentPatine; }
            set
            {
                if (value == currentPatine)
                    return;
                currentPatine = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        private void UpdatePersonalInfoScreen(Object anObj)
        {
            if (!initialized)
            {
                if (PatientId == 0)
                {
                    MessageBox.Show("Nici un pacient Selectat");
                    return;
                }
                InterventionsVisibility = Visibility.Visible;
                initialized = true;
            }
        }

        private List<InterventionDetails> taskLoadedInterventions;

        private void OnPatientSelected(Object patient)
        {

            CurrentPatinent = (LocalPatient)patient;//LocalCache.Instance.PatientsRepository.Patients.FirstOrDefault(item => item.Id == patientId);            
            PatientId = CurrentPatinent.Id;
            PatientInformationViewModel = CurrentPatinent.ToPatientInformationViewModel(this);
            InterventionsVisibility = Visibility.Visible;
            MetroLoadingViewModel = new MetroLoadingViewModel(true);
            //MetroLoadingViewModel.MetroLoadingViewReference = new MetroLoadingView();
            //   get the Ui thread context
            TaskScheduler _uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Action DoInBackground = new Action(() =>
            {
                GetInterventions();
            });

            Action DoOnUiThread = new Action(() =>
            {
                UpdateInterventionsCollection();
            });

            // start the background task
            backgroundTask = Task.Factory.StartNew(() => DoInBackground());
            // when t1 is done run t1..on the Ui thread.
            uiTask = backgroundTask.ContinueWith(t => DoOnUiThread(), _uiScheduler);

            //            Interventions = new ObservableCollection<InterventionDetails>( DatabaseHandler.Instance.GetPatientInterventions(PatientId).Select(item => item.ToInterventionDetails()));
        }


        private void GetInterventions()
        {
            taskLoadedInterventions = DatabaseHandler.Instance.GetPatientInterventions(PatientId).Select(item => item.ToInterventionDetails()).ToList();
        }

        private void UpdateInterventionsCollection()
        {
            Interventions = new ObservableCollection<InterventionDetails>(taskLoadedInterventions);
            //            var firstTask = taskLoadedInterventions.FirstOrDefault();
            //            if (firstTask != null && firstTask.PacientName != CurrentPatinent.AllName)
            //            {
            //                OnPatientSelected(CurrentPatinent);
            //            }
            ViewReference.BestFitColums();
            MetroLoadingViewModel.ShouldContinueAnimation = false;
        }

        public void OnShowInterventions()
        {
            if (PatientId != 0)
            {
                //                Interventions = new ObservableCollection<InterventionDetails>( DatabaseHandler.Instance.GetPatientInterventions(PatientId).Select(item => item.ToInterventionDetails()));
                InterventionsVisibility = InterventionsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                ExpanderRotation = ExpanderRotation == "270" ? "90" : "270";
            }
            else
            {
                MessageBox.Show("Nicu un pacient Selectat");
            }
        }

        public void MouseOn()
        {
            InterventionTabColor = BackgroundColors.PatientsListColorOver;
        }

        public void MouseOff()
        {

            InterventionTabColor = BackgroundColors.PatientsListColor;
        }

        private void OnEdit()
        {
            if (!IsEdit)
            {
                IsEdit = true;

            }
            else
            {

                IsEdit = false;
            }

        }

        private void OnCancel()
        {
            RegularVisibility = Visibility.Visible;
            EditVisibility = Visibility.Collapsed;
            if (CurrentPatinent != null)
                PatientInformationViewModel = CurrentPatinent.ToPatientInformationViewModel(this);
            IsEdit = false;
        }

        private void OnSave()
        {
            RegularVisibility = Visibility.Visible;
            EditVisibility = Visibility.Collapsed;
            var editedPatient = LocalCache.Instance.PatientsRepository.Patients.FirstOrDefault(item => item.Id == CurrentPatinent.Id);
            if (editedPatient.CopyToLocalPatient(PatientInformationViewModel))
            {
                eventAggregator.GetEvent<PatientNameChangedEvent>().Publish(editedPatient);
            }
            DatabaseHandler.Instance.EditPatient(editedPatient);
            IsEdit = false;
        }

        private async void OnDelete()
        {
            try
            {
                await DatabaseHandler.Instance.DeletePatient(CurrentPatinent.Id);
                eventAggregator.GetEvent<PatientDeletedEvent>().Publish(CurrentPatinent.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pacientul nu a fost șters!Selectați pacientul si reîncercați sa il ștergeți.");
            }
        }

        #endregion Methods
    }
}
