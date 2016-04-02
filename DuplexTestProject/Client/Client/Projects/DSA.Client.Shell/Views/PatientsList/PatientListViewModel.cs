using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ClientShell.Views.PatientsList.PatientListItem;
using DSA.Common.Infrastructure.ObjectList;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.Entities;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;

namespace ClientShell.Views.PatientsList
{
    public class PatientListViewModel:ViewModelBase
    {
        #region Constructor

        public PatientListViewModel()
        {
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<PatientNameChangedEvent>().Subscribe(OnPatientNameChanged);
        }

        #endregion Constructor          

        #region Properties

        private readonly IEventAggregator eventAggregator;

        private List<PatientListItemViewModel> patientsList; 

        private ObjectList<PatientListItemViewModel> patientItemList;
        public ObjectList<PatientListItemViewModel> PatientItemList
        {
            get { return patientItemList; }
            set
            {
                if (patientItemList != value)
                {
                    patientItemList = value;
                    OnPropertyChanged();
                }
            }
        }

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

        #endregion Properties

        #region Methods

        public void PopulatePatientsList(List<LocalPatient> localPatients)
        {
            patientsList=new List<PatientListItemViewModel>();
            PatientItemList=new ObjectList<PatientListItemViewModel>(false);
            foreach (var localPatient in localPatients)
            {
                var patientTile = new PatientListItemViewModel(this)
                {
                    Id = localPatient.Id,
                    PatientName = localPatient.EntireName
                };
                PatientItemList.Add(patientTile);
                patientsList.Add(patientTile);
            }
        }

        private void Search(string searchedWord)
        {
            searchedWord = searchedWord.ToLower();
            int wordLength = searchedWord.Length;
            PatientItemList = new ObjectList<PatientListItemViewModel>(false);
            var patientsToDisplay = patientsList.Where(item => item.PatientName.Length >= wordLength && item.PatientName.ToLower().Substring(0, wordLength) == searchedWord);//item => item.PatientName.ToLower().Contains(searchedWord));
            foreach (var patientListItemViewModel in patientsToDisplay)
            {
                PatientItemList.Add(patientListItemViewModel);
            }
            var toSelect = PatientItemList.List.FirstOrDefault();
            if(toSelect!=null)
            toSelect.OnSelected(toSelect);
            // CurrentPatients = new ObservableCollection<LocalPatient>(PatientsList.Where(item => item.Name.Length >= wordLength && item.Name.ToLower().Substring(0, wordLength) == searchedWord));
            // SelectedPatient = CurrentPatients.FirstOrDefault();
        }

        public void SetSelectedPatient(int patientId)
        {
            eventAggregator.GetEvent<PatientSelectedEvent>().Publish(patientId);   
        }

        private void OnPatientNameChanged(int patientID)
        {
            var chagedPatient = PatientItemList.List.FirstOrDefault(item => item.Id == patientID);
            var localPatienet = LocalCache.Instance.PatientsRepository.Patients.FirstOrDefault(item => item.Id == patientID);
            chagedPatient.PatientName =  localPatienet.EntireName;
        }

        #endregion Methods 

    }
}
