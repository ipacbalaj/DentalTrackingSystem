using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA.Financial.Entities
{
    public class ValueOccurenceNumber
    {
        public ValueOccurenceNumber(double value,int occNb)
        {
            Value = value;
            Occurence = occNb;
        }

        public double Value { get; set; }
        public int Occurence { get; set; }

    }
}
