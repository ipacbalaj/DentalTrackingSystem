using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA.Financial.Entities
{
    public class FinancialInfo
    {
        public string ProvidedServices { get; set; }
        public int NumberOfProvServ { get; set; }
        public double DentalPrice { get; set; }
        public double PriceDueToContract { get; set; }
        public double TotalPriceProvServ { get; set; }
        public double TotalDueToContract { get; set; }

    }
}
