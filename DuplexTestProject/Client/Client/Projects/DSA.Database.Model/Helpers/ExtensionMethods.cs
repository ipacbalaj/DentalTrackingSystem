using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Common.Infrastructure.Styles;
using DSA.Database.Model.EfSQlLite;
using DSA.Database.Model.Entities;
using DSA.Database.Model.Entities.Filters;
using DSA.Database.Model.Entities.SettingsEntities;
using log4net;
using DateHourDetail = DSA.Database.Model.EfSQlLite.DateHourDetail;
using Type = DSA.Database.Model.EfSQlLite.Type;

namespace DSA.Database.Model.Helpers
{
    public static class PatientsExtensionMethods
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static LocalPatient ToLocalPatient(this Patient entityPatient)
        {
            var patient = new LocalPatient()
            {
                Id = entityPatient.id,
                Name = entityPatient.name,
                Surname = entityPatient.surname,
                Address = entityPatient.Address,
                City = entityPatient.City,
                Email = entityPatient.Email,
                Phone = entityPatient.Phone,
                BirthDate = entityPatient.BirthDate.HasValue ? entityPatient.BirthDate.Value : new DateTime(1900, 1, 1),
                Block = entityPatient.Block,
                Job = entityPatient.Job,
                StreetNumber = entityPatient.StreetNumber,
                Street = entityPatient.Street,
                Ocupation = entityPatient.Ocupation,
                Country = entityPatient.Country,
                Brush = BackgroundColors.JurnalColor,
                AllName = entityPatient.surname + (!string.IsNullOrEmpty(entityPatient.surname) ? " " : "") + entityPatient.name
            };
            patient.Interventions = new List<LocalIntervention>();
            foreach (var intervention in entityPatient.Interventions)
            {
                patient.Interventions.Add(intervention.ToLocalIntervention());
            }
            return patient;
        }
        public static Patient ToEntityPatient(this LocalPatient localPatient,int userId)
        {
            return new Patient()
            {
                id = localPatient.Id,
                name = localPatient.Name,
                surname = localPatient.Surname,
                Address = localPatient.Address,
                City = localPatient.City,
                Email = localPatient.Email,
                Phone = localPatient.Phone,
                UserId = userId
            };
        }
        public static List<LocalPatient> ExcelToPatient(this Dictionary<String, List<string>> excelResult)
        {
            List<LocalPatient> patients = new List<LocalPatient>();
            excelResult.Remove(excelResult.Keys.Last());
            foreach (var key in excelResult.Keys)
            {
                LocalPatient patient = new LocalPatient();
                patient.Name = excelResult[key][3];
                patient.Surname = "";
                LocalIntervention Intervention = new LocalIntervention();

                Intervention.Area = new LocalArea()
                {
                    Name = excelResult[key][7]
                };
                try
                {
                    Intervention.DateHourDetail = new LocalDateHourDetail()
                    {
                        StartHour = DateTime.FromOADate(float.Parse(excelResult[key][9])),
                        EndingHour = DateTime.FromOADate(float.Parse(excelResult[key][10])),
                        Date = Convert.ToDateTime(excelResult[key][2])
                    };
                }
                catch (Exception e)
                {
                    Log.Error(string.Format("Patient Name: {0}.Start Date Or Enddate Wrong", patient.Name), e);
                }
                Intervention.Location = new LocalLocation()
                {
                    Name = excelResult[key][4]
                };
                Intervention.Material = new LocalMaterial()
                {
                    Name = excelResult[key][6]
                };
                Intervention.Observation = excelResult[key][8];
                try
                {
                    Intervention.Revenue = Convert.ToDouble(excelResult[key][12]);
                }
                catch (Exception e)
                {
                    Log.Error(string.Format("Patient Name: {0} .Revenue Wrong", patient.Name), e);
                }
                Intervention.Lucrare = new LocalWork()
                {
                    Name = excelResult[key][5]
                };
                var t = excelResult[key][14];
                if (!string.IsNullOrEmpty(t))
                    try
                    {
                        Intervention.Percent = Convert.ToDouble(excelResult[key][14]);
                    }
                    catch (Exception e)
                    {
                        Log.Error(string.Format("Patient Name: {0} .Percent Wrong", patient.Name), e);
                    }
                //patient.Intervention = Intervention;
                var currPatient = patients.FirstOrDefault(item => item.Name == patient.Name);
                if (currPatient != null)
                {
                    currPatient.Interventions.Add(Intervention);
                }
                else
                {
                    patient.Interventions = new List<LocalIntervention>();
                    patient.Interventions.Add(Intervention);
                    patients.Add(patient);
                }
            }

            return patients;
        }

