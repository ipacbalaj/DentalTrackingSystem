using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA.Database.Model.Entities
{
    public class LocalAppointment
    {
        public int Id { get; set; }
        public DateTime DateHour { get; set; }
        public int PatientId { get; set; }
        public int WeekNumber { get; set; }
        public int Year { get; set; }

    }
}
