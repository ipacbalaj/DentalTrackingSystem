using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA.Database.Model.Entities
{
    public class LocalDateHourDetail
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartHour { get; set; }
        public DateTime EndingHour { get; set; }
        public TimeSpan Duration { get; set; }

    }
}
