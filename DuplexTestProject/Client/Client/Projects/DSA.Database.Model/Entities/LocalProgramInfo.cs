using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA.Database.Model.Entities
{
    public class LocalProgramInfo
    {
        public string ExportFilePath { get; set; }
        public DateTime LastImportDateFile { get; set; }
        public DateTime LastInterventionAddedTime { get; set; }
        public DateTime SetAsNewStartDate { get; set; }

    }
}
