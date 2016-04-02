using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Controls.Buttons.OptionButton;
using DSA.Common.Infrastructure.Icos;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model.Entities.Filters;
using DSA.Database.Model.Entities.SettingsEntities;
using DSA.Filters.Filters.FilterItem.SelectedItemTile;
using Microsoft.Practices.Prism.Commands;

namespace DSA.Filters.Filters.FilterItem
{
    public class FilterItemViewModel:ViewModelBase
    {
        #region Constructor

        public FilterItemViewModel(FiltersViewModel parent)
        {
            SetIcons();
            Parent = parent;
            IconButton =new OptionButtonViewModel()
            {                
                OnClick = new DelegateCommand(OnIconClick),
                Icon = UncheckedIcon
            };
        }

        #endregion Constructor

        #region Properties

        public FiltersViewModel Parent { get; set; }

        public List<int> itemsToDelete = new List<int>(); 

        private ObservableCollection<SettingsItem> settingsItems;
        public ObservableCollection<SettingsItem> SettingsItems
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

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    if (isChecked)
                    {
                        IconButton.Icon = CheckedIcon;
                        ContentBackground = BackgroundColors.ActiveFilter;
                        // ActiveInactive = "Dezactiveaza Filtrul";
                    }
                    else
                    {
                        IconButton.Icon = UncheckedIcon;
                        ContentBackground = BackgroundColors.InactiveFilter;
                        //ActiveInactive = "Activeaza Filtrul";
                    }
                    Parent.HasChanged(true);
                    OnPropertyChanged();
                }
            }
        }

        private string activeInactive;
        public string ActiveInactive
        {
            get { return activeInactive; }
            set
            {
                if (activeInactive != value)
                {
                    activeInactive = value;
                    Parent.HasChanged(true);
                    OnPropertyChanged();
                }
            }
        }

        private int filterId;
        public int FilterId
        {
            get { return filterId; }
            set
            {
                if (value == filterId)
                    return;
                filterId = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SelectedItemTileViewModel> selectedItemTileViewModels=new ObservableCollection<SelectedItemTileViewModel>();
        public ObservableCollection<SelectedItemTileViewModel> SelectedItemTileViewModels
        {
            get { return selectedItemTileViewModels; }
            set
            {
                if (selectedItemTileViewModels != value)
                {
                    selectedItemTileViewModels = value;
                    OnPropertyChanged();
                }
            }
        }

        private OptionButtonViewModel iconButton;

        public OptionButtonViewModel IconButton
        {
            get { return iconButton; }
            set
            {
                if (value == iconButton)
                    return;
                iconButton = value;
                OnPropertyChanged();
            }
        }

        private string settingsItemName;
        public string SettingsItemName
        {
            get { return settingsItemName; }
            set
            {
                if (settingsItemName != value)
                {
                    settingsItemName = value+":";
                    OnPropertyChanged();
                }
            }
        }

        private Brush iconBackground;

        public Brush IconBackgroud
        {
            get { return iconBackground; }
            set
            {
                if (value == iconBackground)
                    return;
                iconBackground = value;
                OnPropertyChanged();
            }
        }
       
        private SettingsItem selectedSettingsItem;
        public SettingsItem SelectedSettingsItem
        {
            get { return selectedSettingsItem; }
            set
            {
                if (selectedSettingsItem != value )
                {
                    selectedSettingsItem = value;
                    if(selectedSettingsItem!=null)
                    AddSelected(selectedSettingsItem.Name,0);
                    OnPropertyChanged();
                }
            }
        }

        private UIElement checkedIcon;

        public UIElement CheckedIcon
        {
            get { return checkedIcon; }
            set
            {
                if (value == checkedIcon)
                    return;
                checkedIcon = value;
                OnPropertyChanged();
            }
        }

        private UIElement uncheckedIcon;

        public UIElement UncheckedIcon
        {
            get { return uncheckedIcon; }
            set
            {
                if (value == uncheckedIcon)
                    return;
                uncheckedIcon = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        private void AddSelected(string name,int id)
        {
            SelectedItemTileViewModels.Add(new SelectedItemTileViewModel(this)
            {
                Id = id,
                Name = name,
            });
            Parent.HasChanged(true);
        }

        public void RemoveItem(SelectedItemTileViewModel selectedItemTileViewModel)
        {
            if(selectedItemTileViewModel.Id!=0) itemsToDelete.Add(selectedItemTileViewModel.Id);
            SelectedItemTileViewModels.Remove(selectedItemTileViewModel);
            Parent.HasChanged(true);
        }

        public void SetFIlterItem(LocalFilter localFilter)
        {
            foreach (var localFilterItem in localFilter.FilterItems)
            {
                var settingItem = SettingsItems.FirstOrDefault(item => item.Name == localFilterItem.Name);
                if(settingItem!=null)
                AddSelected(settingItem.Name,localFilterItem.Id);
            }
            IsChecked = localFilter.IsActive;
            FilterId = localFilter.Id;
            Parent.HasChanged(false);
        }

        private void OnIconClick()
        {
            IsChecked = !IsChecked;
        }

        private void SetIcons()
        {
            CheckedIcon = new CloseIcon();
            UncheckedIcon =new BlueCancelIcon();
        }

        private Brush contentBackground = BackgroundColors.InactiveFilter;

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

        #endregion Methods

    }
}