        public static LocalDateHourDetail ToLocalDateHourDetail(this DateHourDetail dateHourDetails)
        {
            return new LocalDateHourDetail()
            {
                Id = dateHourDetails.Id,
                StartHour = dateHourDetails.StartHour,
                EndingHour = dateHourDetails.EndingHour,
                Duration = dateHourDetails.Duration.HasValue ? TimeSpan.FromTicks(dateHourDetails.Duration.Value) : new TimeSpan()
            };
        }

        public static LocalIntervention ToLocalIntervention(this Intervention intervention)
        {
            return new LocalIntervention()
            {
                Observation = intervention.Observation,
                Id = intervention.Id,
                Area = intervention.Area != null ? intervention.Area.ToLocalArea() : null,
                Location = intervention.Location != null ? intervention.Location.ToLocalLocation() : null,
                Lucrare = intervention.Work != null ? intervention.Work.TolLocalWork() : null,
                WorkType = intervention.WorkType != null ? intervention.WorkType.ToLocalWorkType() : null,
                Material = intervention.Material != null ? intervention.Material.ToLocalMaterial() : null,
                MaterialTYpe = intervention.Type != null ? intervention.Type.ToMaterialType() : null,
                DateHourDetail = intervention.DateHourDetail != null ? new LocalDateHourDetail()
                {
                    Id = intervention.DateHourDetail.Id,
                    StartHour = intervention.DateHourDetail.StartHour,
                    EndingHour = intervention.DateHourDetail.EndingHour,
                    Duration = intervention.DateHourDetail.Duration.HasValue ? TimeSpan.FromTicks(intervention.DateHourDetail.Duration.Value) : new TimeSpan(),
                    Date = intervention.DateHourDetail.Date,
                    Mili = intervention.DateHourDetail.MiliTime
                } : null,
                Revenue = intervention.Revenue,
                PatientName = intervention.Patient.surname + " " + intervention.Patient.name,
                Percent = intervention.Percent,
                WasPayedByDental = intervention.WasPayedByDental.HasValue ? intervention.WasPayedByDental.Value : false,
                PatientId = intervention.Patient.id,
                TechnicianId = intervention.TechnicianId,
                MaterialCost = intervention.MaterialCost,
                Technician = intervention.Technician != null ? intervention.Technician.ToLocalTechnician() : new LocalTechnician()
            };
        }

