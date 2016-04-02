using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.Entities;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace DSA.Reports.ReportsScreen
{
    public class ReportsScreenViewModel:ViewModelBase
    {
        #region Constructor

        public ReportsScreenViewModel()
        {
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            unityContainer = ServiceLocator.Current.GetInstance<IUnityContainer>();
            eventAggregator.GetEvent<UpdateReportsEvent>().Subscribe(UpdateRecordsScreen);   
        }

        #endregion Constructor

        #region Properties

        private readonly IEventAggregator eventAggregator;
        private readonly IUnityContainer unityContainer;

        private ObservableCollection<InterventionDetails> reportsInterventions;
        public ObservableCollection<InterventionDetails> ReportsInterventions
        {
            get { return reportsInterventions; }
            set
            {
                if (reportsInterventions != value)
                {
                    reportsInterventions = value;
                    OnPropertyChanged();
                }
            }
        }

        private Brush contentBackground ;

        public Brush ContentBackground
        {
            get { return contentBackground; }
            set
            {
                if (value == contentBackground)
                    return;
                contentBackground = value;
                OnPropertyChanged();
            }
        }

        private Brush tableBack ;

        public Brush TtableBack
        {
            get { return tableBack; }
            set
            {
                if (value == tableBack)
                    return;
                tableBack = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        private void UpdateRecordsScreen(Object anObj)
        {
            ReportsInterventions = new ObservableCollection<InterventionDetails>(LocalCache.Instance.FilteredInterventionDetails);
        }

        #endregion Methods

    }
}
