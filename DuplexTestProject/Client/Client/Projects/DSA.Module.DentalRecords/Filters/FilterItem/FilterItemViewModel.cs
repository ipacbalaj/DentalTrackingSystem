using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model.Entities.SettingsEntities;
using DSA.Module.DentalRecords.Filters.FilterItem.SelectedItemTile;

namespace DSA.Module.DentalRecords.Filters.FilterItem
{
    public class FilterItemViewModel:ViewModelBase
    {
        #region Constructor

        public FilterItemViewModel(FiltersViewModel parent)
        {
            Parent = parent;
        }

        #endregion Constructor

        #region Properties

        public FiltersViewModel Parent { get; set; }

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
                    Parent.Parent.Parent.FilterInterventions();
                    OnPropertyChanged();
                }
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

       
        private SettingsItem selectedSettingsItem;
        public SettingsItem SelectedSettingsItem
        {
            get { return selectedSettingsItem; }
            set
            {
                if (selectedSettingsItem != value)
                {
                    selectedSettingsItem = value;
                    AddSelected(selectedSettingsItem);
                    Parent.Parent.Parent.FilterInterventions();
                    OnPropertyChanged();
                }
            }
        }

        #endregion Properties

        #region Methods

        private void AddSelected(SettingsItem settingsItem)
        {
            //if (!SelectedItemTileViewModels.Any(item => item.Name == settingsItem.Name))
            SelectedItemTileViewModels.Add(new SelectedItemTileViewModel()
            {
                Name = settingsItem.Name
            });
//            else
//            {
//                MessageBox.Show("Filtrul adaugat exista");
//            }
        }
        #endregion Methods

    }
}
