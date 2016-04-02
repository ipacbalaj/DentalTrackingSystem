using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;

namespace DSA.Database.Model.HelperClasses
{
    public class PaymentInterval
    {
        public PaymentInterval()
        {
            InitData();
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        }
        private readonly IEventAggregator eventAggregator;
        public int StartInterventionId { get; set; }
        public int EndInterventionId { get; set; }
        public DateTime StartDate { get; set; }
        public long StartMili { get; set; }
        public DateTime EndDate { get; set; }
        public long EndMili { get; set; }
        public bool BothValuesSet { get; set; }

        public void SetValues(int id, DateTime date,long mili)
        {
            if (StartInterventionId == 0)
            {
                StartInterventionId = id;
                StartDate = date;
                StartMili = mili;
            }
            else
            {
                EndInterventionId = id;
                EndDate = date;
                EndMili = mili;
                BothValuesSet = true;
            }                               
        }

        public void InitData()
        {

            StartInterventionId = 0;
            EndInterventionId = 0;
            StartDate = DateTime.MinValue;
            StartDate = DateTime.MinValue;
            BothValuesSet = false;
        }

        private bool shouldSetInterval;

        public bool ShouldSetInterval
        {
            get { return shouldSetInterval; }
            set
            {
                if (!value)
                {
                }
                shouldSetInterval = value;         
            }
        }
    }
}
