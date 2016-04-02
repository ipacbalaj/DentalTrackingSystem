using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using DSA.Common.Controls.Pagination;
using DSA.Common.Infrastructure.ObjectList;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Scheduler.Module.WeekSchedule;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace DSA.Scheduler.Module.SchedulerScreen
{
    public class SchedulerScreenViewModel:ViewModelBase
    {
        #region Constructor
        public SchedulerScreenViewModel()
        {
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            unityContainer = ServiceLocator.Current.GetInstance<IUnityContainer>();
            eventAggregator.GetEvent<UpdateSchedulerEvent>().Subscribe(UpdateSchedulerScreen);
            eventAggregator.GetEvent<DataLoadedEvent>().Subscribe(InitControls);
            NextCommand=new DelegateCommand(OnNext);
            PrevCommand=new DelegateCommand(OnPrev);
        }
        #endregion Constructor

        #region Properties

        private readonly IEventAggregator eventAggregator;
        private readonly IUnityContainer unityContainer;

        private WeekScheduleViewModel activeWeek;
        public WeekScheduleViewModel ActiveWeek
        {
            get { return activeWeek; }
            set
            {
                if (activeWeek != value)
                {
                    activeWeek = value;
                    OnPropertyChanged();
                }
            }
        }

        public Dictionary<int, WeekScheduleViewModel> WeekViewModels;            

        private int currentWeekNumber;
        public int CurrentWeekNumber
        {
            get { return currentWeekNumber; }
            set
            {
                if (currentWeekNumber != value)
                {
                    currentWeekNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand NextCommand { get; set; }
        public ICommand PrevCommand { get; set; }

        #endregion Properties

        #region Methods

        private void UpdateSchedulerScreen(object anObj)
        {
            
        }

        private void InitWeekList()
        {
            WeekViewModels = new Dictionary<int, WeekScheduleViewModel>();
            var week = new WeekScheduleViewModel(DateTime.Now);
            week.SetWeekViewModelAppointments();
            ActiveWeek = week;
            WeekViewModels.Add(ActiveWeek.WeekNumber,ActiveWeek);
        }

        public void InitControls(Object anObj)
        {  
            InitWeekList();        
        }

        public void OnNext()
        {
            if (WeekViewModels.ContainsKey(ActiveWeek.WeekNumber + 1))
            {
                ActiveWeek = WeekViewModels[ActiveWeek.WeekNumber + 1];
            }
            else
            {
                var newDate = ActiveWeek.WeekDayDate.AddDays(7);
                var newWeek = new WeekScheduleViewModel(newDate);
                newWeek.SetWeekViewModelAppointments();
                ActiveWeek = newWeek;
                WeekViewModels.Add(ActiveWeek.WeekNumber, ActiveWeek);
            }
        }

        public void OnPrev()
        {
            if (WeekViewModels.ContainsKey(ActiveWeek.WeekNumber - 1))
            {
                ActiveWeek = WeekViewModels[ActiveWeek.WeekNumber - 1];
            }
            else
            {
                var newDate = ActiveWeek.WeekDayDate.AddDays(-7);
                var newWeek = new WeekScheduleViewModel(newDate);
                newWeek.SetWeekViewModelAppointments();
                ActiveWeek = newWeek;
                WeekViewModels.Add(ActiveWeek.WeekNumber, ActiveWeek);
            }
        }
        #endregion Methods

    }
}
