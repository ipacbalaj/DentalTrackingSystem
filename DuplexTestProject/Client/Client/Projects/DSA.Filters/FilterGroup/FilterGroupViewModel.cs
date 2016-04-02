using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DSA.Common.Controls.Buttons;
using DSA.Common.Controls.Loading.MetroLoading;
using DSA.Common.Infrastructure;
using DSA.Common.Infrastructure.ObjectList;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.Entities;
using DSA.Database.Model.Entities.Filters;
using DSA.Filters.Filters;
using DSA.Filters.FiltersScreen;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;

namespace DSA.Filters.FilterGroup
{
    public class FilterGroupViewModel:ViewModelBase
    {
        #region Constructor

        public FilterGroupViewModel(FIltersScreenViewModel parent)
        {
            Parent = parent;
            FiltersViewModels = new ObjectList<FiltersViewModel>(true);
            SaveGroupButton = new ActionButtonViewModel("Salvează Grup", new DelegateCommand(OnSaveGroup), ImagePath.AddPatientPath);
            AddGroupButton= new ActionButtonViewModel("Adaugă Grup ",new DelegateCommand(OnAddNewGroup),ImagePath.AddFilter);
            CancelGroupButton=new ActionButtonViewModel("Anulează  ",new DelegateCommand(OnCancelAdd),ImagePath.CancelIconPath);
            SaveBtn = new ActionButtonViewModel("Afișează  ", new DelegateCommand(OnSave), ImagePath.ShowFilters);
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();  
            InitLocalGroups();
            PopulateFilterGroups();
        }

        #endregion Constructor

        #region Properties

        public FIltersScreenViewModel Parent { get; set; }

        private readonly IEventAggregator eventAggregator;

        private ObjectList<FiltersViewModel> filtersViewModels;
        public ObjectList<FiltersViewModel> FiltersViewModels
        {
            get { return filtersViewModels; }
            set
            {
                if (filtersViewModels != value)
                {
                    filtersViewModels = value;
                    OnPropertyChanged();
                }
            }
        }

        private ActionButtonViewModel cancelGroupButton;

        public ActionButtonViewModel CancelGroupButton
        {
            get { return cancelGroupButton; }
            set
            {
                if (value == cancelGroupButton)
                    return;
                cancelGroupButton = value;
                OnPropertyChanged();
            }
        }

        private string groupName;
        public string GroupName
        {
            get { return groupName; }
            set
            {
                if (groupName != value)
                {
                    groupName = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility addGroupTileVisibility=Visibility.Collapsed;

        public Visibility AddGroupTileVisibility
        {
            get { return addGroupTileVisibility; }
            set
            {
                if (value == addGroupTileVisibility)
                    return;
                addGroupTileVisibility = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel addGroupButton;
        public ActionButtonViewModel AddGroupButton
        {
            get { return addGroupButton; }
            set
            {
                if (addGroupButton == value)
                    return;
                addGroupButton = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel saveGroupButton;
        public ActionButtonViewModel SaveGroupButton
        {
            get { return saveGroupButton; }
            set
            {
                if (saveGroupButton == value)
                    return;
                saveGroupButton = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel saveBtn;
        public ActionButtonViewModel SaveBtn
        {
            get { return saveBtn; }
            set
            {
                if (saveBtn == value)
                    return;
                saveBtn = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        private void OnSaveGroup()
        {
            FiltersViewModels.Add(new FiltersViewModel()
            {
                GroupName = GroupName
            });
            GroupName = "";
            AddGroupTileVisibility=Visibility.Collapsed;
        }

        private void OnAddNewGroup()
        {
            AddGroupTileVisibility=Visibility.Visible;
        }

        private async void OnSave()
        {
            Parent.MetroLoadingViewModel=new MetroLoadingViewModel(true);
            var localFilterGroupEdited = new List<LocalFilterGroup>();
            var editedGroups = FiltersViewModels.List.Where(item => item.IsChanged);
            foreach (var filtersViewModel in editedGroups)
            {
               // DatabaseHandler.Instance.PopulateTypes();
                var localFilters = filtersViewModel.ToLocalFilterGroup();
                localFilters = await DatabaseHandler.Instance.SaveEditFilterGroup(localFilters);
                filtersViewModel.GroupId = localFilters.Id;                
                filtersViewModel.PopulateFilterItems();
                filtersViewModel.SetFilterGroup(localFilters);
                localFilterGroupEdited.Add(localFilters);
                filtersViewModel.HasChanged(false);
            }

            var localFilterGroup = new List<LocalFilterGroup>();
            foreach (var filtersViewModel in FiltersViewModels.List)
            {
                localFilterGroup.Add(filtersViewModel.ToLocalFilterGroup());
            }

            LocalCache.Instance.FilterGroups = localFilterGroup;
            LocalCache.Instance.FilteredInterventionDetails = new List<InterventionDetails>();
            eventAggregator.GetEvent<ReportsInterventionsEvent>().Publish(null);            
            //eventAggregator.GetEvent<FiltersAppliedEvent>().Publish(null);
            Parent.FilterInterventions();
            Parent.MetroLoadingViewModel.ShouldContinueAnimation = false;
        }

//        private List<LocalFilterGroup> localfilterGroups;

        public async void InitLocalGroups()
        {
            LocalCache.Instance.LocalfilterGroups = await DatabaseHandler.Instance.GetFilterGroups();
        }

        public void PopulateFilterGroups()
        {
        //    var localGroups = await DatabaseHandler.Instance.GetFilterGroups();
            foreach (var localFilterGroup in LocalCache.Instance.LocalfilterGroups)
            {
                var newFilterVIewModel = new FiltersViewModel();
                newFilterVIewModel.SetFilterGroup(localFilterGroup);
                FiltersViewModels.Add(newFilterVIewModel);
            }
        }

        private void OnCancelAdd()
        {
            AddGroupTileVisibility=Visibility.Collapsed;            
        }

        #endregion Methods

    }
}
