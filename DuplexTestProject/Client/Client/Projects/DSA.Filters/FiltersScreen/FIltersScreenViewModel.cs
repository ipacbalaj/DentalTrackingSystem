using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using DSA.Common.Controls.Loading.MetroLoading;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.Entities;
using DSA.Filters.FilterGroup;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace DSA.Filters.FiltersScreen
{
    public class FIltersScreenViewModel:ViewModelBase
    {
        #region Constructor

        public FIltersScreenViewModel()
        {
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            unityContainer = ServiceLocator.Current.GetInstance<IUnityContainer>();
            eventAggregator.GetEvent<UpdateFiltersEvent>().Subscribe(UpdateFiltersScreen);               
        }


        #endregion Constructor

        #region Properties

        public FiltersScreenView ViewReference { get; set; }

        private bool initialized;

        private readonly IEventAggregator eventAggregator;
        private readonly IUnityContainer unityContainer;

        private FilterGroupViewModel filterGroupViewModel;
        public FilterGroupViewModel FilterGroupViewModel
        {
            get { return filterGroupViewModel; }
            set
            {
                if (filterGroupViewModel != value)
                {
                    filterGroupViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        private MetroLoadingViewModel metroLoadingViewModel;

        public MetroLoadingViewModel MetroLoadingViewModel
        {
            get { return metroLoadingViewModel; }
            set
            {
                if (value == metroLoadingViewModel)
                    return;
                metroLoadingViewModel = value;
                OnPropertyChanged();
            }
        }

        private string groupName;
        public string GroupName
        {
            get { return groupName; }
            set
            {
                if (groupName != value)
                {
                    groupName = value;
                    OnPropertyChanged();
                }
            }
        }

        private Brush contentBackground =BackgroundColors.FiltersColor;

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

        #endregion Properties

        #region Methods

        private void UpdateFiltersScreen(Object anObj)
        {
            if (!initialized)
            {
                
                initialized = true;
            }
            else
            {
               // FilterGroupViewModel.PopulateFilterGroups();
            }
            FilterGroupViewModel = new FilterGroupViewModel(this);
            
        }

        public void FilterInterventions()
        {
            ReportsInterventions = new ObservableCollection<InterventionDetails>(LocalCache.Instance.FilteredInterventionDetails);
            ViewReference.BestFitColumns();
        }

        #endregion Methods

    }
}
