using System.Collections.Generic;

namespace DSA.Database.Model.Entities.SettingsEntities
{
    public class LocalMaterial : SettingsItem
    {
        public LocalMaterial()
        {
            MaterialTypes=new List<SettingsType>();
        }
//        public int Id { get; set; }
     //   public string Name { get; set; }
        public double Cost { get; set; }
        public int TechnicianIDd { get; set; }
        public List<SettingsType> MaterialTypes { get; set; } 
    }
}
