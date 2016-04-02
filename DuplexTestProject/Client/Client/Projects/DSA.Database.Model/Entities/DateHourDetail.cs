using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA.Database.Model.Entities
{
   public class DateHourDetail
    {
       public int Id { get; set; }
       public DateTime StartH { get; set; }
       public DateTime EndingH { get; set; }
       public int Duration { get; set; }
       public long MiliTime { get; set; }
    }
}
