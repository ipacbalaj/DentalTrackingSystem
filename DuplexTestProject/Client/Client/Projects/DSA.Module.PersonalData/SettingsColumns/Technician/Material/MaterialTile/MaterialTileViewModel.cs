using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Controls.Buttons;
using DSA.Common.Controls.Buttons.SymbolButton;
using DSA.Common.Infrastructure;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.Entities.SettingsEntities;
using DSA.Module.PersonalData.SettingsColumns.SettingsTile;
using DSA.Module.PersonalData.SettingsColumns.Technician.Material.MaterialTileList;
using DSA.Module.PersonalData.SettingsColumns.Technician.Material.MaterialTypeTile;
using Microsoft.Practices.Prism.Commands;

namespace DSA.Module.PersonalData.SettingsColumns.Technician.Material.MaterialTile
{
    public class MaterialTileViewModel:ViewModelBase
    {
        #region Constructor

        public MaterialTileViewModel(MaterialTileListViewModel parent)
        {
            Parent = parent;
            DeleteMaterialButton=new SymbolIconButtonViewModel(new DelegateCommand(OnDeleteMaterial),ViewConstants.DeleteSymbol);
        }

        #endregion Constructor

        #region Properties

        public MaterialTileListViewModel Parent { get; set; }

        private LocalMaterial localMaterial;

        public LocalMaterial LocalMaterial
        {
            get { return localMaterial; }
            set
            {
                if (value == localMaterial)
                    return;
                localMaterial = value;
                OnPropertyChanged();
            }
        }

        private int materialId;
        public int MaterialId
        {
            get { return materialId; }
            set
            {
                if (value == materialId)
                    return;
                materialId = value;
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

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value == name)
                    return;
                name = value;
                AddMaterialTypeButton = new ActionButtonViewModel("Tip " + value, new DelegateCommand(OnAddMaterialType), ImagePath.AddPatientPath);//new SymbolIconButtonViewModel(new DelegateCommand(OnAddMaterialType), "Adauga Tip " + value);
                AddButtonVisibility=Visibility.Visible;
                OnPropertyChanged();
            }
        }

        private Visibility addButtonVisibility=Visibility.Collapsed;

        public Visibility AddButtonVisibility
        {
            get { return addButtonVisibility; }
            set
            {
                if (value == addButtonVisibility)
                    return;
                addButtonVisibility = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MaterialTypeTileViewModel> materialTypes;
        public ObservableCollection<MaterialTypeTileViewModel> MaterialTypes
        {
            get { return materialTypes; }
            set
            {
                if (value == materialTypes)
                    return;
                materialTypes = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel addMaterialTypeButton;

        public ActionButtonViewModel AddMaterialTypeButton
        {
            get { return addMaterialTypeButton; }
            set
            {
                if (value == addMaterialTypeButton)
                    return;
                addMaterialTypeButton = value;
                OnPropertyChanged();
            }
        }

        private SymbolIconButtonViewModel deleteMaterialButton;

        public SymbolIconButtonViewModel DeleteMaterialButton
        {
            get { return deleteMaterialButton; }
            set
            {
                if (value == deleteMaterialButton)
                    return;
                deleteMaterialButton = value;
                OnPropertyChanged();
            }
        }

        private Visibility nameTxtBlockVisibility =Visibility.Visible;
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

        private Visibility costTxtBlockVisibility=Visibility.Visible;

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

        private Visibility costTxtBoxVisibility =Visibility.Collapsed;
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

        private Brush contentBackground=BackgroundColors.MaterialTileMouseOff;
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

        #endregion Properties

        #region Methods

        private void OnAddMaterialType()
        {
            var materialTypeTile = new MaterialTypeTileViewModel(this);   
            materialTypeTile.OnSwitchView(true);            
            MaterialTypes.Add(materialTypeTile);
        }

        public LocalMaterial OnSave()
        {
            OnSwitchView(false);
            var localMaterial = new LocalMaterial();
            localMaterial.Id = MaterialId;
            localMaterial.Name = Name;
            localMaterial.Cost = Cost;
            localMaterial.MaterialTypes=new List<SettingsType>();
            foreach (var materialTypeTileViewModel in MaterialTypes)
            {
                materialTypeTileViewModel.OnSwitchView(false);
                localMaterial.MaterialTypes.Add(new SettingsType()
                {
                    Id = materialTypeTileViewModel.Id,
                    Name = materialTypeTileViewModel.Name,
                    Cost = materialTypeTileViewModel.Cost
                });
            }
            LocalMaterial = localMaterial;
            var materials = LocalCache.Instance.SettingsItems[LocalCache.SettingsItemEnum.Material];
            if (!materials.Any(item => item.Name == localMaterial.Name))
            {
                materials.Add(localMaterial);
            }
            return localMaterial;
        }

        public void OnNameClick()
        {
            NameTxtBlockVisibility=Visibility.Collapsed;
            NameTxtBoxVisibility=Visibility.Visible;
        }

        public void OnCostClick()
        {
            CostTxtBlockVisibility = Visibility.Collapsed;
            CostTxtBoxVisibility = Visibility.Visible;
        }

        public void OnSwitchView(bool isEditMode)
        {
            if (isEditMode)
            {
                NameTxtBlockVisibility = Visibility.Collapsed;
                NameTxtBoxVisibility = Visibility.Visible;
                CostTxtBlockVisibility = Visibility.Collapsed;
                CostTxtBoxVisibility = Visibility.Visible;
            }
            else
            {
                NameTxtBlockVisibility = Visibility.Visible;
                NameTxtBoxVisibility = Visibility.Collapsed;
                CostTxtBlockVisibility = Visibility.Visible;
                CostTxtBoxVisibility = Visibility.Collapsed;
            }

        }

        public void OnDeleteMaterial()
        {
            Parent.Materials.Remove(this);
            Parent.Parent.Technician.Materials.Remove(LocalMaterial);
            Parent.deletedMaterials.Add(MaterialId);
        }

        public void MouseEnter()
        {
            ContentBackground = BackgroundColors.MaterialTileMouseOn;
        }

        public void MouseLeave()
        {
            ContentBackground = BackgroundColors.MaterialTileMouseOff;
        }

        #endregion Methods
    }
}
