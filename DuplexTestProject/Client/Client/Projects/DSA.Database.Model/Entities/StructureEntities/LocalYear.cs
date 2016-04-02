using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA.Database.Model.Entities.StructureEntities
{
    public class LocalYear
    {
        public int Id { get; set; }
        public int YearNb { get; set; }
        public List<LocalIntervention> Interventions { get; set; } 
    }
}
