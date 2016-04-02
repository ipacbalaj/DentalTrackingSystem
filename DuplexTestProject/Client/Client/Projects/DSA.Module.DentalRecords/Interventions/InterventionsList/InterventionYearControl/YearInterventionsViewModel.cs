using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Infrastructure.ObjectList;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model.Entities;
using DSA.Module.DentalRecords.Helpers;
using DSA.Module.DentalRecords.Interventions.InterventionsList.InterventionMonthControl;
using DSA.Module.DentalRecords.Interventions.InterventionsList.InterventionYearControl.YearList;

namespace DSA.Module.DentalRecords.Interventions.InterventionsList.InterventionYearControl
{
    public class YearInterventionsViewModel : BindableObjectListBase<YearInterventionsViewModel>
    {
        #region Constructor

        public YearInterventionsViewModel(Dictionary<int,List<LocalIntervention>> interventionsOnMonths,int year,YearListViewModel parent)
        {
            Parent = parent;
            PopulateMonthsInterventions(interventionsOnMonths);
            Year = year;
        }

        #endregion Constructor

        #region Properties

        public YearListViewModel Parent { get; set; }

        private ObjectList<MonthInterventionsViewModel> monthList;
        public ObjectList<MonthInterventionsViewModel> MonthList
        {
            get { return monthList; }
            set
            {
                if (monthList != value)
                {
                    monthList = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility contentVisibility=Visibility.Collapsed;
        public Visibility ContentVisibility
        {
            get { return contentVisibility; }
            set
            {
                if (contentVisibility != value)
                {
                    contentVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private Brush contentBackground = BackgroundColors.YearTileColor;
        public Brush ContentBackground
        {
            get { return contentBackground; }
            set
            {
                if (contentBackground != value)
                {
                    contentBackground = value;
                    OnPropertyChanged();
                }
            }
        }

        private int year;
        public int Year
        {
            get { return year; }
            set
            {
                if (year != value)
                {
                    year = value;
                    OnPropertyChanged();
                }
            }
        }

        private string expanderRotation="270";
        public string ExpanderRotation
        {
            get { return expanderRotation; }
            set
            {
                if (expanderRotation != value)
                {
                    expanderRotation = value;
                    OnPropertyChanged();
                }
            }
        }

        private int numarInterventii;
        public int NumarInterventii
        {
            get { return numarInterventii; }
            set
            {
                if (numarInterventii != value)
                {
                    numarInterventii = value;
                    OnPropertyChanged();
                }
            }
        }

        private double totalProfit;
        public double TotalProfit
        {
            get { return totalProfit; }
            set
            {
                if (value == totalProfit)
                    return;
                totalProfit = value;
                OnPropertyChanged();
            }
        }


        private double totalIncasari;
        public double TotalIncasari
        {
            get { return totalIncasari; }
            set
            {
                if (totalIncasari != value)
                {
                    totalIncasari = value;
                    OnPropertyChanged();
                }
            }
        }

        private TimeSpan totalHours;

        public TimeSpan TotalHours
        {
            get { return totalHours; }
            set
            {
                if (value == totalHours)
                    return;
                totalHours = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public override void OnClick(bool shouldSelect)
        {
            if (shouldSelect)
            {
                ContentVisibility=Visibility.Visible;
                ContentBackground = BackgroundColors.YearTileColorMouseOver;
                ExpanderRotation = "90";
            }
            else
            {
                ContentVisibility=Visibility.Collapsed;
                ContentBackground = BackgroundColors.YearTileColor;
                ExpanderRotation = "270";
                foreach (var monthInterventionsViewModel in MonthList.List)
                {
                    if (monthInterventionsViewModel.IsSelected)
                    {
                        monthInterventionsViewModel.OnSelected(monthInterventionsViewModel);
                    }
                }
            }
            IsSelected = shouldSelect;
        }

        public void MouseOn()
        {
            if (!IsSelected)
            {
                ContentBackground = BackgroundColors.YearTileColorMouseOver;
            }
        }

        public void MouseOff()
        {
            if (!IsSelected)
            {
                ContentBackground = BackgroundColors.YearTileColor;
            }
        }

        public void PopulateMonthsInterventions(Dictionary<int, List<LocalIntervention>> monthsWithInterventions)
        {
            MonthList=new ObjectList<MonthInterventionsViewModel>(true);
            foreach (var monthKey in monthsWithInterventions.Keys)
            {
                MonthList.Add(new MonthInterventionsViewModel(this,monthsWithInterventions[monthKey],monthKey));
                //set Numar interventii +total incasari
                NumarInterventii += monthsWithInterventions[monthKey].Count;
                foreach (var inter in monthsWithInterventions[monthKey])
                {
                    TotalIncasari += inter.Revenue;
                    TotalHours += inter.DateHourDetail.Duration;
                    TotalProfit += inter.Percent;
                    
                }
            }           
            MonthList.List=new ObservableCollection<MonthInterventionsViewModel>(MonthList.List.OrderBy(item=>item.MonthId));
        }

        public void FIlter()
        {
            foreach (var mont in MonthList.List)
            {
                mont.FilterInterventions();
            }
        }

        public void AddInterventionToMonth(InterventionDetails interventionDetails)
        {
            int month = interventionDetails.Date.Month;
           var monthToAddTo = MonthList.List.FirstOrDefault(item => item.MonthId == month);
            if (monthToAddTo != null)
            {
                monthToAddTo.AddIntervention(interventionDetails);   
            }
            else
            {
                monthToAddTo=new MonthInterventionsViewModel(this,new List<LocalIntervention>(),month);               
                MonthList.Add(monthToAddTo);
                MonthList.List = new ObservableCollection<MonthInterventionsViewModel>(MonthList.List.OrderBy(item => item.MonthId).ToList());
                monthToAddTo.AddIntervention(interventionDetails); 
            }
            if (interventionDetails.NewlyAdded && !IsSelected)
            {                
                this.OnSelected(this);
//                ContentVisibility = Visibility.Visible;
//                ContentBackground = BackgroundColors.YearTileColorMouseOver;
//                ExpanderRotation = "90";
//                IsSelected = true;
            }
            NumarInterventii++;
            TotalIncasari += interventionDetails.Revenue;
            TotalProfit += interventionDetails.Percent;
        }

        public void removeInterventionFromMonth(InterventionDetails interventionDetails)
        {
            int month = interventionDetails.Date.Month;
            var monthToToRemoveFrom = MonthList.List.FirstOrDefault(item => item.MonthId == month);
            monthToToRemoveFrom.RemoveIntervention(interventionDetails);
        }

        public List<InterventionDetails> SetPaymentInterval(DateTime startDate, DateTime endDate,bool toEnd,bool payed,long startMili,long endMili)
        {
//            List<int> interventionsIds=new List<int>();
            List<InterventionDetails> interventions=new List<InterventionDetails>();
            var monthsInInterval =
                MonthList.List.Where(item => item.MonthId >= startDate.Month && (item.MonthId <= endDate.Month || toEnd));
            foreach (var monthInterventionsViewModel in monthsInInterval)
            {
                foreach (var interventionDetailse in monthInterventionsViewModel.Interventions)
                {
                    if ((interventionDetailse.Date >= startDate && interventionDetailse.Mili >= startMili) && (interventionDetailse.Date <= endDate && interventionDetailse.Mili <= endMili))
                    {
                        interventionDetailse.WasPayedByDental = payed;
                        interventions.Add(interventionDetailse);
                      //  interventionsIds.Add(interventionDetailse.Id);
                        interventionDetailse.IncludedInLastPaymentInterval = true;
                    }

                }
            }
            return interventions;
        }

        public void ChangePaymentStatus(int monthId,bool payed)
        {
            var monthModel = MonthList.List.FirstOrDefault(item => item.MonthId == monthId);
            monthModel.ChangePaymentStatus(payed);
        }

        #endregion Methods

    }
}