        public static InterventionDetails ToInterventionDetails(this LocalIntervention localIntervention)
        {
            int minutes = 0;
            int hours = 0;
            if (localIntervention.DateHourDetail != null)
            {
                hours = localIntervention.DateHourDetail.Duration.Hours;
                minutes = localIntervention.DateHourDetail.Duration.Minutes;
//                minutes = minutes + localIntervention.DateHourDetail.Duration.Seconds > 50 ? 1 : 0;
            }


            return new InterventionDetails()
            {
                Area = localIntervention.Area != null ? localIntervention.Area.Name : null,
                EndH = localIntervention.DateHourDetail.EndingHour,
                StartH = localIntervention.DateHourDetail.StartHour,
                Location = localIntervention.Location != null ? localIntervention.Location.Name : null,
                Observations = localIntervention.Observation,
                Material = localIntervention.Material != null ? localIntervention.Material.Name : null,
                MaterialType = localIntervention.MaterialTYpe != null ? localIntervention.MaterialTYpe.Name : null,
                Work = localIntervention.Lucrare != null ? localIntervention.Lucrare.Name : null,
                WorkType = localIntervention.WorkType != null ? localIntervention.WorkType.Name : null,
                DateString = localIntervention.DateHourDetail != null ? localIntervention.DateHourDetail.Date.ToShortDateString() : "",
                Date = localIntervention.DateHourDetail.Date,
                Mili = localIntervention.DateHourDetail.Mili,
                Revenue = localIntervention.Revenue,
                Durata = new TimeSpan(hours, minutes, 0),// localIntervention.DateHourDetail != null ? localIntervention.DateHourDetail.Duration : new TimeSpan(0, 0, 0, 0),
                //new TimeSpan(hours, minutes, 0),//
                WasPayedByDental = localIntervention.WasPayedByDental,
                //if i put PacientName after was payed i can test smth look in wasPyed setter
                PacientName = localIntervention.PatientName,
                Percent = localIntervention.Percent,
                Id = localIntervention.Id,
                LocalIntervention = localIntervention,
                Brush = localIntervention.WasPayedByDental ? BackgroundColors.GridInterventionRowColor : BackgroundColors.GridNotPayedColor,
//                Brush = localIntervention.DateHourDetail.StartHour > LocalCache.Instance.ProgramInfo.SetAsNewStartDate ? BackgroundColors.BackgroundPending : localIntervention.WasPayedByDental ? BackgroundColors.GridInterventionRowColor : BackgroundColors.GridNotPayedColor,
                MaterialCost = localIntervention.MaterialCost,
                TechnicianId = localIntervention.TechnicianId,
                TechnicianName = localIntervention.Technician != null ? localIntervention.Technician.Name : ""
            };
        }

        #region Filters

        public static LocalFilterItem ToLocalFilterItem(this FilterItem from)
        {
            return new LocalFilterItem
            {
                Id = from.Id,
                Name = from.Name
            };
        }

        public static LocalFilter ToLocalFilter(this Filter from)
        {
            var localFilter = new LocalFilter
            {
                Id = from.Id,
                IsActive = from.IsActive,
                Type = from.FilterType.Name
            };
            localFilter.FilterItems = new List<LocalFilterItem>();
            foreach (var filterItem in from.FilterItems)
            {
                localFilter.FilterItems.Add(filterItem.ToLocalFilterItem());
            }
            return localFilter;
        }

        public static LocalFilterGroup ToLocalFilterGroup(this FilterGroup from)
        {
            var localFilterGroup = new LocalFilterGroup
            {
                Id = from.Id,
                Name = from.Name,
                IsActive = from.IsActive,
                IsIntervalActive = from.IsIntervalActive,
                StartDate = from.StartDate,
                EndDate = from.EndDate,
            };
            localFilterGroup.Filters = new List<LocalFilter>();
            foreach (var filter in from.Filters)
            {
                localFilterGroup.Filters.Add(filter.ToLocalFilter());
            }
            return localFilterGroup;
        }


        #endregion Filters

        #region settings

        public static Work ToWork(this SettingsItem from)
        {
            return new Work
            {
                Name = from.Name,
                Cost = from.Cost,
                Percent = from.Percent,
                IncludedINFinancial = from.IncludedINFinancial
            };
        }

        public static Location ToLocation(this SettingsItem from)
        {
            return new Location
            {
                Name = from.Name,
            };
        }

        public static Material ToMaterial(this SettingsItem from)
        {
            return new Material
            {
                Name = from.Name,
                Cost = from.Cost,

            };
        }

        public static Area ToArea(this SettingsItem from)
        {
            return new Area
            {
                Name = from.Name
            };
        }

        public static LocalArea ToLocalArea(this Area area)
        {
            return new LocalArea()
            {
                Id = area.Id,
                Name = area.Name
            };
        }

        public static LocalWork TolLocalWork(this Work work)
        {
            return new LocalWork()
            {
                Id = work.Id,
                Name = work.Name,
                Cost = work.Cost,
                Percent = work.Percent.HasValue ? work.Percent.Value : 0,
                IncludedINFinancial = work.IncludedINFinancial.HasValue ? work.IncludedINFinancial.Value : false
            };
        }

        public static LocalLocation ToLocalLocation(this Location location)
        {
            return new LocalLocation()
            {
                Id = location.Id,
                Name = location.Name,
            };
        }

        public static LocalMaterial ToLocalMaterialSettingItem(this Material material)
        {
            return new LocalMaterial()
            {
                Id = material.Id,
                Name = material.Name
            };
        }

