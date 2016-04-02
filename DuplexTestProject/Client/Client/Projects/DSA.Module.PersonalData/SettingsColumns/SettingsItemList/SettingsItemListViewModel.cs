using System;
using System.Collections.ObjectModel;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Module.PersonalData.SettingsColumns.SettingsItem;

namespace DSA.Module.PersonalData.SettingsColumns.SettingsItemList
{
    public class SettingsItemListViewModel:ViewModelBase
    {
        #region Constructor

        #endregion Constructor

        #region Properties

        private ObservableCollection<SettingsItemViewModel> settingsItems;
        public ObservableCollection<SettingsItemViewModel> SettingsItems
        {
            get { return settingsItems; }
            set
            {
                if (settingsItems != value)
                {
                    settingsItems = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion Properties

        #region Methods



        public void PopulateSettingsItems()
        {
//            SettingsItems=new ObservableCollection<SettingsItemViewModel>();
//            foreach (var key in LocalCache.Instance.SettingsItems.Keys)
//            {
//                var list = new ObservableCollection<DisplaySettingsItemHelper>();
//                foreach (var settingsSubItem in LocalCache.Instance.SettingsItems[key])
//                {
//                    list.Add(new DisplaySettingsItemHelper()
//                    {
//                        Id = settingsSubItem.Id,
//                        Name = settingsSubItem.Name
//                        
//                    });
//                }
//                SettingsItems.Add(new SettingsItemViewModel()
//                {
//                    SettingsColumnName = key.ToString(),
//                    SettingsSubItems = new ObservableCollection<DisplaySettingsItemHelper>(list)
//                });
//            }
        }

        #endregion Methods

    }
}
