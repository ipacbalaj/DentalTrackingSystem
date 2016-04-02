using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA.Database.Model.Entities.Filters
{
    public class LocalFilterGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LocalFilter> Filters { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsIntervalActive { get; set; }
    }
}
