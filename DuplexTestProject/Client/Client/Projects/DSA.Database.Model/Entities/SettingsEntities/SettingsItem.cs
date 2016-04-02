using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA.Database.Model.Entities.SettingsEntities
{
    public class SettingsItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public double Percent { get; set; }
        public bool IncludedINFinancial { get; set; }
        public int TechnicianId { get; set; }
    }
}
