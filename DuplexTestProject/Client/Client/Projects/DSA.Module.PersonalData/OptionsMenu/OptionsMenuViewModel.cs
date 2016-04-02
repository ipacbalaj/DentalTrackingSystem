using System;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Controls.Buttons.OptionButton;
using DSA.Common.Infrastructure;
using DSA.Common.Infrastructure.Icos;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Module.PersonalData.SettingsColumns.SettingsItem;
using DSA.Module.PersonalData.SettingsColumns.SettingsTileList;

using DSA.Module.PersonalData.SettingsColumns.Works.WorkTileList;
using DSA.Module.PersonalData.SettingsDataScreen;
using Microsoft.Practices.Prism.Commands;
using TULIP.Common.Controls.Icons;

namespace DSA.Module.PersonalData.OptionsMenu
{
    public class OptionsMenuViewModel : ViewModelBase
    {

        #region Properties

        public SettingsScreenViewModel Parent { get; set; }

        public OptionButtonViewModel WorksButtonViewModel { get; private set; }
        public OptionButtonViewModel MaterialsButtonViewModel { get; private set; }
        public OptionButtonViewModel LocationButtonViewModel { get; private set; }
        public OptionButtonViewModel AreaButtonViewModel { get; private set; }
        public OptionButtonViewModel SelectedButton { get; private set; }
        public OptionButtonViewModel TechnicianButtonViewModel { get; private set; }

        private Brush contentBackground;
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

        #endregion

        #region Constructor

        public OptionsMenuViewModel(SettingsScreenViewModel parent)
        {
            Parent = parent;
            SelectedButton = new OptionButtonViewModel();
            InitButtons();
        }

        #endregion

        #region Methods

        private void InitButtons()
        {
            WorksButtonViewModel = new OptionButtonViewModel
            {
                ImagePath = ImagePath.Work,
                OnClick = new DelegateCommand(OnWorks),
                Name = "Manopere"
            };

            MaterialsButtonViewModel = new OptionButtonViewModel
            {
                ImagePath = ImagePath.Materiale,
                OnClick = new DelegateCommand(OnMaterials),
                Name = "Materiale",
            };

            LocationButtonViewModel = new OptionButtonViewModel
            {
                ImagePath = ImagePath.Location,
                OnClick = new DelegateCommand(OnLocation),
                Name = "Locații",
            };


            AreaButtonViewModel = new OptionButtonViewModel
            {
                ImagePath = ImagePath.Zona,
                OnClick = new DelegateCommand(OnArea),
                Name = "Zone",
            };

            TechnicianButtonViewModel = new OptionButtonViewModel
            {
                ImagePath = ImagePath.Zona,
                OnClick = new DelegateCommand(OnTechs),
                Name = "Tehnicieni",
            };
        }

        public void OnWorks()
        {
            SelectedButton.IsSelected = false;
            WorksButtonViewModel.IsSelected = true;
            SelectedButton = WorksButtonViewModel;
            Parent.WorkTileListViewModel = new WorkTileListViewModel();
            Parent.SettingsTileListViewModel = new SettingsTileListViewModel(LocalCache.SettingsItemEnum.Manopera);
            Parent.WorksVisibility = Visibility.Visible;
        }

        private void OnMaterials()
        {
            SelectedButton.IsSelected = false;
            MaterialsButtonViewModel.IsSelected = true;
            SelectedButton = MaterialsButtonViewModel;
            Parent.SettingsTileListViewModel = new SettingsTileListViewModel(LocalCache.SettingsItemEnum.Material);
            Parent.WorksVisibility = Visibility.Collapsed;
        }

        private void OnLocation()
        {
            SelectedButton.IsSelected = false;
            LocationButtonViewModel.IsSelected = true;
            SelectedButton = LocationButtonViewModel;
            Parent.SettingsTileListViewModel = new SettingsTileListViewModel(LocalCache.SettingsItemEnum.Locatie);
            Parent.WorksVisibility = Visibility.Collapsed;
        }

        private void OnArea()
        {
            SelectedButton.IsSelected = false;
            AreaButtonViewModel.IsSelected = true;
            SelectedButton = AreaButtonViewModel;
            Parent.SettingsTileListViewModel = new SettingsTileListViewModel(LocalCache.SettingsItemEnum.Zona);
            Parent.WorksVisibility = Visibility.Collapsed;
        }

        private void OnTechs()
        {
            SelectedButton.IsSelected = false;
            TechnicianButtonViewModel.IsSelected = true;
            SelectedButton = TechnicianButtonViewModel;
            Parent.SettingsTileListViewModel = new SettingsTileListViewModel(LocalCache.SettingsItemEnum.Tehnician);
            Parent.WorksVisibility = Visibility.Collapsed;
        }

        private void SetVisibility(bool isTechnician)
        {
            if (isTechnician)
            {
                Parent.SettingsListVisibility = Visibility.Collapsed;
                Parent.WorksVisibility = Visibility.Collapsed;
                Parent.TechnicianListVisibility = Visibility.Visible;
            }
            else
            {
                Parent.SettingsListVisibility = Visibility.Visible;
                Parent.TechnicianListVisibility = Visibility.Collapsed;
                Parent.WorksVisibility = Visibility.Collapsed;
            }
        }

        private void SetWorkVis()
        {
            Parent.SettingsListVisibility = Visibility.Collapsed;
            Parent.WorksVisibility = Visibility.Visible;
            Parent.TechnicianListVisibility = Visibility.Collapsed;
        }

        private void SetVisibilityBasedOnType(MenuItem menuItem)
        {
            switch (menuItem)
            {
                case MenuItem.Areas:

                    break;
                case MenuItem.Locations:
                    break;
                case MenuItem.Techs:
                    break;
                case MenuItem.Works:
                    break;
            }
        }

        #endregion

        enum MenuItem { Works, Locations, Techs, Areas }
    }
}
