using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA.Financial
{
    public  static class ExtensionMethods
    {
        public static void Shuffle(this Random rng, List<double> array)
        {
            int n = array.Count;
            while (n > 1)
            {
                int k = rng.Next(n--);
                double temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}
