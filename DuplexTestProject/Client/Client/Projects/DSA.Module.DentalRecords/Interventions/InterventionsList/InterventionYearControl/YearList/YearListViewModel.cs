using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Common.Infrastructure.Entities;
using DSA.Common.Infrastructure.ObjectList;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.Entities;
using DSA.Database.Model.Entities.SettingsEntities;
using DSA.Module.DentalRecords.DentalRecordsScreen;
using DSA.Module.DentalRecords.Helpers;
using DSA.Module.DentalRecords.Interventions.InterventionsList.InterventionMonthControl;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;

namespace DSA.Module.DentalRecords.Interventions.InterventionsList.InterventionYearControl.YearList
{
    public class YearListViewModel : ViewModelBase
    {
        #region Constructor

        public YearListViewModel(DentalRecordsScreeViewModel parent)
        {
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            Parent = parent;
        }

        #endregion Constructor

        #region Properties

        private readonly IEventAggregator eventAggregator;

        public DentalRecordsScreeViewModel Parent { get; set; }

        private ObjectList<YearInterventionsViewModel> yearList;
        public ObjectList<YearInterventionsViewModel> YearList
        {
            get { return yearList; }
            set
            {
                if (yearList != value)
                {
                    yearList = value;
                    OnPropertyChanged();
                }
            }
        }



        #endregion Properties

        #region Methods

        public void PopulateYears(Dictionary<int, Dictionary<int, List<LocalIntervention>>> yearMonthInterventions)
        {
            YearList = new ObjectList<YearInterventionsViewModel>(true);
            LocalCache.Instance.SettingsItems.Add(LocalCache.SettingsItemEnum.An, new List<SettingsItem>());
            int totalInterventions = 0;
            double totalRevenue = 0;
            double totalProfit = 0;
            TimeSpan totalDuration = new TimeSpan();
            var listOfYears = new List<YearInterventionsViewModel>();
            foreach (var yearKey in yearMonthInterventions.Keys)
            {
                var newYear = new YearInterventionsViewModel(yearMonthInterventions[yearKey], yearKey, this);
                // YearList.Add(newYear);
                listOfYears.Add(newYear);
                totalInterventions += newYear.NumarInterventii;
                totalRevenue += newYear.TotalIncasari;
                totalDuration += newYear.TotalHours;
                totalProfit += newYear.TotalProfit;
                LocalCache.Instance.SettingsItems[LocalCache.SettingsItemEnum.An].Add(new SettingsItem()
                {
                    Id = yearKey,
                    Name = yearKey.ToString()
                });
            }
            eventAggregator.GetEvent<TotalsModifiedEvent>().Publish(new TotalsIfo
            {
                TotalHours = totalDuration,
                TotalInverventions = totalInterventions,
                TotalRevenue = totalRevenue,
                TotalProfit = totalProfit,
            });
            listOfYears = listOfYears.OrderBy(item => item.Year).ToList();
            foreach (var yearInterventionsViewModel in listOfYears)
            {
                YearList.Add(yearInterventionsViewModel);
            }
        }

        public void AddYear(InterventionDetails interventionDetails)
        {
            int year = interventionDetails.Date.Year;
            var yearToAddTo = YearList.List.FirstOrDefault(item => item.Year == year);
            if (yearToAddTo != null)
            {
                yearToAddTo.AddInterventionToMonth(interventionDetails);
            }
            else
            {
                yearToAddTo = new YearInterventionsViewModel(new Dictionary<int, List<LocalIntervention>>(), year, this);
                YearList.Add(yearToAddTo);
                yearToAddTo.AddInterventionToMonth(interventionDetails);
                LocalCache.Instance.SettingsItems[LocalCache.SettingsItemEnum.An].Add(new SettingsItem()
                {
                    Id = year,
                    Name = year.ToString()
                });

            }

        }

        public void RemoveInterventionFromYear(InterventionDetails interventionDetails)
        {
            int year = interventionDetails.Date.Year;
            var yearToRemoveFrom = YearList.List.FirstOrDefault(item => item.Year == year);
            yearToRemoveFrom.removeInterventionFromMonth(interventionDetails);
        }

        public void FIlter()
        {
            foreach (var yearInterventionsViewModel in YearList.List)
            {
                yearInterventionsViewModel.FIlter();
            }
        }

        public void ChangePaymentStatus(bool payed)
        {
            foreach (var key in LocalCache.Instance.SelectedInterventions.Keys)
            {
                var selectedYear = YearList.List.FirstOrDefault(item => item.Year == key / 100);
                selectedYear.ChangePaymentStatus(key % 100, payed);
            }
        }

        #endregion Methods

    }
}
