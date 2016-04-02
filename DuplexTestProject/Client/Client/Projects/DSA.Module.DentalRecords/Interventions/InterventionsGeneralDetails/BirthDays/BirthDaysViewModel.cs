using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;

namespace DSA.Module.DentalRecords.Interventions.InterventionsGeneralDetails.BirthDays
{
    public class BirthDaysViewModel:ViewModelBase
    {
        #region Constructor

        public BirthDaysViewModel(string imagePath)
        {
            ImagePath = imagePath;
        }

        #endregion Constructor

        #region Properties

        private List<string> patientName;

        public List<string> PatientNames
        {
            get { return patientName; }
            set
            {
                if (value == patientName)
                    return;
                patientName = value;
                OnPropertyChanged();
            }
        }

        private Brush textForeground=BackgroundColors.White;

        public Brush TextForeground
        {
            get { return textForeground; }
            set
            {
                if (value == textForeground)
                    return;
                textForeground = value;
                OnPropertyChanged();
            }
        }

        private bool isPopupOpened;

        public bool PopupOpened
        {
            get { return isPopupOpened; }
            set
            {
                if (value == isPopupOpened)
                    return;
                isPopupOpened = value;
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

        #endregion Properties


        #region Methods

        public void MouseEnter()
        {
            PopupOpened = true;
            TextForeground = BackgroundColors.blue;
        }

        public void MouseLeave()
        {
            TextForeground = BackgroundColors.White;
            PopupOpened = false;
        }

        #endregion Methods
    }
}
