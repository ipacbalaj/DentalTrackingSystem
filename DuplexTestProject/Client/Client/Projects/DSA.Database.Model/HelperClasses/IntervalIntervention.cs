using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model.Entities;

namespace DSA.Database.Model.HelperClasses
{
    public class IntervalIntervention : ViewModelBase
    {

        public Boolean IsShiftHold { get; set; }



        private InterventionDetails startInterventionDetails;

        public InterventionDetails StarInterventionDetails
        {
            get { return startInterventionDetails; }
            set
            {
                if (value == startInterventionDetails)
                    return;
                startInterventionDetails = value;
                OnPropertyChanged();
            }
        }

        private InterventionDetails endInterventionDetails;

        public InterventionDetails EndInterventionDetails
        {
            get { return endInterventionDetails; }
            set
            {
                if (value == endInterventionDetails)
                    return;
                endInterventionDetails = value;
                OnPropertyChanged();
            }
        }

        public bool SetIntervention(InterventionDetails intervention)
        {
            if (IsShiftHold && intervention != null)
            {
                if (StarInterventionDetails != null && EndInterventionDetails != null)
                {
                    StarInterventionDetails = intervention;
                    EndInterventionDetails = null;
                    return false;
                }
                if (StarInterventionDetails == null)
                {
                    StarInterventionDetails = intervention;
                    return false;
                }
                if (EndInterventionDetails == null)
                {
                    EndInterventionDetails = intervention;
                    return true;
                }
                //                StarInterventionDetails = intervention;
                //                EndInterventionDetails = null;
                return false;
            }
            return false;
        }
    }

}
