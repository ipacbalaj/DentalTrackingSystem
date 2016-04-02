using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Scheduler.Module.DaySchedule;
using DSA.Scheduler.Module.DaySchedule.DayHeader;
using DSA.Scheduler.Module.Helpers;

namespace DSA.Scheduler.Module.Appointments.AppointmentItem
{
    public class AppointmentViewModel : BindableObjectListBase<AppointmentViewModel>
    {
        #region Constructor

        public AppointmentViewModel(DayScheduleViewModel parent)
        {
            Parent = parent;
        }
        #endregion Constructor

        #region Properties
        public DayScheduleViewModel Parent { get; set; }

        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        private HourId hourId;
        public HourId HourId
        {
            get { return hourId; }
            set
            {
                if (hourId != value)
                {
                    hourId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string patientName;
        public string PatientName
        {
            get { return patientName; }
            set
            {
                if (patientName != value)
                {
                    patientName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

//        private string hour;
//        public string Hour
//        {
//            get { return hour; }
//            set
//            {
//                if (hour != value)
//                {
//                    hour = value;
//                    OnPropertyChanged();
//                }
//            }
//        }

        private Visibility hoverVisibility=Visibility.Collapsed;
        public Visibility HoverVisibility
        {
            get { return hoverVisibility; }
            set
            {
                if (hoverVisibility != value)
                {
                    hoverVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private Brush contentBackground=BackgroundColors.lightblue;
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

        private Visibility dayVisibility=Visibility.Visible;
        public Visibility DayVisibility
        {
            get { return dayVisibility; }
            set
            {
                if (dayVisibility != value)
                {
                    dayVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (date != value)
                {
                    date = value;
                    HourId=new HourId()
                    {
                        Id = date.Hour,
                        Hour = LocalCache.Instance.WeekRepository.Hours[date.Hour]
                    };
                    OnPropertyChanged();
                }
            }
        }

        #endregion Properties

        #region Methods
        public override void OnClick(bool shouldSelect)
        {
            IsSelected = shouldSelect;
            if (IsSelected)
            {
                ContentBackground = BackgroundColors.green;
                AddAppointment();
                try
                {
                    PatientName = LocalCache.Instance.SelectedPatient.Name;
                }
                catch (Exception)
                {

                    int p = 10;
                }
                //LocalCache.Instance.SelectedPatient != null ? (LocalCache.Instance.SelectedPatient.Surname + " " + LocalCache.Instance.SelectedPatient.Surname ): PatientName;
                //Command.Execute(null);
            }
            else
            {
                ContentBackground = BackgroundColors.lightblue;
            }      
        }

        public void MouseEnter()
        {
            if (!IsSelected)
            {
                ContentBackground = BackgroundColors.gray;
            }
            HoverVisibility=Visibility.Visible;
        }

        public void MouserLeave()
        {
            if (!IsSelected)
            {
                ContentBackground = BackgroundColors.lightblue;
            }
             HoverVisibility=Visibility.Collapsed;
        
        }

        public async void AddAppointment()
        {
            if (LocalCache.Instance.SelectedPatient != null)
            {
                var addedAppointment = new LocalAppointment()
                {
                    DateHour = Date,
                    PatientId = LocalCache.Instance.SelectedPatient.Id,
                    WeekNumber = Parent.Parent.WeekNumber,
                    Year = Parent.Parent.Year
                };
                Id = await LocalCache.Instance.ServiceHandler.AddAppointment(addedAppointment);
            }
        }
        #endregion Methods

    }
}
