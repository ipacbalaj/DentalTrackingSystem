using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA.Financial.Entities
{
    public class FinancialInfoTotal
    {
        public string TotalText { get; set; }
        public int TotalNBOfProvServ { get; set; }
        public double DentalPrice { get; set; }
        public double PriceDueToContract { get; set; }
        public double TotalPriceProvServ { get; set; }
        public double TotalDueToContract { get; set; }        
    }
}
