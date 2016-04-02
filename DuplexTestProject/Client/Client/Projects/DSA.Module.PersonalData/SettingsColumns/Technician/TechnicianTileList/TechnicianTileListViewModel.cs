using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DSA.Common.Controls.Buttons;
using DSA.Common.Infrastructure;
using DSA.Common.Infrastructure.ObjectList;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Module.PersonalData.SettingsColumns.SettingsTile;
using DSA.Module.PersonalData.SettingsColumns.SettingsTile.EditSettingsTile;
using DSA.Module.PersonalData.SettingsColumns.Technician.Material.MaterialTile;
using DSA.Module.PersonalData.SettingsColumns.Technician.Material.MaterialTileList;
using DSA.Module.PersonalData.SettingsColumns.Technician.Material.MaterialTypeTile;
using DSA.Module.PersonalData.SettingsColumns.Technician.TechnicianTile;
using DSA.Module.PersonalData.SettingsColumns.Technician.TechnicianTile.AddTechnicianTile;
using Microsoft.Practices.Prism.Commands;

namespace DSA.Module.PersonalData.SettingsColumns.Technician.TechnicianTileList
{
   public class TechnicianTileListViewModel:ViewModelBase
    {
       #region Constructor

       public TechnicianTileListViewModel()
       {
           PopulateTechnicians();
           AddTechnicianButton = new ActionButtonViewModel("Adaugă Tehnician", new DelegateCommand(OnAddTechnician), ImagePath.AddPatientPath);
           AddTechnicianTileViewModel=new AddTechnicianTileViewModel(this);
           SaveTechnicianButton = new ActionButtonViewModel("Salvează  ", new DelegateCommand(OnSave), ImagePath.SaveIconPath);
       }

       #endregion Constructor

       #region Properties

       private AddTechnicianTileViewModel addTechnicianTileViewModel;
       public AddTechnicianTileViewModel AddTechnicianTileViewModel
       {
           get { return addTechnicianTileViewModel; }
           set
           {
               if (value == addTechnicianTileViewModel)
                   return;
               addTechnicianTileViewModel = value;
               OnPropertyChanged();
           }
       }

       private ObjectList<TechnicianTileViewModel> techniciansObjectList;
       public ObjectList<TechnicianTileViewModel> TechniciansObjectList
       {
           get { return techniciansObjectList; }
           set
           {
               if (value == techniciansObjectList)
                   return;
               techniciansObjectList = value;
               OnPropertyChanged();
           }
       }

       private ActionButtonViewModel addTechnicianButton;

       public ActionButtonViewModel AddTechnicianButton
       {
           get { return addTechnicianButton; }
           set
           {
               if (value == addTechnicianButton)
                   return;
               addTechnicianButton = value;
               OnPropertyChanged();
           }
       }

       private Visibility saveButtonVisibility=Visibility.Collapsed;

       public Visibility SaveButtonVisibility
       {
           get { return saveButtonVisibility; }
           set
           {
               if (value == saveButtonVisibility)
                   return;
               saveButtonVisibility = value;
               OnPropertyChanged();
           }
       }

       private EditSettingsTileViewModel addTechnitianTile;
       public EditSettingsTileViewModel AddTechnicianTile
       {
           get { return addTechnitianTile; }
           set
           {
               if (value == addTechnitianTile)
                   return;
               addTechnitianTile = value;
               OnPropertyChanged();
           }
       }

       private Visibility addButtonVisibility =Visibility.Visible;
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

       private Visibility addTileVisibility=Visibility.Collapsed;
       public Visibility AddTileVisibility
       {
           get { return addTileVisibility; }
           set
           {
               if (value == addTileVisibility)
                   return;
               addTileVisibility = value;
               OnPropertyChanged();
           }
       }

       private ActionButtonViewModel saveTechnicianButton;

       public ActionButtonViewModel SaveTechnicianButton
       {
           get { return saveTechnicianButton; }
           set
           {
               if (value == saveTechnicianButton)
                   return;
               saveTechnicianButton = value;
               OnPropertyChanged();
           }
       }

       private MaterialTileListViewModel materialTileListViewModel;

       public MaterialTileListViewModel MaterialTileListViewModel
       {
           get { return materialTileListViewModel; }
           set
           {
               if (value == materialTileListViewModel)
                   return;
               materialTileListViewModel = value;
               OnPropertyChanged();
           }
       }

       #endregion Properties

       #region Methods

       private void PopulateTechnicians()
       {
           TechniciansObjectList=new ObjectList<TechnicianTileViewModel>(false);
           foreach (var localTechnician in LocalCache.Instance.Technicians)
           {
               var technicianTile = new TechnicianTileViewModel(this)
               {
                   Id = localTechnician.Id,
                   Name = localTechnician.Name,
                   Technician = localTechnician
               };
               technicianTile.MaterialTileListViewModel=new MaterialTileListViewModel(technicianTile);
               //set materials for technician
               if(localTechnician.Materials!=null)
               foreach (var localMaterial in localTechnician.Materials)
               {
                   var materialTile = new MaterialTileViewModel(technicianTile.MaterialTileListViewModel)
                   {
                       Name = localMaterial.Name,
                       Cost = localMaterial.Cost,
                       MaterialTypes = new ObservableCollection<MaterialTypeTileViewModel>(),
                       MaterialId = localMaterial.Id,
                       LocalMaterial = localMaterial
                   };

                   //set types for material 
                   foreach (var materialType in localMaterial.MaterialTypes)
                   {
                       materialTile.MaterialTypes.Add(new MaterialTypeTileViewModel(materialTile)
                       {
                           Name = materialType.Name,
                           Id = materialType.Id,
                           Cost = materialType.Cost,
                           LocalMaterialType = materialType
                       });
                   }
                   technicianTile.MaterialTileListViewModel.Materials.Add(materialTile);
               }
               else
               {
                   technicianTile.MaterialTileListViewModel=new MaterialTileListViewModel(technicianTile);
               }
               TechniciansObjectList.Add(technicianTile);
           }
           if (TechniciansObjectList.List.Count>0)
           {
               var selectTechnician = TechniciansObjectList.List.FirstOrDefault();
               selectTechnician.OnSelected(selectTechnician);
           }
       }

       private void OnSave()
       {
           foreach (var technicianTileViewModel in TechniciansObjectList.List.ToList())
           {
               technicianTileViewModel.OnSave();
           }
           AddButtonVisibility=Visibility.Visible;
           SaveButtonVisibility=Visibility.Collapsed;
       }

       private void OnAddTechnician()
       {
           AddButtonVisibility=Visibility.Collapsed;
           var technicianTile = new TechnicianTileViewModel(this);
           technicianTile.IsEdit = true;
           TechniciansObjectList.Add(technicianTile);
           SaveButtonVisibility=Visibility.Visible;
           //AddTileVisibility=Visibility.Visible;
       }

       #endregion Methods
    }
}
