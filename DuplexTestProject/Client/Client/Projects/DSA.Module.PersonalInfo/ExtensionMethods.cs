using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Database.Model.Entities;
using DSA.Module.PersonalInfo.PatientInformation;
using DSA.Module.PersonalInfo.PersonalInfoScreen;

namespace DSA.Module.PersonalInfo
{
    public static class ExtensionMethods
    {
        public static PatientInformationViewModel ToPatientInformationViewModel(this LocalPatient localPatient, PersonalInfoScreenViewModel parent)
        {
            return new PatientInformationViewModel(parent)
            {
                Name = localPatient.Name,
                Surname = localPatient.Surname,
                BirthDate = localPatient.BirthDate,
                Email = localPatient.Email,
                Phone = localPatient.Phone,
                Street = localPatient.Street,
                Number = localPatient.StreetNumber,
                Bloc = localPatient.Block,
                Country = localPatient.Country,
                Job = localPatient.Job,
                Ocupation = localPatient.Ocupation,
                City = localPatient.City,                
            };
        }

        public static bool CopyToLocalPatient(this LocalPatient localPatient, PatientInformationViewModel patientInformationViewModel)
        {
            bool toreturn = false;
            if (localPatient.Name != patientInformationViewModel.Name ||
                localPatient.Surname != patientInformationViewModel.Surname)
                toreturn = true;
            localPatient.BirthDate = patientInformationViewModel.BirthDate;
            localPatient.Block = patientInformationViewModel.Bloc;
            localPatient.Ocupation = patientInformationViewModel.Ocupation;
            localPatient.Phone = patientInformationViewModel.Phone;
            localPatient.Street = patientInformationViewModel.Street;
            localPatient.Name = patientInformationViewModel.Name;
            localPatient.StreetNumber = patientInformationViewModel.Number;
            localPatient.Surname = patientInformationViewModel.Surname;
            localPatient.Email = patientInformationViewModel.Email;
            localPatient.Country = patientInformationViewModel.Country;
            localPatient.City = patientInformationViewModel.City;
            localPatient.Job = patientInformationViewModel.Job;
            localPatient.AllName = localPatient.Surname + (!string.IsNullOrEmpty(localPatient.Surname) ? " " : "") + localPatient.Name;
            return true;
        }
    }
}
