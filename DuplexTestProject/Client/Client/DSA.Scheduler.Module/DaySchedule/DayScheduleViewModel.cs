using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Infrastructure.ObjectList;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Scheduler.Module.Appointments.AppointmentItem;
using DSA.Scheduler.Module.DaySchedule.DayHeader;
using DSA.Scheduler.Module.Helpers;
using DSA.Scheduler.Module.WeekSchedule;


namespace DSA.Scheduler.Module.DaySchedule
{
    public class DayScheduleViewModel:ViewModelBase
    {
        #region Constructor

        public DayScheduleViewModel(WeekScheduleViewModel parent,DateTime date)
        {

            Parent = parent;
            PopulateAppointmets(8,22,date);
        }
        #endregion Constructor

        #region Properties

        public WeekScheduleViewModel Parent { get; set; }

        private Brush contentBackground;
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

        private string dayOfWeek;
        public string DayOfWeek
        {
            get { return dayOfWeek; }
            set
            {
                if (dayOfWeek != value)
                {
                    dayOfWeek = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObjectList<AppointmentViewModel> appointmetList = new ObjectList<AppointmentViewModel>(true);
        public ObjectList<AppointmentViewModel> AppointmetList
        {
            get { return appointmetList; }
            set
            {
                if (appointmetList != value)
                {
                    appointmetList = value;
                    OnPropertyChanged();
                }
            }
        }

        private DayHeaderViewModel dayHeaderViewModel;
        public DayHeaderViewModel DayHeaderViewModel
        {
            get { return dayHeaderViewModel; }
            set
            {
                if (dayHeaderViewModel != value)
                {
                    dayHeaderViewModel = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion Properties

        #region Methods

        private void PopulateAppointmets(int startingHour,int finishHour,DateTime date)
        {
           date = date.AddHours(startingHour);
            int interval = finishHour - startingHour;
            for (int i = 0; i < interval; i++)
            {
                AppointmetList.Add(new AppointmentViewModel(this)
                {
                    Date = date,

                });
                date = date.AddHours(1);
            }           
        }

        public void SetAppointMents(List<LocalAppointment> appointments)
        {
            try
            {
                foreach (var appointmentViewModel in AppointmetList.List)
                {
                    foreach (var localAppointment in appointments)
                    {
                        if (appointmentViewModel.HourId.Id == localAppointment.DateHour.Hour)
                        {
                            appointmentViewModel.PatientName =
                                LocalCache.Instance.PatientsRepository.PatientsDictionary[localAppointment.PatientId]
                                    .Surname;
                            appointmentViewModel.OnSelected(appointmentViewModel);
                        }
                    }
                    appointmentViewModel.Name = DayHeaderViewModel.Day;
                }    
            }
            catch (Exception)
            {

                int p = 10;
            }        
        }

        #endregion Methods

    }
}