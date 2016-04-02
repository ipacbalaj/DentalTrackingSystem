using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Database.Model.Entities;
using DSA.Database.Model.Entities.SettingsEntities;
using DSA.Database.Model.Helpers;

namespace DSA.Database.Model
{
    public class DatabaseHandler
    {
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

        public List<LocalPatient> GetPatients()
        {
            List<LocalPatient> localPatients=new List<LocalPatient>();
            using (DSAEntities dsaModel = new DSAEntities())
            {
                foreach (var patient in dsaModel.Patients)
                {
                    localPatients.Add(patient.ToLocalPatient());
                }
            }
            return localPatients;
        }

        public int AddPatient(LocalPatient localPatient)
        {
            using (DSAEntities dsaModel = new DSAEntities())
            {
                var patient = localPatient.ToEntityPatient();
                dsaModel.Patients.AddObject(patient);
                dsaModel.SaveChanges();
                return patient.id;
            }
        }
 

        #endregion Patients



        #region Excel

//        public List<LocalTooth> GetTeeth()
//        {
//            List<LocalTooth> teeth=new List<LocalTooth>();
//            using (DSAEntities dsaModel = new DSAEntities())
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
            using (DSAEntities dsaModel = new DSAEntities())
            {foreach (var workValue in workValues)
            {
                dsaModel.Works.AddObject(new Work()
                {
                    Name = workValue
                });
            }
                dsaModel.SaveChanges();
            }
        }

        public void SaveAreas(List<string> areasList )
        {
            using (DSAEntities dsaModel = new DSAEntities())
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
            using (DSAEntities dsaModel = new DSAEntities())
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
            using (DSAEntities dsaModel = new DSAEntities())
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
                    var entityPactient = new Patient();
                    entityPactient.name = localPatient.Name;
                    foreach (var localIntervention in localPatient.Interventions)
                    {
                        var year =
                            dsaModel.Years.FirstOrDefault(
                                item => item.YearNb == localIntervention.DateHourDetail.Date.Year);
                        if (year == null)
                        {
                            year=new Year()
                            {
                                YearNb = localIntervention.DateHourDetail.Date.Year
                            };
                            dsaModel.Years.AddObject(year);
                            dsaModel.SaveChanges();
                        }
                        entityPactient.Interventions.Add(new Intervention()
                        {
                            Area = dsaModel.Areas.FirstOrDefault(item=>item.Name==localIntervention.Area.Name),
                            Location = dsaModel.Locations.FirstOrDefault(item => item.Name == localIntervention.Location.Name),
                            Work = dsaModel.Works.FirstOrDefault(item => item.Name == localIntervention.Lucrare.Name),
                            Revenue = dsaModel.Revenues.FirstOrDefault(item => item.Amount == localIntervention.Revenue.Amount),
                            DateHourDetail = new DateHourDetails()
                            {
                                Date = localIntervention.DateHourDetail.Date,
                                StartHour = localIntervention.DateHourDetail.StartHour,
                                EndingHour = localIntervention.DateHourDetail.EndingHour,
                                Duration = localIntervention.DateHourDetail.EndingHour - localIntervention.DateHourDetail.StartHour
                            },      
                            Year=year,
                            Month = dsaModel.Months.FirstOrDefault(item=>item.MOnthNumber==localIntervention.DateHourDetail.Date.Month),
                            Material = dsaModel.Materials.FirstOrDefault(item => item.Name == localIntervention.Material.Name),
                            Observation = localIntervention.Observation
                        });
                    }
                    dsaModel.Patients.AddObject(entityPactient);
                }
                dsaModel.SaveChanges();
            }

        }

        #endregion Excel

        #region InterventionDetails

        public List<LocalArea> GetAreas()
        {
            List<LocalArea> areas = new List<LocalArea>();
            using (DSAEntities dsaModel = new DSAEntities())
            {
                foreach (var area in dsaModel.Areas)
                {
                    areas.Add(area.ToLocalArea());
                }
            }
            return areas;
        }

        public List<LocalWork> GetWorks()
        {
            List<LocalWork> works = new List<LocalWork>();
            using (DSAEntities dsaModel = new DSAEntities())
            {
                foreach (var work in dsaModel.Works)
                {
                    works.Add(work.TolLocalWork());
                }
            }
            return works;
        }

        public List<LocalLocation> GetlLocations()
        {
            List<LocalLocation> locations = new List<LocalLocation>();
            using (DSAEntities dsaModel = new DSAEntities())
            {
                foreach (var loc in dsaModel.Locations)
                {
                    locations.Add(loc.ToLocalLocation());
                }
            }
            return locations;
        }

        public List<LocalMaterial> GetMaterials()
        {
            List<LocalMaterial> materials = new List<LocalMaterial>();
            using (DSAEntities dsaModel = new DSAEntities())
            {
                foreach (var loc in dsaModel.Materials)
                {
                    materials.Add(loc.ToLocalMaterial());
                }
            }
            return materials;
        }

        #endregion InterventionDetails
    }
}
