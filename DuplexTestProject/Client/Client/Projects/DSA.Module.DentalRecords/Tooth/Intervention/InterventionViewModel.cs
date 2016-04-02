using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Common.Controls.Buttons;
using DSA.Common.Infrastructure;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Module.DentalRecords.Tooth.ToothDetails;
using Microsoft.Practices.Prism.Commands;

namespace DSA.Module.DentalRecords.Tooth.Intervention
{
    public class InterventionViewModel:ViewModelBase
    {
        #region Constructor

        public InterventionViewModel(ToothDetailsViewModel parent)
        {
            Parent = parent;
            SaveButton = new ActionButtonViewModel("Save", new DelegateCommand(OnSave), ImagePath.SaveIconPath);
        }
        #endregion Constructor

        #region Properties

        public ToothDetailsViewModel Parent { get; set; }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged();
                }
            }
        }

        private string apointment;
        public string Appointment
        {
            get { return apointment; }
            set
            {
                if (apointment != value)
                {
                    apointment = value;
                    OnPropertyChanged();
                }
            }
        }


        private ActionButtonViewModel saveButton;
        public ActionButtonViewModel SaveButton
        {
            get { return saveButton; }
            set
            {
                if (saveButton == value)
                    return;
                saveButton = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        private void OnSave()
        {
            
        }
        #endregion Methods

    }
}
