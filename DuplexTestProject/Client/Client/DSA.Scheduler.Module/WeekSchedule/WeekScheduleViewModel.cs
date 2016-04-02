using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Scheduler.Module.DaySchedule;
using DSA.Scheduler.Module.DaySchedule.DayHeader;

namespace DSA.Scheduler.Module.WeekSchedule
{
    public class WeekScheduleViewModel : BindableObjectListBase<WeekScheduleViewModel>
    {
        #region Constructor
        public WeekScheduleViewModel(DateTime date)
        {
            PopulateHours(8,22);
            PopulateDays(date);
            weekDayDate = date;

        }
        #endregion Constructor

        #region Properties
        private ObservableCollection<DayScheduleViewModel> weekDays = new ObservableCollection<DayScheduleViewModel>();
        public ObservableCollection<DayScheduleViewModel> WeekDays
        {
            get { return weekDays; }
            set
            {
                if (weekDays != value)
                {
                    weekDays = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<String> hours;
        public List<String> Hours
        {
            get { return hours; }
            set
            {
                if (hours != value)
                {
                    hours = value;
                    OnPropertyChanged();
                }
            }
        }

        private int weekNumber;
        public int WeekNumber
        {
            get { return weekNumber; }
            set
            {
                if (weekNumber != value)
                {
                    weekNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime weekDayDate;
        public DateTime WeekDayDate
        {
            get { return weekDayDate; }
            set
            {
                if (weekDayDate != value)
                {
                    weekDayDate = value;
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

        private String firstDayDate;
        public String FirstDayDate
        {
            get { return firstDayDate; }
            set
            {
                if (firstDayDate != value)
                {
                    firstDayDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private String lastDayDate;
        public String LastDayDate
        {
            get { return lastDayDate; }
            set
            {
                if (lastDayDate != value)
                {
                    lastDayDate = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion Properties

        #region Methods

        private void PopulateDays(DateTime date)
        {
            SetCurrentWeekAndYearNumber(date);
            DateTime dt = date.StartOfWeek(DayOfWeek.Monday);
            FirstDayDate = dt.ToShortDateString();
            var columnHour = new DayScheduleViewModel(this,dt)
            {
                DayHeaderViewModel = new DayHeaderViewModel()
                {
                    Day = "Ora",                    
                },
                ContentBackground = BackgroundColors.gray,                               
            };
            foreach (var appointmentViewModel in columnHour.AppointmetList.List)
            {
                appointmentViewModel.DayVisibility=Visibility.Collapsed;
                appointmentViewModel.PatientName = appointmentViewModel.HourId.Hour;
            }
            WeekDays.Add(columnHour);
            for (int i = 0; i < 7; i++)
            {
               WeekDays.Add(new DayScheduleViewModel(this,dt)
               {
                   DayOfWeek = dt.DayOfWeek.ToString(),
                   DayHeaderViewModel = new DayHeaderViewModel()
                   {
                       Date = dt.Date.ToShortDateString(),
                       Day = dt.DayOfWeek.ToString(),                       
                   }
               });
              dt= dt.AddDays(1);
            }
            LastDayDate = dt.AddDays(-1).ToShortDateString();
        }

        private void SetCurrentWeekAndYearNumber(DateTime date)
        {
            Year = date.Year;
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;         
            Calendar cal = dfi.Calendar;
            
            WeekNumber = cal.GetWeekOfYear(date,dfi.CalendarWeekRule,dfi.FirstDayOfWeek);
            
        }
        private void PopulateHours(int startH, int endH)
        {
            Hours=new List<string>();
            for (int i = startH; i <= endH; i++)
            {
                hours.Add(i+":00 ");
            }
        }

        public override void OnClick(bool shouldSelect)
        {
            IsSelected = shouldSelect;
            if (IsSelected)
            {
               // ContentBackground = BackgroundColors.green;
                //Command.Execute(null);
            }
            else
            {
                //ContentBackground = BackgroundColors.lightblue;
            }  
        }

        private void SetDays(List<LocalDay> days )
        {
//            foreach (var localDay in days)
//            {
//                WeekDays.FirstOrDefault(item => item.DayOfWeek == localDay.Name).SetAppointMents(localDay.Appointments);
//
//            }
        }

        public void SetWeekViewModelAppointments()
        {
            var weekNumber = this.WeekNumber;
            var currentWeekDays =LocalCache.Instance.WeekRepository.Weeks.ContainsKey(weekNumber)? LocalCache.Instance.WeekRepository.Weeks[weekNumber].Days:null;
            if (currentWeekDays != null)
                this.SetDays(currentWeekDays);

        }
        #endregion Methods

    }
}