        public static LocalMaterial ToLocalMaterial(this Material material)
        {
            var locMaterial = new LocalMaterial()
            {
                Id = material.Id,
                Name = material.Name,
                Cost = material.Cost,
            };
            locMaterial.MaterialTypes = new List<SettingsType>();
            foreach (var type in material.Types)
            {
                locMaterial.MaterialTypes.Add(new SettingsType()
                {
                    Id = type.Id,
                    Name = type.Name,
                    Cost = type.Cost
                });
            }
            return locMaterial;
        }

        public static LocalArea SettingTOarea(this SettingsItem from)
        {
            return new LocalArea
            {
                Id = from.Id,
                Name = from.Name,
                Cost = from.Cost,
                Percent = from.Percent
            };
        }

        public static LocalWork SettingToWork(this LocalWork from)
        {
            return new LocalWork
            {
                Id = from.Id,
                Name = from.Name,
                Cost = from.Cost,
                Percent = from.Percent
            };
        }

        public static LocalLocation SettingToLocation(this SettingsItem from)
        {
            return new LocalLocation
            {
                Id = from.Id,
                Name = from.Name,
                Cost = from.Cost,
                Percent = from.Percent
            };
        }

        public static LocalMaterial SettingToMaterial(this SettingsItem from)
        {
            return new LocalMaterial
            {
                Id = from.Id,
                Name = from.Name,
                Cost = from.Cost,
                IncludedINFinancial = from.IncludedINFinancial,
                Percent = from.Percent,
                TechnicianIDd = from.TechnicianId
            };
        }

        public static LocalTechnician ToLocalTechnician(this Technician from)
        {
            var technician = new LocalTechnician
            {
                Id = from.Id,
                Name = from.Name
            };
            return technician;
        }

        public static Technician ToTechnician(this LocalTechnician from)
        {
            return new Technician
            {
                Id = from.Id,
                Name = from.Name
            };
        }

        public static SettingsType ToMaterialType(this Type materialType)
        {
            return new SettingsType()
            {
                Id = materialType.Id,
                Cost = materialType.Cost,
                Name = materialType.Name
            };
        }

        public static LocalWorkType ToLocalWorkType(this WorkType workType)
        {
            return new LocalWorkType()
            {
                Id = workType.Id,
                Cost = workType.Cost,
                Name = workType.Name,
                Percent = workType.Percent
            };
        }

        #endregion settings

        #region dentalPayment

        public static LocalDentalPaymentInfo ToLocalDentalPaymentInfo(this DentalPaymentInfo from)
        {
            return new LocalDentalPaymentInfo
            {
                Id = from.Id,
                Name = from.Name,
                Date = from.Date,
                TotalRevenue = from.TotalRevenue,
                TotalPercent = from.TotalPercent
            };
        }

        public static DentalPaymentInfo ToDentalPaymentInfo(this LocalDentalPaymentInfo from)
        {
            return new DentalPaymentInfo
            {
                Id = from.Id,
                Name = from.Name,
                Date = from.Date,
                TotalRevenue = from.TotalRevenue,
                TotalPercent = from.TotalPercent
            };
        }

        #endregion dentalPayment

        #region Works

        public static LocalWork ToLocalWork(this Work work)
        {
            var localWork = new LocalWork()
            {
                Id = work.Id,
                Name = work.Name,
                Cost = work.Cost,
                Percent = work.Percent.HasValue ? work.Percent.Value : 0,
                IncludedINFinancial = work.IncludedINFinancial.HasValue && work.IncludedINFinancial.Value,
            };
            localWork.WorkTypes = new List<LocalWorkType>();
            foreach (var workType in work.WorkTypes)
            {
                localWork.WorkTypes.Add(new LocalWorkType()
                {
                    Id = workType.Id,
                    Name = workType.Name,
                    Cost = workType.Cost,
                    Percent = workType.Percent,
                });
            }
            return localWork;
        }

        public static List<LocalWork> ToLocalWorks(this IEnumerable<Work> works)
        {
            return works.Select(item => item.ToLocalWork()).ToList();
        }

        #endregion Works

    }
}
