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

        public LocalMaterial Material { get; set; }
        public LocalLocation Location { get; set; }
        public LocalArea Area { get; set; }
        public LocalRevenue Revenue { get; set; }
        public LocalDateHourDetail DateHourDetail { get; set; }
        public LocalWork Lucrare { get; set; }

        public string PatientName { get; set; }
    }
}
