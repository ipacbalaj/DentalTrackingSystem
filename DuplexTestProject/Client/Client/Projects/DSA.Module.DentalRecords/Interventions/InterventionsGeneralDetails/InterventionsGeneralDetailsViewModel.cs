using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Common.Controls.Buttons;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Module.DentalRecords.Interventions.InterventionsGeneralDetails.BirthDays;
using Microsoft.Practices.Prism.Commands;

namespace DSA.Module.DentalRecords.Interventions.InterventionsGeneralDetails
{
    public class InterventionsGeneralDetailsViewModel:ViewModelBase
    {
        #region Constructor

        public InterventionsGeneralDetailsViewModel()
        {
           // ShowBirthdayButton = new ActionButtonViewModel("", new DelegateCommand(OnShowBD), Common.Infrastructure.ImagePath.Present);
            BirthDaysViewModel = new BirthDaysViewModel(Common.Infrastructure.ImagePath.Present);
        }

        #endregion Constructor

        #region Properties

        private int totalPatients=0;

        public int TotalPatients
        {
            get { return totalPatients; }
            set
            {
                if (value == totalPatients)
                    return;
                totalPatients = value;
                OnPropertyChanged();
            }
        }

        private decimal nbH;

        public decimal NbH
        {
            get { return nbH; }
            set
            {
                if (value == nbH)
                    return;
                nbH = value;
                OnPropertyChanged();
            }
        }

        private int nbMinutes;

        public int NBMinutes
        {
            get { return nbMinutes; }
            set
            {
                if (value == nbMinutes)
                    return;
                nbMinutes = value;
                OnPropertyChanged();
            }
        }

        private long totalInverventions=0;
        public long TotalInverventions
        {
            get { return totalInverventions; }
            set
            {
                if (value == totalInverventions)
                    return;
                totalInverventions = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel showBirthdayButton;

        public ActionButtonViewModel ShowBirthdayButton
        {
            get { return showBirthdayButton; }
            set
            {
                if (value == showBirthdayButton)
                    return;
                showBirthdayButton = value;
                OnPropertyChanged();
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
                NbH = Decimal.Truncate((decimal)totalHours.TotalHours);
                NBMinutes = totalHours.Minutes;
                OnPropertyChanged();
            }
        }

        private BirthDaysViewModel birthDaysViewModel;

        public BirthDaysViewModel BirthDaysViewModel
        {
            get { return birthDaysViewModel; }
            set
            {
                if (value == birthDaysViewModel)
                    return;
                birthDaysViewModel = value;
                OnPropertyChanged();
            }
        }

        private double totalRevenue=0;

        public double TotalRevenue
        {
            get { return totalRevenue; }
            set
            {
                if (value == totalRevenue)
                    return;
                totalRevenue = value;
                OnPropertyChanged();
            }
        }

        private double totalProfit=0;
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

        private string imagePath;
        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                if (value == imagePath)
                    return;
                imagePath = value;
                OnPropertyChanged();
            }
        }

        private int nbOfB;

        public int NbOfB
        {
            get { return nbOfB; }
            set
            {
                if (value == nbOfB)
                    return;
                nbOfB = value;
                OnPropertyChanged();
            }
        }

        private string names;

        public string Names
        {
            get { return names; }
            set
            {
                if (value == names)
                    return;
                names = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        private void OnShowBD()
        {
            
        }

        public void InitNames()
        {
            List<string> patientNames =new List<string>();
            foreach (var localPatient in LocalCache.Instance.PatientsRepository.BirthDays)
            {
                patientNames.Add(localPatient.AllName);

            }
            BirthDaysViewModel.PatientNames = patientNames;
        }

        #endregion Methods
       
    }
}
