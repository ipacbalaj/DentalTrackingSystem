using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Common.Controls.Buttons;
using DSA.Common.Infrastructure;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Module.PersonalData.SettingsColumns.Works.WorkTile.WorkTypeTile;
using Microsoft.Practices.Prism.Commands;

namespace DSA.Module.PersonalData.SettingsColumns.Works.WorkTile.WorkTypeTileList
{
   public class WorkTypeTileListViewModel:ViewModelBase
    {
       #region Constructor

       public WorkTypeTileListViewModel(WorkTileViewModel parent)
       {
           AddWorkTypeButton = new ActionButtonViewModel("Adaugă Tip", new DelegateCommand(OnAddWorkType), ImagePath.AddPatientPath);
           Parent = parent;
       }

       #endregion Constructor

       #region Properties

       public WorkTileViewModel Parent { get; set; }

       private ObservableCollection<WorkTypeTileViewModel> workTypes=new ObservableCollection<WorkTypeTileViewModel>();
       public ObservableCollection<WorkTypeTileViewModel> WorkTypes
       {
           get { return workTypes; }
           set
           {
               if (value == workTypes)
                   return;
               workTypes = value;
               OnPropertyChanged();
           }
       }

       private ActionButtonViewModel addWorkTypeButton;
       public ActionButtonViewModel AddWorkTypeButton
       {
           get { return addWorkTypeButton; }
           set
           {
               if (value == addWorkTypeButton)
                   return;
               addWorkTypeButton = value;
               OnPropertyChanged();
           }
       }



       #endregion Properties

       #region Methods

       private void OnAddWorkType()
       {
           if(WorkTypes==null) WorkTypes=new ObservableCollection<WorkTypeTileViewModel>();
           var newWorkType = new WorkTypeTileViewModel(this);
           WorkTypes.Add(newWorkType);
           newWorkType.OnEdit();
           Parent.WasChanged = true;

       }

       public void OnSave()
       {
           foreach (var workTypeTileViewModel in WorkTypes)
           {
               workTypeTileViewModel.OnSave();
           }
       }

       public void RemoveItem(WorkTypeTileViewModel tileToRemove)
       {
           WorkTypes.Remove(tileToRemove);
           Parent.WasChanged = true;
           if (tileToRemove.Id != 0)
           {
               Parent.Parent.workTypeToDeleteIds.Add(tileToRemove.Id);
           }
       }



       #endregion Methods
    }
}
