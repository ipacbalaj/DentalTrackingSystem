using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Database.Model.Entities;

namespace DSA.Database.Model.Repositories
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
        public List<LocalPatient> BirthDays { get; set; } 

        #endregion Properties

        #region Methods

        public void SetPatient(List<LocalPatient> localPatients )
        {
            Patients = localPatients;
            SetBirthDays();
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

       public void SetBirthDays()
       {
           BirthDays = new List<LocalPatient>();
           foreach (var localPatient in Patients)
           {
               var date = DateTime.Now;
               if (date.Month == localPatient.BirthDate.Month && date.Day == localPatient.BirthDate.Day)
               {
                   BirthDays.Add(localPatient);
               }
           }
       }

        #endregion Methods

    }
    
}
