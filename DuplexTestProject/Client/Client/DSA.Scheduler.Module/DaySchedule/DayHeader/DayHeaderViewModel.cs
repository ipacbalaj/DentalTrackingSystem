using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Common.Infrastructure.ViewModel;

namespace DSA.Scheduler.Module.DaySchedule.DayHeader
{
    public class DayHeaderViewModel : BindableObjectListBase<DayHeaderViewModel>
    {
        #region Constructor

        #endregion Constructor

        #region Properties

        private string day;
        public string Day
        {
            get { return day; }
            set
            {
                if (day != value)
                {
                    day = value;
                    OnPropertyChanged();
                }
            }
        }

        private string date;
        public string Date
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
        #endregion Properties

        #region Methods
        public override void OnClick(bool shouldSelect)
        {
            throw new NotImplementedException();
        }
        #endregion Methods
    }
}
