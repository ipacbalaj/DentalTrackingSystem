using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Module.PersonalData;
using DSA.Module.PersonalData.Patient;
using DSA.Subscription.Service;

namespace DSA.Client.Shell.Views.Tabs.ListView
{
    public class LeftListViewModel:ViewModelBase
    {
        #region Properties

        private List<PatientViewModel> patientsList;
        public List<PatientViewModel> PatientsList
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

        #endregion Properties

        #region Methods

        public void PopulatePatientsList(List<LocalPatient> localPatients )
        {
            PatientsList=new List<PatientViewModel>(localPatients.Select(patient=>patient.ToPatientViewModel()));
        }

        public void AddPatient()
        {
            
        }

        #endregion Methods

    }
}
