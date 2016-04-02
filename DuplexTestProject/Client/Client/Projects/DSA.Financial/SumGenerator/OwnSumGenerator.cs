using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using DevExpress.Data;
using DSA.Database.Model;
using DSA.Database.Model.Entities.SettingsEntities;
using DSA.Financial.Entities;
using log4net;

namespace DSA.Financial.SumGenerator
{
    public class OwnSumGenerator
    {
        private List<double> valuesToAdd;
        public Dictionary<string, double> workNameAndValue;
        //        public Dictionary<int, ValueOccurenceNumber> worksValues; 
        private Dictionary<double, int> ValueAndOccurence;
        public OwnSumGenerator(List<LocalWork> works)
        {
            this.works = works;
        }
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<LocalWork> works;

        private void PopulateList()
        {
            try
            {
                valuesToAdd = new List<double>();
                workNameAndValue = new Dictionary<string, double>();
                ValueAndOccurence = new Dictionary<double, int>();
                int index = 0;
                foreach (var settingsItem in works)
                {
                    if (!valuesToAdd.Contains(settingsItem.Cost))
                    {
                        valuesToAdd.Add(settingsItem.Cost);
                        ValueAndOccurence.Add(settingsItem.Cost, 0);
                        workNameAndValue.Add(settingsItem.Name, settingsItem.Cost);
                    }
                    // worksValues.Add(index, new ValueOccurenceNumber(settingsItem.Cost, 0));
                }
            }
            catch (Exception)
            {
                //  MessageBox.Show("Eroare generare tabel.Completati toate campurile");
                Log.Error("Financial.Multiple works with same name");
            }
        }

        public List<FinancialInfo> GenerateList(double target, int percent)
        {
            PopulateList();
            try
            {
                List<FinancialInfo> infoToDisplayList = new List<FinancialInfo>();
                int count = valuesToAdd.Count;
                double sum = 0;
                int index = 0;
                var random = new Random();
                int repeats = 0;
                while (sum < target)
                {
                    index = random.Next(count);
                    double selectedValue = valuesToAdd[index];
                    ValueAndOccurence[selectedValue] += 1;
                    sum += selectedValue;
                    repeats += 1;
                    if (repeats == 1000000)
                    {
                        MessageBox.Show("Exista prea multe manopere avand Costul 0!");
                        return new List<FinancialInfo>();
                    }
                }

                int costOccurences;
                foreach (var key in ValueAndOccurence.Keys)
                {
                    var worksWithCurrentCost = works.Where(item => item.Cost == key).ToList();
                    costOccurences = ValueAndOccurence[key];
                    // take works randomly till number is < occurence * cost (suppose its correct)                
                    //foreach (var settingsItem in worksWithCurrentCost)

                    while (costOccurences > 0)
                    {
                        int numberOfAppearces = random.Next(costOccurences - (costOccurences / 2) + 1);
                        int costIndex = random.Next(worksWithCurrentCost.Count);
                        if (worksWithCurrentCost.Count > 0)
                        {
                            var currentWork = worksWithCurrentCost[costIndex];
                            if (worksWithCurrentCost.Count == 1)
                            {
                                infoToDisplayList.Add(new FinancialInfo()
                                {
                                    ProvidedServices = currentWork.Name,
                                    DentalPrice = currentWork.Cost,
                                    NumberOfProvServ = costOccurences >= 0 ? costOccurences : 0,
                                    PriceDueToContract = (currentWork.Cost * percent) / 100,
                                    TotalPriceProvServ = costOccurences * currentWork.Cost,
                                    TotalDueToContract = costOccurences * currentWork.Cost * percent / 100
                                });
                                costOccurences = -1;
                            }
                            else
                            {
                                infoToDisplayList.Add(new FinancialInfo()
                                {

                                    ProvidedServices = currentWork.Name,
                                    DentalPrice = currentWork.Cost,
                                    NumberOfProvServ = numberOfAppearces >= 0 ? numberOfAppearces : 0,
                                    PriceDueToContract = (currentWork.Cost * percent) / 100,
                                    TotalPriceProvServ = numberOfAppearces * currentWork.Cost,
                                    TotalDueToContract = numberOfAppearces * currentWork.Cost * percent / 100
                                });
                            }
                            worksWithCurrentCost.Remove(currentWork);
                            costOccurences -= numberOfAppearces;

                        }
                    }

                }

                var numeObturatie =LocalCache.Instance.Works.FirstOrDefault(item => item.Id == 14).Name;
                var obturatie = infoToDisplayList.FirstOrDefault(item => item.ProvidedServices == numeObturatie);
                obturatie.TotalPriceProvServ = obturatie.TotalPriceProvServ - (sum - target);
                obturatie.TotalDueToContract = (obturatie.TotalDueToContract * 100 / percent - (sum - target)) * percent / 100;
                return infoToDisplayList;
            }
            catch (Exception e)
            {
                Log.Error("GenerateList " + e.Message);
                return new List<FinancialInfo>();
            }

        }
    }
}
