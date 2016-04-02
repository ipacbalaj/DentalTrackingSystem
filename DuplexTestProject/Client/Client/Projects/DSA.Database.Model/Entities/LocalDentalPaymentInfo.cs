using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA.Database.Model.Entities
{
    public class LocalDentalPaymentInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double TotalRevenue { get; set; }
        public double TotalPercent { get; set; }
    }
}
