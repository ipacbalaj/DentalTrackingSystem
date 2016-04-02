using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model.Entities.SettingsEntities;
using DSA.Financial.Entities;
using DSA.Financial.FinancialScreen;

namespace DSA.Financial.SumGenerator
{
    public class SumGenerator:ViewModelBase
    {

        #region Constructor

        public SumGenerator(List<SettingsItem> works,FinancialScreenViewModel parent)
        {
            Parent = parent;
            PopulatePrices(works);
        }

        #endregion Constructor


        #region Properties

        private List<double> revenues { get; set; }
        private Dictionary<double, string> prices { get; set; }
        private List<ValueOccurenceNumber> ocurrences { get; set; }

        private double target;

        public double Target
        {
            get { return target; }
            set
            {
                if (value == target)
                    return;
                target = value;
                OnPropertyChanged();
            }
        }

        public FinancialScreenViewModel Parent { get; set; }

        #endregion Properties

        #region Methods

        #endregion Methods

        public void Generate(double targ)
        {
            Target = targ;
            SumUp(revenues, Target);
        }

        public void SumUp(List<double> numbers, double target)
        {
            SumUpRecursive(numbers, target, new List<double>());
        }

        public void SumUpRecursive(List<double> numbers, double target, List<double> partial)
        {
            int s = 0;
            foreach (int x in partial) s += x;

            if (s == target)
            {
                //SetOcurrences(partial);
                MessageBox.Show("sum(" + string.Join(",", partial.ToArray()) + ")=" + target);
            }

            if (s >= target)
                return;
            int c = 0;
            int positionInArray = 0;
            for (int i = 0; i < numbers.Count; i++)
            {
                List<double> remaining = new List<double>();
                double n = numbers[i];
                int p = 0;
                if (c == 3)
                {
                    p = positionInArray + 1;c=0;
                    remaining = new List<double>();
                }

                for (int j = p; j < numbers.Count; j+=new Random(5).Next()) 
                    remaining.Add(numbers[j]);

                List<double> partial_rec = new List<double>(partial);
                partial_rec.Add(n);
                if (remaining.Count == 1) remaining = numbers;
                SumUpRecursive(remaining, target, partial_rec);
                c++;
            }
        }

        private void PopulatePrices(List<SettingsItem> works)
        {
            revenues=new List<double>();
            prices=new Dictionary<double, string>();
            foreach (var displaySettingsItemHelper in works)
            {
                revenues.Add(displaySettingsItemHelper.Cost);
                //prices.Add(displaySettingsItemHelper.Cost,displaySettingsItemHelper.Name);
            }

            new Random().Shuffle(revenues);
        }

//        private void SetOcurrences(List<double> values)
//        {
//            ocurrences=new List<ValueOccurenceNumber>();
//            List<double> testValues = new List<double>() { 1000, 250, 650, 150 };
//            Random random=new Random();
//            foreach (var @decimal in values)
//            {
//
//                var ocVal = ocurrences.FirstOrDefault(item => item.Value == @decimal);
//                if (ocVal != null)
//                {
//                    ocVal.Occurence+=1;
//                }
//                else
//                {
//                    ocurrences.Add(new ValueOccurenceNumber()
//                    {
//                        Occurence = 1,
//                        Value = @decimal
//                    });
//                }
//            }
//        }


    }
}
