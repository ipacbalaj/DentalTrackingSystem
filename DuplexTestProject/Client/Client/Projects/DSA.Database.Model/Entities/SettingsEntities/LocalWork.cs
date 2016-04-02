using System.Collections.Generic;
using System.Windows.Documents;

namespace DSA.Database.Model.Entities.SettingsEntities
{
    public class LocalWork : SettingsItem
    {
        public LocalWork()
        {
            WorkTypes=new List<LocalWorkType>();
        }
//        public int Id { get; set; }
//        public string Name { get; set; }
          public List<LocalWorkType> WorkTypes { get; set; }
    }
}
