using System;
using System.Windows.Media;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;

namespace DSA.Database.Model.Entities
{
    public class InterventionDetails : ViewModelBase
    {
        #region Constructor

        public InterventionDetails()
        {
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        }

        #endregion Constructor

        #region Properties

        private readonly IEventAggregator eventAggregator;

        private string pacientName;
        public string PacientName
        {
            get { return pacientName; }
            set
            {
                if (pacientName != value)
                {
                    pacientName = value;
                    OnPropertyChanged();
                }
            }
        }

        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (value == id)
                    return;
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private Brush brush = BackgroundColors.GridInterventionRowColor;

        public Brush Brush
        {
            get { return brush; }
            set
            {
                if (value == brush)
                    return;
                brush = value;
                OnPropertyChanged();
            }
        }

        private bool wasPayedByDental;
        public bool WasPayedByDental
        {
            get { return wasPayedByDental; }
            set
            {
                Brush = value ? BackgroundColors.GridInterventionRowColor : BackgroundColors.GridNotPayedColor;
                wasPayedByDental = value;
                OnPropertyChanged();
            }
        }

        private bool newlyAdded;
        public bool NewlyAdded
        {
            get { return newlyAdded; }
            set
            {
                if (value == newlyAdded)
                    return;
                newlyAdded = value;
                OnPropertyChanged();
            }
        }

        private string location;
        public string Location
        {
            get { return location; }
            set
            {
                if (location != value)
                {
                    location = value;
                    OnPropertyChanged();
                }
            }
        }

        private double revenue;
        public double Revenue
        {
            get { return revenue; }
            set
            {
                if (revenue != value)
                {
                    revenue = value;
                    OnPropertyChanged();
                }
            }
        }

        private string datestring;
        public string DateString
        {
            get { return datestring; }
            set
            {
                if (datestring != value)
                {
                    datestring = value;
                    OnPropertyChanged("DateString");
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
                    OnPropertyChanged();
                }
            }
        }

        private long mili;
        public long Mili
        {
            get { return mili; }
            set
            {
                if (value == mili)
                    return;
                mili = value;
                OnPropertyChanged();
            }
        }

        private string work;
        public string Work
        {
            get { return work; }
            set
            {
                if (work != value)
                {
                    work = value;
                    WorkAndType = work;
                    OnPropertyChanged();
                }
            }
        }

        private string workType;
        public string WorkType
        {
            get { return workType; }
            set
            {
                if (value == workType)
                    return;
                workType = value;
                WorkAndType = !string.IsNullOrEmpty(workType) ? Work + "(" + workType + ")" : Work;
                OnPropertyChanged();
            }
        }

        private string workAndType;
        public string WorkAndType
        {
            get { return workAndType; }
            set
            {
                if (value == workAndType)
                    return;
                workAndType = value;
                OnPropertyChanged();
            }
        }

        private string material;
        public string Material
        {
            get { return material; }
            set
            {
                if (material != value)
                {
                    material = value;
                    MaterialAndType = material;
                    OnPropertyChanged();
                }
            }
        }

        private string materialType;
        public string MaterialType
        {
            get { return materialType; }
            set
            {
                if (value == materialType)
                    return;
                materialType = value;
                MaterialAndType = !string.IsNullOrEmpty(materialType) ? Material + "(" + materialType + ")" : material;
                OnPropertyChanged();
            }
        }

        private string materialAndType;
        public string MaterialAndType
        {
            get { return materialAndType; }
            set
            {
                if (value == materialAndType)
                    return;
                materialAndType = value;
                OnPropertyChanged();
            }
        }

        private string area;
        public string Area
        {
            get { return area; }
            set
            {
                if (area != value)
                {
                    area = value;
                    OnPropertyChanged();
                }
            }
        }

        private string observations;
        public string Observations
        {
            get { return observations; }
            set
            {
                if (observations != value)
                {
                    observations = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime startH;
        public DateTime StartH
        {
            get { return startH; }
            set
            {
                if (startH != value)
                {
                    startH = value;
                    OnPropertyChanged();
                }
            }
        }

        private LocalIntervention localIntervention;
        public LocalIntervention LocalIntervention
        {
            get { return localIntervention; }
            set
            {
                if (value == localIntervention)
                    return;
                localIntervention = value;
                OnPropertyChanged();
            }
        }

        private DateTime endH;
        public DateTime EndH
        {
            get { return endH; }
            set
            {
                if (endH != value)
                {
                    endH = value;
                    OnPropertyChanged();
                }
            }
        }

        private TimeSpan durata;
        public TimeSpan Durata
        {
            get { return durata; }
            set
            {
                if (durata != value)
                {
                    durata = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal incasari;
        public decimal Incasari
        {
            get { return incasari; }
            set
            {
                if (incasari != value)
                {
                    incasari = value;
                    OnPropertyChanged();
                }
            }
        }

        private double percent;
        public double Percent
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

        private double? materialCost;
        public double? MaterialCost
        {
            get { return materialCost; }
            set
            {
                if (value == materialCost)
                    return;
                materialCost = value;
                OnPropertyChanged();
            }
        }

        private bool includedInLastPaymentInterval;
        public bool IncludedInLastPaymentInterval
        {
            get { return includedInLastPaymentInterval; }
            set
            {
                if (value == includedInLastPaymentInterval)
                    return;
                includedInLastPaymentInterval = value;
                if (value)
                    Brush = BackgroundColors.InterventionCurrentInterval;
                else
                {
                    Brush = BackgroundColors.GridInterventionRowColor;
                }
                OnPropertyChanged();
            }
        }

        private int? technicianId;
        public int? TechnicianId
        {
            get { return technicianId; }
            set
            {
                if (value == technicianId)
                    return;
                technicianId = value;
                OnPropertyChanged();
            }
        }

        private string technicianName;
        public string TechnicianName
        {
            get { return technicianName; }
            set
            {
                if (value == technicianName)
                    return;
                technicianName = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods



        #endregion Methods

    }
}
