using System.Collections.Generic;
using System.Linq;
using DSA.Database.Model.Entities;
using DSA.Database.Model.Entities.SettingsEntities;
using DSA.Module.PersonalData.SettingsColumns.Works.WorkTile;
using DSA.Module.PersonalData.SettingsColumns.Works.WorkTile.WorkTypeTile;
using DSA.Module.PersonalData.UserDetails;

namespace DSA.Module.PersonalData
{
    public static class ExtensionMethods
    {
//        public static PatientViewModel ToPatientViewModel(this LocalPatient localPatient)
//        {
//            return new PatientViewModel()
//            {
//                Id = localPatient.Id,
//                Name = localPatient.Name,
//                Surname = localPatient.Surname,
//                Address = localPatient.Address,
//                City = localPatient.City,
//                Phone = localPatient.Phone,
//                Email = localPatient.Email
//            };
//        }

        public static UserDetailsViewModel ToDetailsViewModel(this LocalPatient patient)
        {
            return new UserDetailsViewModel()
            {
                Address = patient.Address,
                Name = patient.Name,
                Surname = patient.Surname,
                Phone = patient.Phone,               
            };
        }

        public static LocalWorkType ToLocalWorkType(this WorkTypeTileViewModel workTypeTile)
        {
            return new LocalWorkType()
            {
                Id = workTypeTile.Id,
                Cost = workTypeTile.Cost.HasValue?workTypeTile.Cost.Value:0,
                Name = workTypeTile.Name,
                Percent = workTypeTile.Percent
            };
        }

        public static LocalWork ToLocalWork(this WorkTileViewModel workTile)
        {
            var localWork = new LocalWork();
            localWork.Id = workTile.Id;
            localWork.Name = workTile.Name;
            localWork.Cost = workTile.Cost.HasValue?workTile.Cost.Value:0;
            localWork.Percent = workTile.Percent.HasValue?workTile.Percent.Value:0;
            localWork.IncludedINFinancial = workTile.IncludedINFinancial;
            localWork.WorkTypes=new List<LocalWorkType>();
            if(workTile.WorkTypeTileList!=null)
            foreach (var workTypeTileViewModel in workTile.WorkTypeTileList.WorkTypes)
            {
                localWork.WorkTypes.Add(workTypeTileViewModel.ToLocalWorkType());
            }
            return localWork;
        }

        public static List<LocalWork> TolLocalWorks(this List<WorkTileViewModel> worktiles)
        {
            return worktiles.Select(item => item.ToLocalWork()).ToList();
        }
    }
}
