using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Subscription.Service.DuplexService;

namespace DSA.Subscription.Service.Repositories
{
    public class WeekRepository
    {
        #region Constructor

        public WeekRepository()
        {
            PopulateHours();
        }
        #endregion Constructor

        #region Properties

        public Dictionary<int, LocalWeek> Weeks { get; set; }
        public Dictionary<float, String> Hours { get; set; } 

        #endregion Properties

        #region Methods

        public  void PopulateWeeks(List<LocalWeek> weeks)
        {
            Weeks=new Dictionary<int, LocalWeek>();
//                new Task(delegate
//                {
                    foreach (var week in weeks)
                    {
                        Weeks.Add(week.WeekNumber, week);
                        //todo:differetiate years
                    }
               // }).Start();          
        }

        private void PopulateHours()
        {
            Hours=new Dictionary<float, string>();
                for (int i = 0; i < 24; i++)
            {
                Hours.Add(i,i+":00");
            }
        }

        #endregion Methods

    }
}
