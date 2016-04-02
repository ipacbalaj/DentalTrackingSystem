using System.Collections.Generic;
using System.Collections.ObjectModel;
using DSA.Common.Controls.Buttons;
using DSA.Common.Infrastructure;
using DSA.Common.Infrastructure.Entities;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using Microsoft.Practices.Prism.Commands;
using DSA.Database.Model.Entities.SettingsEntities;

namespace DSA.Module.PersonalData.SettingsColumns.SettingsItem
{
    public class SettingsItemViewModel:ViewModelBase
    {
        #region Constructor

        public SettingsItemViewModel(LocalCache.SettingsItemEnum key)
        {
            AddSetItem = new ActionButtonViewModel("Add", new DelegateCommand(OnSettingAdded), ImagePath.SaveIconPath);
            PopulateSettingsSubItems(key);
        }
        #endregion Constructor

        #region Properties
        private ObservableCollection<DisplaySettingsItemHelper> settingsSubItems;
        public ObservableCollection<DisplaySettingsItemHelper> SettingsSubItems
        {
            get { return settingsSubItems; }
            set
            {
                if (settingsSubItems != value)
                {
                    settingsSubItems = value;
                    OnPropertyChanged();
                }
            }
        }

        private string settingsColumnName;
        public string SettingsColumnName
        {
            get { return settingsColumnName; }
            set
            {
                if (settingsColumnName != value)
                {
                    settingsColumnName = value;
                    OnPropertyChanged();
                }
            }
        }

        private ActionButtonViewModel addSetItem;
        public ActionButtonViewModel AddSetItem
        {
            get { return addSetItem; }
            set
            {
                if (addSetItem == value)
                    return;
                addSetItem = value;
                OnPropertyChanged();
            }
        }

        private string addedSettingName;
        public string AddedSettingName
        {
            get { return addedSettingName; }
            set
            {
                if (addedSettingName == value)
                    return;
                addedSettingName = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        private void OnSettingAdded()
        {
            SettingsSubItems.Add(new DisplaySettingsItemHelper()
            {
                Name = addedSettingName
            });
            switch (SettingsColumnName)
            {
                case "Locatie":
                    LocalCache.Instance.SettingsItems[LocalCache.SettingsItemEnum.Locatie].Add(new Database.Model.Entities.SettingsEntities.SettingsItem()
                    {
                        Name = addedSettingName
                    });
                    break;
                case "Material":
                    LocalCache.Instance.SettingsItems[LocalCache.SettingsItemEnum.Material].Add(new Database.Model.Entities.SettingsEntities.SettingsItem()
                    {
                        Name = addedSettingName
                    });
                    break;
                case "Manopera":
                    LocalCache.Instance.SettingsItems[LocalCache.SettingsItemEnum.Manopera].Add(new Database.Model.Entities.SettingsEntities.SettingsItem()
                    {
                        Name = addedSettingName
                    });
                    break;
                case "Zona":
                    LocalCache.Instance.SettingsItems[LocalCache.SettingsItemEnum.Zona].Add(new Database.Model.Entities.SettingsEntities.SettingsItem()
                    {
                        Name = addedSettingName
                    });
                    break;
            }
        }

        public void PopulateSettingsSubItems(LocalCache.SettingsItemEnum key)
        {           
            var list = new ObservableCollection<DisplaySettingsItemHelper>();
            foreach (var settingsSubItem in LocalCache.Instance.SettingsItems[key])
            {
                list.Add(new DisplaySettingsItemHelper()
                {
                    Id = settingsSubItem.Id,
                    Name = settingsSubItem.Name

                });
            }
            SettingsSubItems = new ObservableCollection<DisplaySettingsItemHelper>(list);
        }



        #endregion Methods

    }
}
