using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA.Database.Model.Entities.Filters
{
    public class LocalFilter
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public List<LocalFilterItem> FilterItems { get; set; } 
        public bool IsActive { get; set; }
        public List<int> ItemsToDeleteIds { get;set; } 

    }
}
