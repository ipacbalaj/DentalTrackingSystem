using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Module.DentalRecords.Tooth.Intervention;

namespace DSA.Module.DentalRecords.Tooth.ToothDetails
{
    public class ToothDetailsViewModel:ViewModelBase
    {
        #region Constructor

        #endregion Constructor

        #region Properties
        public ToothDetailsViewModel Parent { get; set; }

        private ObservableCollection<InterventionViewModel> intervenrions;
        public ObservableCollection<InterventionViewModel> Intervenrions
        {
            get { return intervenrions; }
            set
            {
                if (intervenrions != value)
                {
                    intervenrions = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion Properties

        #region Methods

        public void AddIntervention(InterventionViewModel addedIntervenion)
        {
            Intervenrions.Add(addedIntervenion);
        }

        #endregion Methods

    }
}
