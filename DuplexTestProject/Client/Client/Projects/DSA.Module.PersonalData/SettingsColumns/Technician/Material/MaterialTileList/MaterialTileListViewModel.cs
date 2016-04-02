using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Controls.Buttons;
using DSA.Common.Infrastructure;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.Entities;
using DSA.Database.Model.Entities.SettingsEntities;
using DSA.Module.PersonalData.SettingsColumns.Technician.Material.MaterialTile;
using DSA.Module.PersonalData.SettingsColumns.Technician.Material.MaterialTypeTile;
using DSA.Module.PersonalData.SettingsColumns.Technician.TechnicianTile;
using Microsoft.Practices.Prism.Commands;

namespace DSA.Module.PersonalData.SettingsColumns.Technician.Material.MaterialTileList
{
    public class MaterialTileListViewModel : ViewModelBase
    {
        #region Constructor

        public MaterialTileListViewModel(TechnicianTileViewModel parent)
        {
            Parent = parent;
            SaveButton = new ActionButtonViewModel("Salvează  ", new DelegateCommand(OnSave), ImagePath.SaveIconPath);
            AddMaterialButton = new ActionButtonViewModel("Adaugă Material", new DelegateCommand(OnAddMaterial), ImagePath.AddPatientPath);
        }

        #endregion Constructor

        #region Properties

         public MaterialTileListView ViewReference { get; set; }

        public List<int> deletedMaterials = new List<int>();
        public List<int> deletedMaterialTypes = new List<int>();

        private Brush stackColor=null;

        public Brush StackColor
        {
            get { return stackColor; }
            set
            {
                if (value == stackColor)
                    return;
                stackColor = value;
                OnPropertyChanged();
            }
        }

        private bool wasChanged;

        public bool WasChanged
        {
            get { return wasChanged; }
            set
            {
                if (value == wasChanged)
                    return;
                wasChanged = value;
                StackColor = wasChanged ? BackgroundColors.BackgroundFilled : null;
                OnPropertyChanged();
            }
        }

        public TechnicianTileViewModel Parent { get; set; }

        private ObservableCollection<MaterialTileViewModel> materials = new ObservableCollection<MaterialTileViewModel>();

        public ObservableCollection<MaterialTileViewModel> Materials
        {
            get { return materials; }
            set
            {
                if (value == materials)
                    return;
                materials = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel saveButton;

        public ActionButtonViewModel SaveButton
        {
            get { return saveButton; }
            set
            {
                if (value == saveButton)
                    return;
                saveButton = value;
                OnPropertyChanged();
            }
        }

        private Brush contentBackground = BackgroundColors.TechnicianTileMouseOn;
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

        private ActionButtonViewModel addMaterialButton;

        public ActionButtonViewModel AddMaterialButton
        {
            get { return addMaterialButton; }
            set
            {
                if (value == addMaterialButton)
                    return;
                addMaterialButton = value;
                OnPropertyChanged();
            }
        }

        private Visibility addButtonVisibility;

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

        #endregion Properties

        #region Methods

        private async void OnSave()
        {
            LocalTechnician technician = new LocalTechnician()
            {
                Id = Parent.Id,
                Name = Parent.Name,
                Materials = new List<LocalMaterial>()
            };

            foreach (var materialTileViewModel in Materials)
            {
                technician.Materials.Add(materialTileViewModel.OnSave());
            }
            if(Parent.Technician!=null)
            Parent.Technician.Materials = technician.Materials;
            var updatedTehnician = LocalCache.Instance.Technicians.FirstOrDefault(item => item.Id == technician.Id);
            LocalCache.Instance.Technicians.Remove(updatedTehnician);
            LocalCache.Instance.Technicians.Add(technician);
            //todo:add technician
//            DatabaseHandler.Instance.AddTechnicianAndMaterials(technician);
            await DatabaseHandler.Instance.DeleteMaterials(deletedMaterials);
            await DatabaseHandler.Instance.DeleteMaterialTypes(deletedMaterialTypes);
            deletedMaterials=new List<int>();
            deletedMaterialTypes=new List<int>();
            WasChanged = false;
        }

        private void OnAddMaterial()
        {
            var materialTile = new MaterialTileViewModel(this);
            materialTile.OnSwitchView(true);
            materialTile.MaterialTypes = new ObservableCollection<MaterialTypeTileViewModel>();
            materialTile.ContentBackground = BackgroundColors.SuccessfulColor;
            Materials.Add(materialTile);
            ViewReference.scrollViewerMatTiles.ScrollToBottom();
            WasChanged = true;
        }

        #endregion Methods
    }
}
