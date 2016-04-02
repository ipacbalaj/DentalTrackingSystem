using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DSA.Common.Infrastructure.Helpers;
using DSA.Common.Infrastructure.Login;
using DSA.Common.Infrastructure.Styles;
using DSA.Database.Model.EfSQlLite;
using DSA.Database.Model.Entities;
using DSA.Database.Model.Entities.Filters;
using DSA.Database.Model.Entities.SettingsEntities;
using DSA.Database.Model.Entities.StructureEntities;
using DSA.Database.Model.Helpers;
using log4net;
using DateHourDetail = DSA.Database.Model.EfSQlLite.DateHourDetail;
using Type = DSA.Database.Model.EfSQlLite.Type;

namespace DSA.Database.Model
{
    public class DatabaseHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private static readonly DatabaseHandler instance = new DatabaseHandler();
        public static DatabaseHandler Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion Fields

        #region Patients

        public async Task<List<LocalPatient>> GetPatients()
        {
            List<LocalPatient> localPatients = new List<LocalPatient>();
            using (dsaEntities dsaModel = new dsaEntities())
            {
                foreach (var patient in dsaModel.Patients)
                {
                    localPatients.Add(patient.ToLocalPatient());
                }
            }
            return localPatients;
        }

        public async Task<int> AddPatient(LocalPatient localPatient)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var patient = localPatient.ToEntityPatient();
                dsaModel.Patients.AddObject(patient);
                dsaModel.SaveChanges();
                return patient.id;
            }
        }

        public void EditPatient(LocalPatient localPatient)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var editedPatient = dsaModel.Patients.FirstOrDefault(item => item.id == localPatient.Id);
                editedPatient.BirthDate = localPatient.BirthDate > new DateTime(1900, 1, 1) ? localPatient.BirthDate : new DateTime(1900, 1, 1);
                editedPatient.Block = localPatient.Block;
                editedPatient.Ocupation = localPatient.Ocupation;
                editedPatient.Phone = localPatient.Phone;
                editedPatient.Street = localPatient.Street;
                editedPatient.name = localPatient.Name;
                editedPatient.StreetNumber = localPatient.StreetNumber;
                editedPatient.surname = localPatient.Surname;
                editedPatient.Email = localPatient.Email;
                editedPatient.City = localPatient.City;
                editedPatient.Job = localPatient.Job;
                editedPatient.Country = localPatient.Country;
                dsaModel.SaveChanges();
            }
        }

        public List<LocalIntervention> GetPatientInterventions(int patientId)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var patient = dsaModel.Patients.FirstOrDefault(item => item.id == patientId);
                var interventions = new List<LocalIntervention>();
                foreach (var intervention in patient.Interventions)
                {
                    interventions.Add(intervention.ToLocalIntervention());
                }
                return interventions;
            }
        }

        public async Task DeletePatient(int patientId)
        {
            using (dsaEntities dsaEntities = new dsaEntities())
            {
                var patientToDelete = dsaEntities.Patients.FirstOrDefault(item => item.id == patientId);
                var ids = patientToDelete.Interventions.Select(item => item.Id).ToList();
                foreach (var id in ids)
                {
                    var interventionToDelete = dsaEntities.Interventions.FirstOrDefault(item => item.Id == id);
                    dsaEntities.Interventions.DeleteObject(interventionToDelete);
                }

                patientToDelete.Interventions = null;
                dsaEntities.Patients.DeleteObject(patientToDelete);
                dsaEntities.SaveChanges();
            }
        }

        public async Task MergePatients(List<int> patientsIds)
        {
            using (dsaEntities dsaEntities = new dsaEntities())
            {
                var patientsToMerge = new List<Patient>();
                foreach (var patient in dsaEntities.Patients)
                {
                    if (patientsIds.Contains(patient.id))
                    {
                        patientsToMerge.Add(patient);
                    }
                }

                var patientThatRemains = patientsToMerge.FirstOrDefault(item => item.id == patientsIds.First());
                patientsToMerge.Remove(patientThatRemains);
                var inderventionsIds = new List<int>();
                foreach (var patient in patientsToMerge)
                {
                    inderventionsIds.AddRange(patient.Interventions.Select(item => item.Id).ToList());
                }

                foreach (var inderventionId in inderventionsIds)
                {
                    var interventionToMove = dsaEntities.Interventions.FirstOrDefault(item => item.Id == inderventionId);
                    interventionToMove.Patient = patientThatRemains;
                }
                dsaEntities.SaveChanges();
            }
        }

        #endregion Patients

        #region Excel

        //        public List<LocalTooth> GetTeeth()
        //        {
        //            List<LocalTooth> teeth=new List<LocalTooth>();
        //            using (dsaEntities dsaModel = new dsaEntities())
        //            {
        //                foreach (var tooth in dsaModel.Teeth)
        //                {
        //                    teeth.Add(tooth.ToLocalTooth());
        //                }
        //            }
        //            return teeth;
        //        }

        public void SaveWorks(List<string> workValues)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                foreach (var workValue in workValues)
                {
                    dsaModel.Works.AddObject(new Work()
                    {
                        Name = workValue
                    });
                }
                dsaModel.SaveChanges();
            }
        }

        public void SaveAreas(List<string> areasList)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                foreach (var area in areasList)
                {
                    dsaModel.Areas.AddObject(new Area()
                    {
                        Name = area
                    });
                }
                dsaModel.SaveChanges();
            }
        }

        public void SaveMaterials(List<string> materialsList)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                foreach (var material in materialsList)
                {
                    dsaModel.Materials.AddObject(new Material()
                    {
                        Name = material
                    });
                }
                dsaModel.SaveChanges();
            }
        }

        public void SavePatients(List<LocalPatient> patients)
        {
            try
            {
                using (dsaEntities dsaModel = new dsaEntities())
                {
                    for (int i = 1; i < 13; i++)
                    {
                        dsaModel.Months.AddObject(new Month()
                        {
                            MOnthNumber = i
                        });
                    }
                    dsaModel.SaveChanges();
                    foreach (var localPatient in patients)
                    {
                        bool shouldAddPatient = false;
                        var entityPactient = dsaModel.Patients.FirstOrDefault(item => (item.surname + " " + item.name) == (localPatient.Name));
                        if (entityPactient == null)
                        {
                            entityPactient = new Patient();
                            if (!string.IsNullOrEmpty(localPatient.Name))
                            {
                                int spaceOcc = localPatient.Name.IndexOf(" ");
                                entityPactient.name = spaceOcc > 0
                                    ? localPatient.Name.Substring(spaceOcc + 1, localPatient.Name.Length - spaceOcc - 1)
                                    : localPatient.Name;
                                entityPactient.surname = spaceOcc > 0 ? localPatient.Name.Substring(0, spaceOcc) : "";
                                shouldAddPatient = true;
                            }
                        }
                        foreach (var localIntervention in localPatient.Interventions)
                        {
                            var year =
                                dsaModel.Years.FirstOrDefault(
                                    item => item.YearNb == localIntervention.DateHourDetail.Date.Year);
                            if (year == null)
                            {
                                year = new Year()
                                {
                                    YearNb = localIntervention.DateHourDetail.Date.Year
                                };
                                dsaModel.Years.AddObject(year);
                                dsaModel.SaveChanges();
                            }
                            entityPactient.Interventions.Add(new Intervention()
                            {
                                Area = dsaModel.Areas.FirstOrDefault(item => item.Name == localIntervention.Area.Name),
                                Location = dsaModel.Locations.FirstOrDefault(item => item.Name == localIntervention.Location.Name),
                                Work = dsaModel.Works.FirstOrDefault(item => item.Name == localIntervention.Lucrare.Name),
                                Revenue = localIntervention.Revenue,
                                DateHourDetail = new DateHourDetail()
                                {
                                    Date = localIntervention.DateHourDetail.Date,
                                    StartHour = localIntervention.DateHourDetail.StartHour,
                                    EndingHour = localIntervention.DateHourDetail.EndingHour,
                                    Duration = (localIntervention.DateHourDetail.EndingHour - localIntervention.DateHourDetail.StartHour).Ticks
                                },
                                Year = year,
                                Month = dsaModel.Months.FirstOrDefault(item => item.MOnthNumber == localIntervention.DateHourDetail.Date.Month),
                                Material = dsaModel.Materials.FirstOrDefault(item => item.Name == localIntervention.Material.Name),
                                Observation = localIntervention.Observation,
                                Percent = localIntervention.Percent,
                                WasPayedByDental = true,

                            });
                        }
                        if (shouldAddPatient)
                        {
                            dsaModel.Patients.AddObject(entityPactient);
                        }
                    }
                    dsaModel.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Log.Error(string.Format("SavePatients method"), e);
            }


        }

        #endregion Excel

        #region InterventionDetails

        public async Task<List<LocalArea>> GetAreas()
        {
            List<LocalArea> areas = new List<LocalArea>();
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var availableAreas = dsaModel.Areas.Where(item => !item.WasDeleted.HasValue || (item.WasDeleted.HasValue && !item.WasDeleted.Value));
                foreach (var area in availableAreas)
                {
                    areas.Add(area.ToLocalArea());
                }
            }
            return areas.OrderBy(o => o.Name).ToList();
        }

        public async Task<List<LocalWork>> GetWorks()
        {
            List<LocalWork> works = new List<LocalWork>();
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var availableWorks = dsaModel.Works.Where(item => !item.WasDeleted.HasValue || (item.WasDeleted.HasValue && !item.WasDeleted.Value));
                foreach (var work in availableWorks)
                {
                    works.Add(work.TolLocalWork());
                }
            }
            return works.OrderBy(o => o.Name).ToList();
        }

        public async Task<List<LocalLocation>> GetlLocations()
        {
            List<LocalLocation> locations = new List<LocalLocation>();
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var availableLocations = dsaModel.Locations.Where(item => !item.WasDeleted.HasValue || (item.WasDeleted.HasValue && !item.WasDeleted.Value));
                foreach (var loc in availableLocations)
                {
                    locations.Add(loc.ToLocalLocation());
                }
            }
            return locations.OrderBy(o => o.Name).ToList();
        }

        public async Task<List<LocalMaterial>> GetMaterials()
        {
            List<LocalMaterial> materials = new List<LocalMaterial>();
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var availableMaterials = dsaModel.Materials.Where(item => !item.WasDeleted.HasValue || (item.WasDeleted.HasValue && !item.WasDeleted.Value));
                foreach (var loc in availableMaterials)
                {
                    materials.Add(loc.ToLocalMaterialSettingItem());
                }
            }
            return materials.OrderBy(o => o.Name).ToList();
        }

        public async Task<List<LocalTechnician>> GetTechnicians()
        {
            List<LocalTechnician> technicians = new List<LocalTechnician>();
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var availableTechinicians = dsaModel.Technicians.Where(item => !item.WasDeleted.HasValue || (item.WasDeleted.HasValue && !item.WasDeleted.Value));
                foreach (var technician in availableTechinicians)
                {
                    technicians.Add(technician.ToLocalTechnician());
                }
            }
            return technicians.OrderBy(o => o.Name).ToList();
        }

        #endregion InterventionDetails

        #region Year

        public List<LocalYear> GetLocalYears()
        {
            List<LocalYear> localYears = new List<LocalYear>();
            int currentYear = DateTime.Now.Year;
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var yearsEntity = dsaModel.Years;
                if (yearsEntity != null)
                {
                    foreach (var year in yearsEntity)
                    {
                        localYears.Add(new LocalYear()
                        {
                            Id = year.Id,
                            YearNb = year.YearNb,
                            Interventions = year.Interventions.Select(item => item.ToLocalIntervention()).ToList()
                        });
                    }

                    return localYears;
                }
                return new List<LocalYear>()
                {
                    new LocalYear()
                {
                    YearNb = currentYear
                }
                };
            }
        }

        public async Task<LocalYear> GetLocalYear(int yearNumber)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var entityYear = dsaModel.Years.FirstOrDefault(item => item.YearNb == yearNumber);
                if (entityYear != null)
                {
                    LocalYear year = new LocalYear()
                    {

                        Id = entityYear.Id,
                        YearNb = entityYear.YearNb,
                        Interventions = entityYear.Interventions.Select(item => item.ToLocalIntervention()).ToList()
                    };
                    return year;
                }
                else
                {
                    return new LocalYear();
                }

            }

        }
        #endregion Year

        #region Intervention

        public int AddIntervention(LocalIntervention localIntervention)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var year =
                         dsaModel.Years.FirstOrDefault(
                             item => item.YearNb == localIntervention.DateHourDetail.Date.Year);
                if (year == null)
                {
                    year = new Year()
                    {
                        YearNb = localIntervention.DateHourDetail.Date.Year
                    };
                    dsaModel.Years.AddObject(year);
                    dsaModel.SaveChanges();
                }
                var patient = dsaModel.Patients.FirstOrDefault(item => item.id == localIntervention.PatientId);
                var intervention = dsaModel.Interventions.FirstOrDefault(item => item.Id == localIntervention.Id);
                if (intervention != null)
                {
                    EditIntervention(localIntervention, intervention, dsaModel, year, patient);
                }
                else
                {
                    intervention = AddNewIntervention(localIntervention, intervention, dsaModel, year, patient);
                }
                dsaModel.SaveChanges();
                return intervention.Id;
            }
        }

        private Intervention AddNewIntervention(LocalIntervention localIntervention, Intervention intervention,
            dsaEntities dsaModel, Year year, Patient patient)
        {
            intervention = new Intervention()
            {
                Area =
                    localIntervention.Area != null
                        ? dsaModel.Areas.FirstOrDefault(item => item.Name == localIntervention.Area.Name)
                        : null,
                Location =
                    localIntervention.Location != null
                        ? dsaModel.Locations.FirstOrDefault(item => item.Name == localIntervention.Location.Name)
                        : null,
                Work = dsaModel.Works.FirstOrDefault(item => item.Name == localIntervention.Lucrare.Name),
                Revenue = localIntervention.Revenue,
                DateHourDetail = new DateHourDetail()
                {
                    MiliTime = localIntervention.DateHourDetail.Mili,
                    Date = localIntervention.DateHourDetail.Date,
                    StartHour = localIntervention.DateHourDetail.StartHour,
                    EndingHour = localIntervention.DateHourDetail.EndingHour,
                    Duration =
                        (localIntervention.DateHourDetail.EndingHour -
                         localIntervention.DateHourDetail.StartHour).Ticks
                },
                Year = year,
                Month =
                    dsaModel.Months.FirstOrDefault(
                        item => item.MOnthNumber == localIntervention.DateHourDetail.Date.Month),
                Material =
                    localIntervention.Material != null
                        ? dsaModel.Materials.FirstOrDefault(item => item.Id == localIntervention.Material.Id)
                        : null,
                Observation = localIntervention.Observation,
                WasPayedByDental = localIntervention.WasPayedByDental,
                Patient = patient,
                Percent = localIntervention.Percent,
                MaterialCost = localIntervention.MaterialCost,
                TechnicianId = localIntervention.TechnicianId != 0 ? localIntervention.TechnicianId : null
            };
            dsaModel.Interventions.AddObject(intervention);
            return intervention;
        }

        private void EditIntervention(LocalIntervention localIntervention, Intervention intervention,
            dsaEntities dsaModel, Year year, Patient patient)
        {
            //setate dupa nume din cauza loadului din fisier
            intervention.Area = localIntervention.Area != null
                ? dsaModel.Areas.FirstOrDefault(item => item.Name == localIntervention.Area.Name)
                : null;
            intervention.Location = localIntervention.Location != null
                ? dsaModel.Locations.FirstOrDefault(item => item.Name == localIntervention.Location.Name)
                : null;
            intervention.Work = dsaModel.Works.FirstOrDefault(item => item.Name == localIntervention.Lucrare.Name);
            if (localIntervention.WorkType != null)
                intervention.WorkType = dsaModel.WorkTypes.FirstOrDefault(item => item.Id == localIntervention.WorkType.Id);
            intervention.Revenue = localIntervention.Revenue;
            intervention.DateHourDetail = new DateHourDetail()
            {
                MiliTime = localIntervention.DateHourDetail.Mili,
                Date = localIntervention.DateHourDetail.Date,
                StartHour = localIntervention.DateHourDetail.StartHour,
                EndingHour = localIntervention.DateHourDetail.EndingHour,
                Duration = (localIntervention.DateHourDetail.EndingHour - localIntervention.DateHourDetail.StartHour).Ticks
            };
            intervention.Year = year;
            intervention.Month =
                dsaModel.Months.FirstOrDefault(item => item.MOnthNumber == localIntervention.DateHourDetail.Date.Month);
            if (localIntervention.Material != null)
                intervention.Material = dsaModel.Materials.FirstOrDefault(item => item.Id == localIntervention.Material.Id);
            if (localIntervention.MaterialTYpe != null)
                intervention.Type = dsaModel.Types.FirstOrDefault(item => item.Id == localIntervention.MaterialTYpe.Id);
            intervention.Observation = localIntervention.Observation;
            intervention.WasPayedByDental = localIntervention.WasPayedByDental;
            intervention.Patient = patient;
            intervention.Percent = localIntervention.Percent;
            intervention.TechnicianId = localIntervention.TechnicianId != 0 ? localIntervention.TechnicianId : null;
            intervention.MaterialCost = localIntervention.MaterialCost;
        }

        public void RemoveIntervention(int id)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var interventionToDelete = dsaModel.Interventions.FirstOrDefault(item => item.Id == id);
                dsaModel.Interventions.DeleteObject(interventionToDelete);
                dsaModel.SaveChanges();
            }
        }

        public async Task<bool> SetInterventionsAsPayed(List<int> interventionsIds, bool payedByDental)
        {
            DentalPaymentInfo dentalPaymentInfo = new DentalPaymentInfo();
            using (dsaEntities dsaModel = new dsaEntities())
            {
                foreach (var interventionId in interventionsIds)
                {
                    var currentIntervention = dsaModel.Interventions.FirstOrDefault(item => item.Id == interventionId);
                    //if (currentIntervention.WasPayedByDental.HasValue&&currentIntervention.WasPayedByDental.Value)
                    {

                        currentIntervention.WasPayedByDental = payedByDental;
                        dentalPaymentInfo.TotalPercent += currentIntervention.Percent;
                        dentalPaymentInfo.TotalRevenue += currentIntervention.Revenue;
                    }
                }
                dentalPaymentInfo.Date = DateTime.Now;
                dentalPaymentInfo.Name = DateTime.Now.ToShortDateString();
                // SaveDentalPaymentInfo(dentalPaymentInfo);
                dsaModel.DentalPaymentInfoes.AddObject(dentalPaymentInfo);
                dsaModel.SaveChanges();
                return true;
            }
        }

        public async Task<bool> SetInterventionDentalPaymentStatus(int interventionId, bool payedByDental)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                dsaModel.Interventions.FirstOrDefault(item => item.Id == interventionId).WasPayedByDental = payedByDental;
                dsaModel.SaveChanges();
                return true;
            }
        }

        public void SaveInterventionsFromFile()
        {

        }

        #endregion Intervention

        #region Settings

        public async Task<int> AddWork(SettingsItem localWork)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var work = localWork.ToWork();
                dsaModel.Works.AddObject(work);
                dsaModel.SaveChanges();
                return work.Id;
            }
        }

        public void EditWork(SettingsItem localWork)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var workToEdit = dsaModel.Works.FirstOrDefault(item => item.Id == localWork.Id);
                workToEdit.Name = localWork.Name;
                workToEdit.Percent = localWork.Percent;
                workToEdit.Cost = localWork.Cost;
                workToEdit.IncludedINFinancial = localWork.IncludedINFinancial;
                dsaModel.SaveChanges();
            }
        }

        public async Task<List<string>> DeleteWorks(List<int> deletedWorksIds)
        {
            List<string> unableToDeleteWorks = new List<string>();
            try
            {
                using (dsaEntities dsaModel = new dsaEntities())
                {
                    foreach (var deletedWorkId in deletedWorksIds)
                    {
                        var deletedWork = dsaModel.Works.FirstOrDefault(item => item.Id == deletedWorkId);
                        //var currItem = dsaModel.Interventions.FirstOrDefault(item => item.Work.Id == deletedWorksId);
                        if (deletedWork.Interventions != null && deletedWork.Interventions.Any())
                        {
                            //unableToDeleteWorks.Add(currItem.Work.Name);         
                            deletedWork.WasDeleted = true;
                        }
                        else
                        {

                            dsaModel.Works.DeleteObject(deletedWork);
                        }
                    }
                    dsaModel.SaveChanges();
                    return unableToDeleteWorks;
                }
            }
            catch (Exception e)
            {
                Log.Error("Delete Works" + e.Message);
                return new List<string>();
            }

        }

        public void DeleteWork(int id)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var deletedWork = dsaModel.Works.FirstOrDefault(item => item.Id == id);
                dsaModel.Works.DeleteObject(deletedWork);
                dsaModel.SaveChanges();
            }
        }

        public async Task<int> AddMaterial(SettingsItem localMaterial)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var material = localMaterial.ToMaterial();
                dsaModel.Materials.AddObject(material);
                dsaModel.SaveChanges();
                return material.Id;
            }
        }

        public void EditMaterial(SettingsItem localMaterial)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var materialToEdit = dsaModel.Materials.FirstOrDefault(item => item.Id == localMaterial.Id);
                materialToEdit.Name = localMaterial.Name;
                materialToEdit.Cost = localMaterial.Cost;
                dsaModel.SaveChanges();
            }
        }

        //        public void DeleteMaterial(int id)
        //        {
        //            using (dsaEntities dsaModel = new dsaEntities())
        //            {
        //                var deletedMaterial = dsaModel.Materials.FirstOrDefault(item => item.Id == id);
        //                dsaModel.Materials.DeleteObject(deletedMaterial);
        //                dsaModel.SaveChanges();
        //            }
        //        }

        public async Task<int> AddLocation(SettingsItem localLocation)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var location = localLocation.ToLocation();
                dsaModel.Locations.AddObject(location);
                dsaModel.SaveChanges();
                return location.Id;
            }
        }

        public void EditLocation(SettingsItem localLocation)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var locationToEdit = dsaModel.Locations.FirstOrDefault(item => item.Id == localLocation.Id);
                locationToEdit.Name = localLocation.Name;
                dsaModel.SaveChanges();
            }
        }

        public void DelteLocation(int id)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var deletedLocation = dsaModel.Locations.FirstOrDefault(item => item.Id == id);
                dsaModel.Locations.DeleteObject(deletedLocation);
                dsaModel.SaveChanges();
            }
        }

        public async Task<List<string>> DeleteLocations(List<int> deletedLocationsIds)
        {
            try
            {
                using (dsaEntities dsaModel = new dsaEntities())
                {
                    List<string> unableToDeleteLocations = new List<string>();
                    foreach (var id in deletedLocationsIds)
                    {
                        //                        var currItem = dsaModel.Interventions.FirstOrDefault(item => item.Location.Id == id);
                        var deletedLocation = dsaModel.Locations.FirstOrDefault(item => item.Id == id);
                        if (deletedLocation.Interventions != null && deletedLocation.Interventions.Any())
                        {
                            //                          ?  unableToDeleteLocations.Add(currItem.Location.Name);
                            deletedLocation.WasDeleted = true;
                        }
                        else
                        {
                            dsaModel.Locations.DeleteObject(deletedLocation);
                        }

                    }
                    dsaModel.SaveChanges();
                    return unableToDeleteLocations;
                }
            }
            catch (Exception e)
            {
                Log.Error("Delete Locations" + e.Message);
                return new List<string>();
            }

        }

        public async Task<int> AddArea(SettingsItem localArea)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var area = localArea.ToArea();
                dsaModel.Areas.AddObject(area);
                dsaModel.SaveChanges();
                return area.Id;
            }
        }

        public void EditArea(SettingsItem localArea)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var areaToEdit = dsaModel.Areas.FirstOrDefault(item => item.Id == localArea.Id);
                areaToEdit.Name = localArea.Name;
                dsaModel.SaveChanges();
            }
        }

        public void DeleteArea(int id)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var deletedArea = dsaModel.Areas.FirstOrDefault(item => item.Id == id);
                dsaModel.Areas.DeleteObject(deletedArea);
                dsaModel.SaveChanges();
            }
        }

        public async Task<List<string>> DeleteAreas(List<int> deletedAreasIds)
        {
            try
            {
                using (dsaEntities dsaModel = new dsaEntities())
                {
                    List<string> unableToDeleteAreas = new List<string>();
                    foreach (var id in deletedAreasIds)
                    {
                        //   var currItem = dsaModel.Interventions.FirstOrDefault(item => item.Area.Id == id);
                        var deletedLocation = dsaModel.Areas.FirstOrDefault(item => item.Id == id);
                        if (deletedLocation.Interventions != null && deletedLocation.Interventions.Any())
                        {
                            //                            unableToDeleteAreas.Add(currItem.Area.Name);
                            deletedLocation.WasDeleted = true;
                        }
                        else
                        {
                            dsaModel.Areas.DeleteObject(deletedLocation);
                        }
                    }
                    dsaModel.SaveChanges();
                    return unableToDeleteAreas;
                }
            }
            catch (Exception e)
            {
                Log.Error("Delete Areas" + e.Message);
                return new List<string>();
            }
        }

        public void EditSetting(SettingsItem settingsItem, LocalCache.SettingsItemEnum settingsitemType)
        {
            new Task(delegate
            {
                switch (settingsitemType)
                {
                    case LocalCache.SettingsItemEnum.Locatie:
                        EditLocation(settingsItem);
                        break;
                    case LocalCache.SettingsItemEnum.Manopera:
                        EditWork(settingsItem);
                        break;
                    case LocalCache.SettingsItemEnum.Material:
                        EditMaterial(settingsItem);
                        break;
                    case LocalCache.SettingsItemEnum.Zona:
                        EditArea(settingsItem);
                        break;
                }
            }).Start();
        }

        public async Task<int> AddSetting(SettingsItem settingsItem, LocalCache.SettingsItemEnum settingsitemType)
        {
            switch (settingsitemType)
            {
                case LocalCache.SettingsItemEnum.Locatie:
                    return await AddLocation(settingsItem);
                case LocalCache.SettingsItemEnum.Manopera:
                    return await AddWork(settingsItem);
                case LocalCache.SettingsItemEnum.Material:
                    return await AddMaterial(settingsItem);
                case LocalCache.SettingsItemEnum.Zona:
                    return await AddArea(settingsItem);
                case LocalCache.SettingsItemEnum.Tehnician:
                    return await AddTechnician(settingsItem);
                    break;
            }
            return 0;
        }

        public async Task<int> AddTechnician(SettingsItem localTechnician)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var enityTech = dsaModel.Technicians.FirstOrDefault(item => item.Id == localTechnician.Id);
                if (enityTech != null)
                {
                    enityTech.Name = localTechnician.Name;
                }
                else
                {
                    enityTech = new Technician()
                    {
                        Name = localTechnician.Name
                    };
                    dsaModel.Technicians.AddObject(enityTech);
                }
                dsaModel.SaveChanges();
                return enityTech.Id;
            }
        }

        //        public async Task<LocalTechnician> AddTechnicianAndMaterials(LocalTechnician localTechnician)
        //        {
        //            using (dsaEntities dsaModel = new dsaEntities())
        //            {
        //                var technician = dsaModel.Technicians.FirstOrDefault(item => item.Id == localTechnician.Id);
        //                if (technician != null)
        //                {
        //                    technician.Name = localTechnician.Name;
        //                    foreach (var localMaterial in localTechnician.Materials)
        //                    {
        //                        var material = technician.Materials.FirstOrDefault(item => item.Id == localMaterial.Id);
        //                        if (material != null)
        //                        {
        //                            material.Name = localMaterial.Name;
        //                            material.Cost = localMaterial.Cost;
        //                            //set types
        //                            foreach (var materialType in localMaterial.MaterialTypes)
        //                            {
        //                                var type = material.Types.FirstOrDefault(item => item.Id == materialType.Id);
        //                                if (type != null)
        //                                {
        //                                    type.Name = materialType.Name;
        //                                    type.Cost = materialType.Cost;
        //                                }
        //                                else
        //                                {
        //                                    type = new Type();
        //                                    type.Name = materialType.Name;
        //                                    type.Cost = materialType.Cost;
        //                                    material.Types.Add(type);
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            material = new Material();
        //                            material.Name = localMaterial.Name;
        //                            material.Cost = localMaterial.Cost;
        //                            material.Types = new EntityCollection<Type>();
        //                            foreach (var materialType in localMaterial.MaterialTypes)
        //                            {
        //                                material.Types.Add(new Type()
        //                                {
        //                                    Name = materialType.Name,
        //                                    Cost = materialType.Cost
        //                                });
        //                            }
        //
        //                            technician.Materials.Add(material);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    technician = new Technician();
        //                    technician.Name = localTechnician.Name;
        //                    technician.Materials = new EntityCollection<Material>();
        //                    //set materials
        //                    foreach (var localMaterial in localTechnician.Materials)
        //                    {
        //                        var material = new Material()
        //                        {
        //                            Cost = localMaterial.Cost,
        //                            Name = localMaterial.Name,
        //                            Types = new EntityCollection<Type>()
        //                        };
        //
        //                        //set types for material
        //                        foreach (var materialType in localMaterial.MaterialTypes)
        //                        {
        //                            material.Types.Add(new Type()
        //                            {
        //                                Name = materialType.Name,
        //                                Cost = materialType.Cost
        //                            });
        //                        }
        //
        //                        technician.Materials.Add(material);
        //                    }
        //                }
        //                dsaModel.SaveChanges();
        //                return technician.ToLocalTechnician();
        //            }
        //        }

        public async Task<bool> DeleteMaterials(List<int> materialIds)
        {
            try
            {
                using (dsaEntities dsaModel = new dsaEntities())
                {
                    foreach (var materialId in materialIds)
                    {
                        var materialToDelete = dsaModel.Materials.FirstOrDefault(item => item.Id == materialId);
                        var typesIds = materialToDelete.Types.Select(item => item.Id).ToList();
                        foreach (var typeId in typesIds)
                        {
                            var typeTodelete = dsaModel.Types.FirstOrDefault(item => item.Id == typeId);
                            if (typeTodelete.Interventions != null && typeTodelete.Interventions.Any())
                                if (typeTodelete != null)
                                    dsaModel.Types.DeleteObject(typeTodelete);
                        }
                        if (materialToDelete.Interventions != null && materialToDelete.Interventions.Any())
                        {
                            materialToDelete.WasDeleted = true;
                        }
                        else
                        {
                            dsaModel.Materials.DeleteObject(materialToDelete);
                        }
                    }
                    dsaModel.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Error(string.Format("DatabaseHandler -> DeleteMaterial,Error Message" + e.Message));
                return false;
            }
        }

        public async Task<bool> DeleteMaterialTypes(List<int> typesIds)
        {
            try
            {
                using (dsaEntities dsaModel = new dsaEntities())
                {
                    foreach (var typeId in typesIds)
                    {
                        var typeTodelete = dsaModel.Types.FirstOrDefault(item => item.Id == typeId);
                        if (typeTodelete != null)
                            dsaModel.Types.DeleteObject(typeTodelete);
                    }
                    dsaModel.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Error(string.Format("DatabaseHandler -> DeleteMaterialTypes,Error Message:" + e.Message));
                return false;
            }
        }

        #endregion Settings

        #region Filters

        public async Task<LocalFilterGroup> SaveEditFilterGroup(LocalFilterGroup localFilterGroup)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var filterGroup = dsaModel.FilterGroups.FirstOrDefault(item => item.Id == localFilterGroup.Id);
                bool shouldAddGroup = false;
                if (filterGroup == null)
                {
                    filterGroup = new FilterGroup();
                    shouldAddGroup = true;
                }
                filterGroup.Name = localFilterGroup.Name;
                filterGroup.IsIntervalActive = localFilterGroup.IsIntervalActive;
                filterGroup.StartDate = localFilterGroup.StartDate;
                filterGroup.EndDate = localFilterGroup.EndDate;
                filterGroup.IsActive = localFilterGroup.IsActive;
                foreach (var localFilter in localFilterGroup.Filters)
                {

                    var filter = localFilter.Id != 0 ? filterGroup.Filters.FirstOrDefault(item => item.Id == localFilter.Id) : null;
                    bool shoulDAddfilter = false;
                    if (filter == null)
                    {
                        filter = new Filter();
                        shoulDAddfilter = true;
                    }
                    var filterType = dsaModel.FilterTypes.FirstOrDefault(item => item.Name == localFilter.Type);
                    if (filterType == null)
                    {
                        filterType = new FilterType();
                        filterType.Name = localFilter.Type;
                        dsaModel.FilterTypes.AddObject(filterType);
                    }
                    filter.IsActive = localFilter.IsActive;
                    foreach (var localFilterItem in localFilter.FilterItems)
                    {
                        var filterItem = filter.FilterItems.FirstOrDefault(item => item.Name == localFilterItem.Name);
                        if (filterItem == null)
                        {
                            filterItem = new FilterItem();
                            filterItem.Name = localFilterItem.Name;
                            filter.FilterItems.Add(filterItem);
                        }
                    }
                    //delete filterItems
                    foreach (var itemToDeleteId in localFilter.ItemsToDeleteIds)
                    {
                        var filterItemToDelete = dsaModel.FilterItems.FirstOrDefault(item => item.Id == itemToDeleteId);
                        filterItemToDelete.Filters.Remove(filter);
                        filter.FilterItems.Remove(filterItemToDelete);
                    }
                    filterType.Filters.Add(filter);
                    if (shoulDAddfilter)
                        filterGroup.Filters.Add(filter);
                }
                if (shouldAddGroup)
                    dsaModel.FilterGroups.AddObject(filterGroup);
                dsaModel.SaveChanges();
                return filterGroup.ToLocalFilterGroup();
            }
        }

        public async Task<List<LocalFilterGroup>> GetFilterGroups()
        {
            List<LocalFilterGroup> localFilterGroups = new List<LocalFilterGroup>();
            using (dsaEntities dsaModel = new dsaEntities())
            {
                foreach (var filterGroup in dsaModel.FilterGroups)
                {
                    localFilterGroups.Add(filterGroup.ToLocalFilterGroup());
                }
            }
            return localFilterGroups;
        }

        public void PopulateTypes()
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                foreach (var type in Enum.GetValues(typeof(LocalCache.SettingsItemEnum)))
                {
                    dsaModel.FilterTypes.AddObject(new FilterType()
                    {
                        Name = type + ":"
                    });
                }
                dsaModel.SaveChanges();
            }
        }

        #endregion Filters

        #region dentailPayment

        public async Task<bool> SaveDentalPaymentInfo(DentalPaymentInfo localDentalPaymentInfo)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                dsaModel.DentalPaymentInfoes.AddObject(localDentalPaymentInfo);
                dsaModel.SaveChanges();
            }
            return true;
        }

        public async Task<LocalDentalPaymentInfo> getLastDentalPaymentInfo()
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                return dsaModel.DentalPaymentInfoes.ToList().LastOrDefault().ToLocalDentalPaymentInfo();
            }

        }

        #endregion dentalPayment

        #region Login


        public LoginResponse Login(string username, string password)
        {
            LoginResponse loginResponse = new LoginResponse();
            using (dsaEntities dsaModel = new dsaEntities())
            {
                var currentUser = dsaModel.Users.FirstOrDefault(item => item.username == username);
                if (currentUser != null)
                {
                    bool shouldContinue = false;
                    foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                    {
                        if (nic.NetworkInterfaceType.ToString() == "Ethernet" ||
                            nic.NetworkInterfaceType.ToString().ToLower().Contains("wireless"))
                        {
                            string ecryptedMac = EncryptString(nic.GetPhysicalAddress().ToString());
                            Log.Debug(
                                "Encrypted MAC Address: " + ecryptedMac +
                                " Type: " + nic.NetworkInterfaceType);
                            if (currentUser.MACs.Any(item => item.macId == ecryptedMac))
                            {
                                shouldContinue = true;
                                break;
                            }
                        }
                    }
                    if (shouldContinue)
                    {
                        if (currentUser.password == EncryptString(password))
                        {
                            loginResponse.Description = "";
                            loginResponse.Status = LoginStatus.Successful;
                            XmlSerializerHelper.SaveToXml(ViewConstants.appDataPath, new LocalUser()
                            {
                                Username = username
                            });
                        }
                        else
                        {
                            loginResponse.Description = "Parola Gresita!";
                            loginResponse.Status = LoginStatus.Unsuccessful;
                        }
                    }
                    else
                    {
                        loginResponse.Description = "Pentru a folosi aplicatia aveti nevoie de licenta!";
                        loginResponse.Status = LoginStatus.Unsuccessful;
                        return loginResponse;
                    }
                }
                else
                {
                    loginResponse.Description = "Nume Utilizator gresit!";
                    loginResponse.Status = LoginStatus.Unsuccessful;
                }
            }
            return loginResponse;
        }

        public string EncryptString(string ClearText)
        {
            byte[] clearTextBytes = Encoding.UTF8.GetBytes(ClearText);

            System.Security.Cryptography.SymmetricAlgorithm rijn = SymmetricAlgorithm.Create();
            MemoryStream ms = new MemoryStream();
            byte[] rgbIV = Encoding.ASCII.GetBytes("ryojvlzffalyglrj");
            byte[] key = Encoding.ASCII.GetBytes("hcxilkqbbffffeultgbskdmaunivmfuo");
            CryptoStream cs = new CryptoStream(ms, rijn.CreateEncryptor(key, rgbIV),
                CryptoStreamMode.Write);

            cs.Write(clearTextBytes, 0, clearTextBytes.Length);
            cs.Close();

            return Convert.ToBase64String(ms.ToArray());
        }

        public void EditUser(LocalUser localUser)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                try
                {
                    var user = dsaModel.Users.FirstOrDefault();
                    if (user != null)
                    {
                        user.username = localUser.Username;
                        user.password = EncryptString(localUser.Password);
                    }
                    dsaModel.SaveChanges();
                    //    MessageBox.Show("Numele Utilizator si parola au fost moficate");
                }
                catch (Exception)
                {
                    MessageBox.Show("Eroare la salvare");
                }
            }
        }

        #endregion Login

        #region IntervalInfo

        public void AddIntervalInfo(LocalDentalPaymentInfo info)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                dsaModel.IntervalInfoes.AddObject(new IntervalInfo()
                {
                    Name = info.Name,
                    TotalPercent = info.TotalPercent,
                    TotalRevenue = info.TotalRevenue,
                });
                dsaModel.SaveChanges();
            }

        }

        #endregion IntervalInfo

        #region Materials

        public async Task<List<LocalWork>> GetWorksWithTypes()
        {
            List<LocalWork> works = new List<LocalWork>();
            using (dsaEntities dsaModel = new dsaEntities())
            {
                return dsaModel.Works.ToLocalWorks();
            }
        }

        public async Task<List<LocalWork>> SaveEditWorks(List<LocalWork> localWorks)
        {
            using (dsaEntities dsaModel = new dsaEntities())
            {
                foreach (var localWork in localWorks)
                {
                    var workToEdit = dsaModel.Works.FirstOrDefault(item => item.Id == localWork.Id);
                    if (workToEdit == null)
                    {
                        Work newWork = new Work()
                        {
                            Cost = localWork.Cost,
                            Name = localWork.Name,
                            Percent = localWork.Percent,
                            IncludedINFinancial = localWork.IncludedINFinancial,
                        };
                        newWork.WorkTypes = new EntityCollection<WorkType>();
                        foreach (var localWorkType in localWork.WorkTypes)
                        {
                            newWork.WorkTypes.Add(new WorkType()
                            {
                                Id = -1,
                                Cost = localWorkType.Cost,
                                Name = localWorkType.Name,
                                Percent = localWorkType.Percent,
                            });
                        }
                        newWork.Id = -1;
                        dsaModel.Works.AddObject(newWork);
                    }
                    else
                    {
                        workToEdit.Cost = localWork.Cost;
                        workToEdit.Name = localWork.Name;
                        workToEdit.Percent = localWork.Percent;
                        workToEdit.IncludedINFinancial = localWork.IncludedINFinancial;
                        foreach (var localWorkType in localWork.WorkTypes)
                        {
                            var editedWorkType = workToEdit.WorkTypes.FirstOrDefault(item => item.Id == localWorkType.Id);
                            if (editedWorkType == null)
                            {
                                editedWorkType = new WorkType()
                                {
                                    Id = -1,
                                    Cost = localWorkType.Cost,
                                    Name = localWorkType.Name,
                                    Percent = localWorkType.Percent,
                                };
                                workToEdit.WorkTypes.Add(editedWorkType);
                            }
                            else
                            {
                                editedWorkType.Cost = localWorkType.Cost;
                                editedWorkType.Name = localWorkType.Name;
                                editedWorkType.Percent = localWorkType.Percent;
                            }
                        }
                    }
                }
                dsaModel.SaveChanges();
                return dsaModel.Works.ToLocalWorks();
            }
        }

        public async Task<bool> DeleteWorksById(List<int> ids)
        {
            try
            {
                using (dsaEntities dsaModel = new dsaEntities())
                {
                    foreach (var id in ids)
                    {
                        var workToDelete = dsaModel.Works.FirstOrDefault(item => item.Id == id);
                        dsaModel.Works.DeleteObject(workToDelete);
                    }
                    dsaModel.SaveChanges();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Manopera nu poate fi stearsa.Exista cel putin o interventie cu aceasta manopera");
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteWorkTypesById(List<int> ids)
        {
            try
            {
                using (dsaEntities dsaModel = new dsaEntities())
                {
                    foreach (var id in ids)
                    {
                        var workTypeToDelete = dsaModel.WorkTypes.FirstOrDefault(item => item.Id == id);
                        dsaModel.WorkTypes.DeleteObject(workTypeToDelete);
                    }
                    dsaModel.SaveChanges();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Tipul de manopera nu poate fi sters. Exista cel putin o intervetie cu acest tip");
            }
            return true;
        }

        #endregion Materials

        #region ProgramInfo

        public LocalProgramInfo GetProgramInfo()
        {
            using (dsaEntities dsaEntities = new dsaEntities())
            {
                var progInfo = dsaEntities.ProgramInfoes.FirstOrDefault();
                if (progInfo != null)
                {
                    return new LocalProgramInfo()
                    {
                        ExportFilePath = progInfo.ExportFileName,
                        LastImportDateFile = progInfo.LastFileDateImport,
                        LastInterventionAddedTime = progInfo.LastAddedInterventionDate,
                        SetAsNewStartDate = progInfo.LastAddedInterventionDate
                    };
                }
                return null;
            }
        }

        public void AddEditProgramInfo(LocalProgramInfo localProgramInfo)
        {
            using (dsaEntities dsaEntities = new dsaEntities())
            {
                var progInfo = dsaEntities.ProgramInfoes.FirstOrDefault();
                if (progInfo != null)
                {
                    progInfo.ExportFileName = localProgramInfo.ExportFilePath;
                    progInfo.LastFileDateImport = localProgramInfo.LastImportDateFile;
                }
                else
                {
                    progInfo = new ProgramInfo()
                    {
                        LastFileDateImport = localProgramInfo.LastImportDateFile,
                        ExportFileName = localProgramInfo.ExportFilePath,
                        LastAddedInterventionDate = new DateTime(2000, 1, 1)
                    };
                    dsaEntities.ProgramInfoes.AddObject(progInfo);
                }
                dsaEntities.SaveChanges();
            }
        }

        public void EditPrInfoLastFIleDate(DateTime newDate)
        {
            using (dsaEntities dsaEntities = new dsaEntities())
            {
                var progInfo = dsaEntities.ProgramInfoes.FirstOrDefault();
                progInfo.LastFileDateImport = newDate;
                dsaEntities.SaveChanges();
            }
        }


        public void EditLastAddedIntervention(DateTime newDate)
        {
            using (dsaEntities dsaEntities = new dsaEntities())
            {
                var progInfo = dsaEntities.ProgramInfoes.FirstOrDefault();
                progInfo.LastAddedInterventionDate = newDate;
                dsaEntities.SaveChanges();
            }
        }

        #endregion ProgramInfo

    }
}
