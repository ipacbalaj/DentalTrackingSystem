using ClientShell.Views.Tabs.ListView;
using ClientShell.Views.Tabs.ListView.Patient;
using DSA.Database.Model.Entities;

namespace ClientShell.Helpers
{
    public static class ExtensionMethods
    {
                public static PatientViewModel ToPatientViewModel(this LocalPatient localPatient,LeftListViewModel parent)
                {
                    return new PatientViewModel(parent)
                    {
                        Id = localPatient.Id,
                        Name = localPatient.Name,
                        Surname = localPatient.Surname,
                        Address = localPatient.Address,
                        City = localPatient.City,
                        Phone = localPatient.Phone,
                        Email = localPatient.Email
                    };
                }

        public static LocalPatient ToLocalPatient(this PatientViewModel patientViewModel)
        {
            return new LocalPatient()
            {
                Name = patientViewModel.Name,
                Surname = patientViewModel.Surname,
                Phone = patientViewModel.Phone,
                Address = patientViewModel.Address,
                Email = patientViewModel.Email,
                City = patientViewModel.City,
                Id = patientViewModel.Id
            };
        }

    }
}
