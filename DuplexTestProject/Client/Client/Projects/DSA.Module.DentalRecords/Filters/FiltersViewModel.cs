using System.Collections.ObjectModel;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.Entities.SettingsEntities;
using DSA.Module.DentalRecords.Filters.FilterItem;
using DSA.Module.DentalRecords.Options.DisplayOptions;

namespace DSA.Module.DentalRecords.Filters
{
    public class FiltersViewModel:ViewModelBase
    {
        #region Constructor

        public FiltersViewModel(DisplayOptionsViewModel parent)
        {
            PopulateFilterItems();
            Parent = parent;
        }

        #endregion Constructor

        #region Properties

        public DisplayOptionsViewModel Parent { get; set; }

        private ObservableCollection<FilterItemViewModel> filterItemList;
        public ObservableCollection<FilterItemViewModel> FilterItemList
        {
            get { return filterItemList; }
            set
            {
                if (filterItemList != value)
                {
                    filterItemList = value;
                    OnPropertyChanged();
                }
            }
        }      

        #endregion Properties

        #region Methods

        private void PopulateFilterItems()
        {
             FilterItemList=new ObservableCollection<FilterItemViewModel>();
            foreach (var key in LocalCache.Instance.SettingsItems.Keys)
            {
                FilterItemList.Add(new FilterItemViewModel(this)
                {
                    SettingsItemName = key.ToString(),
                    SettingsItems = new ObservableCollection<SettingsItem>(LocalCache.Instance.SettingsItems[key])
                });
            }          
        }

        public void HasChanged()
        {
            
        }

        #endregion Methods

    }
}
