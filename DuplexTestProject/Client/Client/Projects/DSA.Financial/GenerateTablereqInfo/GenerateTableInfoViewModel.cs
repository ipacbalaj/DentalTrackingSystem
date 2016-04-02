using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;

namespace DSA.Financial.GenerateTablereqInfo
{
    public class GenerateTableInfoViewModel:ViewModelBase
    {
        #region Constructor

        public GenerateTableInfoViewModel()
        {
            PopulateMonths();            
        }

        #endregion Constructor

        #region Properties

        private List<String> monthsList;
        public List<String> MonthsList
        {
            get { return monthsList; }
            set
            {
                if (value == monthsList)
                    return;
                monthsList = value;
                OnPropertyChanged();
            }
        }

        private string selectedMonth;

        public string SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                if (value == selectedMonth)
                    return;
                selectedMonth = value;
                OnPropertyChanged();
            }
        }

        private int year;

        public int Year
        {
            get { return year; }
            set
            {
                if (value == year)
                    return;
                year = value;
                OnPropertyChanged();
            }
        }

        private string beneficiary;

        public string Beneficiary
        {
            get { return beneficiary; }
            set
            {
                if (value == beneficiary)
                    return;
                beneficiary = value;
                OnPropertyChanged();
            }
        }

        private string provider;

        public string Provider
        {
            get { return provider; }
            set
            {
                if (value == provider)
                    return;
                provider = value;
                OnPropertyChanged();
            }
        }

        private int percent;

        public int Percent
        {
            get { return percent; }
            set
            {
                if (value == percent)
                    return;
                percent = value;
                OnPropertyChanged();
            }
        }

        private double totalSum;

        public double TotalSum
        {
            get { return totalSum; }
            set
            {
                if (value == totalSum)
                    return;
                totalSum = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        private void PopulateMonths()
        {
            MonthsList = new List<string>();
            foreach (var key in LocalCache.Instance.MonthsNames.Keys)
            {
                MonthsList.Add(LocalCache.Instance.MonthsNames[key]);
            }
        }

        #endregion Methods
    }
}
