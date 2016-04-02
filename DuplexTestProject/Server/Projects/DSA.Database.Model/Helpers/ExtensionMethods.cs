 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Database.Model.Entities;
 using DSA.Database.Model.Entities.SettingsEntities;

namespace DSA.Database.Model.Helpers
{
    public static class PatientsExtensionMethods
    {
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
                Phone = entityPatient.Phone
                
            };
            patient.Interventions=new List<LocalIntervention>();
            foreach (var intervention in entityPatient.Interventions)
            {
                patient.Interventions.Add(intervention.ToLocalIntervention());
            }
            return patient;
        }
        public static Patient ToEntityPatient(this LocalPatient localPatient)
        {
            return new Patient()
            {
                id = localPatient.Id,
                name = localPatient.Name,
                surname = localPatient.Surname,
                Address = localPatient.Address,
                City = localPatient.City,
                Email = localPatient.Email,
                Phone = localPatient.Phone
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
                LocalIntervention Intervention = new LocalIntervention();

                Intervention.Area = new LocalArea()
                {
                    Name = excelResult[key][7]
                };
                Intervention.DateHourDetail = new LocalDateHourDetail()
                {
                    StartHour = DateTime.FromOADate(float.Parse(excelResult[key][9])),
                    EndingHour = DateTime.FromOADate(float.Parse(excelResult[key][10])),                                       
                    Date = Convert.ToDateTime(excelResult[key][2])
                };                  
                Intervention.Location = new LocalLocation()
                {
                    Name = excelResult[key][4]
                };
                Intervention.Material = new LocalMaterial()
                {
                    Name = excelResult[key][6]
                };
                Intervention.Observation = excelResult[key][8];
                Intervention.Revenue = new LocalRevenue()
                {
                    Amount = Convert.ToDecimal(excelResult[key][12])
                };
                Intervention.Lucrare = new LocalWork()
                {
                    Name = excelResult[key][5]
                };
                //patient.Intervention = Intervention;
                var currPatient = patients.FirstOrDefault(item => item.Name == patient.Name);
                if (currPatient!=null)
                {
                   currPatient.Interventions.Add(Intervention);
                }
                else
                {
                    patient.Interventions=new List<LocalIntervention>();
                    patient.Interventions.Add(Intervention);
                    patients.Add(patient);
                }
            }

            return patients;
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
                Name = work.Name
            };
        }

        public static LocalLocation ToLocalLocation(this Location location)
        {
            return new LocalLocation()
            {
                Id = location.Id,
                Name = location.Name
            };
        }

        public static LocalMaterial ToLocalMaterial(this Material material)
        {
            return new LocalMaterial()
            {
                Id = material.Id,
                Name = material.Name
            };
        }

        public static LocalDateHourDetail ToLocalDateHourDetail(this DateHourDetails dateHourDetails)
        {
            return new LocalDateHourDetail()
            {
                Id = dateHourDetails.Id,
                StartHour = dateHourDetails.StartHour,
                EndingHour = dateHourDetails.EndingHour,
                Duration = dateHourDetails.Duration
            };
        }

        public static LocalIntervention ToLocalIntervention(this Intervention intervention)
        {
            return new LocalIntervention()
            {
                Observation = intervention.Observation,
                Id = intervention.Id,
                Area =intervention.Area!=null? intervention.Area.ToLocalArea():null,
                Location = intervention.Location!=null? intervention.Location.ToLocalLocation():null,
                Lucrare =intervention.Work!=null? intervention.Work.TolLocalWork():null,
                Material =intervention.Material!=null? intervention.Material.ToLocalMaterial():null,
                DateHourDetail =intervention.DateHourDetail!=null? new LocalDateHourDetail()
                {
                    Id = intervention.DateHourDetail.Id,
                    StartHour = intervention.DateHourDetail.StartHour,
                    EndingHour = intervention.DateHourDetail.EndingHour,
                    Duration = intervention.DateHourDetail.Duration
                }:null,    
                PatientName = intervention.Patient.surname+" "+intervention.Patient.name
            };
        }
    }
}
