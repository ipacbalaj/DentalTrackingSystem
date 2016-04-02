using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Database.Model.Entities.SettingsEntities;

namespace DSA.Database.Model.Entities
{
    public class LocalIntervention
    {
        public int Id { get; set; }
        public String Observation { get; set; }

        public SettingsItem Material { get; set; }        
        public SettingsType MaterialTYpe { get; set; }
        public LocalLocation Location { get; set; }
        public LocalArea Area { get; set; }
        public double Revenue { get; set; }
        public LocalDateHourDetail DateHourDetail { get; set; }
        public LocalWork Lucrare { get; set; }
        public LocalWorkType WorkType { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public double Percent { get; set; }      
        public bool WasPayedByDental { get; set; }
        public double? MaterialCost { get; set; }
        public int? TechnicianId { get; set; }      
        public LocalTechnician Technician { get; set; }
}
}
