using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Subscription.Service;
using DSA.Subscription.Service.DuplexService;

namespace DSA.Subscription.Service.Repositories
{
    public class PatientsRepository
    {
        #region Constructor

        public PatientsRepository()
        {
            PatientsDictionary=new Dictionary<int, LocalPatient>();
        }
        #endregion Constructor

        #region Properties
        public List<LocalPatient> Patients { get; set; }
        public Dictionary<int, LocalPatient> PatientsDictionary { get; set; } 

        #endregion Properties

        #region Methods

        public void SetPatient(List<LocalPatient> localPatients )
        {
            Patients = localPatients;
        }

        public void AddPatients(List<LocalPatient> localPatients)
        {
            new Task(delegate
            {
                foreach (var localPatient in localPatients)
                {
                    PatientsDictionary.Add(localPatient.Id, localPatient);
                }
            }).Start();
        }

        public void AddPatient(LocalPatient localPatient)
        {
            PatientsDictionary.Add(localPatient.Id,localPatient);
        }

        public void ClearPatients()
        {
            Patients.Clear();
        }

        #endregion Methods

    }
}
