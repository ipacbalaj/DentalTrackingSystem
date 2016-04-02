using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Controls.Buttons;
using DSA.Common.Controls.Buttons.SymbolButton;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.Entities.SettingsEntities;
using DSA.Module.PersonalData.SettingsColumns.Technician.Material.MaterialTile;
using Microsoft.Practices.Prism.Commands;

namespace DSA.Module.PersonalData.SettingsColumns.Technician.Material.MaterialTypeTile
{
    public class MaterialTypeTileViewModel:ViewModelBase
    {
        #region Constructor

        public MaterialTypeTileViewModel(MaterialTileViewModel parent)
        {
            Parent = parent;
//            DeleteBtn = new SymbolIconButtonViewModel(new DelegateCommand(OnDelete), ViewConstants.DeleteSymbol);
//            EditButton = new SymbolIconButtonViewModel(new DelegateCommand(OnEdit), ViewConstants.EditSymbol);
        }

        #endregion Constructor

        #region Properties

        public MaterialTileViewModel Parent { get; set; }

        private int id;

        public int Id
        {
            get { return id; }
            set
            {
                if (value == id)
                    return;
                id = value;
                OnPropertyChanged();
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (value == name)
                    return;
                name = value;
                OnPropertyChanged();
            }
        }

        private SettingsType localMaterialType;

        public SettingsType LocalMaterialType
        {
            get { return localMaterialType; }
            set
            {
                if (value == localMaterialType)
                    return;
                localMaterialType = value;
                OnPropertyChanged();
            }
        }

        private Brush deleteIconForeground=BackgroundColors.Black;

        public Brush DeleteIconForeground
        {
            get { return deleteIconForeground; }
            set
            {
                if (value == deleteIconForeground)
                    return;
                deleteIconForeground = value;
                OnPropertyChanged();
            }
        }

        private SymbolIconButtonViewModel deleteBtn;
        public SymbolIconButtonViewModel DeleteBtn
        {
            get { return deleteBtn; }
            set
            {
                if (deleteBtn == value)
                    return;
                deleteBtn = value;
                OnPropertyChanged();
            }
        }

        private SymbolIconButtonViewModel editButton;
        public SymbolIconButtonViewModel EditButton
        {
            get { return editButton; }
            set
            {
                if (value == editButton)
                    return;
                editButton = value;
                OnPropertyChanged();
            }
        }

        private double cost;

        public double Cost
        {
            get { return cost; }
            set
            {
                if (value == cost)
                    return;
                cost = value;
                OnPropertyChanged();
            }
        }

        private Brush contentBackground = BackgroundColors.lightblue;
        public Brush ContentBackGround
        {
            get { return contentBackground; }
            set
            {
                if (contentBackground != value)
                {
                    contentBackground = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility costTxtBoxVisibility=Visibility.Collapsed;

        public Visibility CostTxtBoxVisibility
        {
            get { return costTxtBoxVisibility; }
            set
            {
                if (value == costTxtBoxVisibility)
                    return;
                costTxtBoxVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility nameTxtBoxVisibility=Visibility.Collapsed;

        public Visibility NameTxtBoxVisibility
        {
            get { return nameTxtBoxVisibility; }
            set
            {
                if (value == nameTxtBoxVisibility)
                    return;
                nameTxtBoxVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility costTxtBlockVisibility;

        public Visibility CostTxtBlockVisibility
        {
            get { return costTxtBlockVisibility; }
            set
            {
                if (value == costTxtBlockVisibility)
                    return;
                costTxtBlockVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility nameTxtBlockVisibility;

        public Visibility NameTxtBlockVisibility
        {
            get { return nameTxtBlockVisibility; }
            set
            {
                if (value == nameTxtBlockVisibility)
                    return;
                nameTxtBlockVisibility = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public void OnCostClick()
        {
            CostTxtBoxVisibility=Visibility.Visible;
            Parent.Parent.Parent.MaterialTileListViewModel.WasChanged = true;
        }

        public void OnNameClick()
        {
            NameTxtBoxVisibility=Visibility.Visible;
            Parent.Parent.Parent.MaterialTileListViewModel.WasChanged = true;
        }

        public void OnSwitchView(bool isEDit)
        {
            if (isEDit)
            {
                CostTxtBoxVisibility = Visibility.Visible;
                NameTxtBoxVisibility = Visibility.Visible;
                CostTxtBlockVisibility = Visibility.Collapsed;
                NameTxtBlockVisibility = Visibility.Collapsed;
                Parent.Parent.Parent.MaterialTileListViewModel.WasChanged = true;
            }
            else
            {
                CostTxtBoxVisibility = Visibility.Collapsed;
                NameTxtBoxVisibility = Visibility.Collapsed;
                CostTxtBlockVisibility = Visibility.Visible;
                NameTxtBlockVisibility = Visibility.Visible;
            }
        }

        public void DeleteIconMouseEnter()
        {
            DeleteIconForeground = BackgroundColors.red;
        }

        public void DeleteIconMouseLeave()
        {
            DeleteIconForeground = BackgroundColors.Black;
        }

        public void DeleteIconMouseClick()
        {
            Parent.MaterialTypes.Remove(this);
            Parent.LocalMaterial.MaterialTypes.Remove(LocalMaterialType);
            Parent.Parent.deletedMaterialTypes.Add(Id);
            Parent.Parent.Parent.MaterialTileListViewModel.WasChanged = true;
        }

        private void OnEdit()
        {
            
        }

        #endregion Methods

    }
}
