using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Controls.Buttons;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.Entities.Filters;
using DSA.Database.Model.Entities.SettingsEntities;
using DSA.Filters.Filters.FilterItem;
using DSA.Filters.Filters.FilterItem.IntervalFilterItem;
using Microsoft.Practices.Prism.Commands;

namespace DSA.Filters.Filters
{
    public class FiltersViewModel : BindableObjectListBase<FiltersViewModel>
    {
        #region Constructor

        public FiltersViewModel( )
        {
            PopulateFilterItems(); 
            IntervalFilterItemViewModel=new IntervalFilterItemViewModel();
        }


        #endregion Constructor

        #region Properties
        
        private ObservableCollection<FilterItemViewModel> filterItemList;
        public ObservableCollection<FilterItemViewModel> FilterItemList
        {
            get { return filterItemList; }
            set
            {
                if (filterItemList != value)
                {
                    filterItemList = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isChanged=false;

        public bool IsChanged
        {
            get { return isChanged; }
            set
            {
                if (value == isChanged)
                    return;
                isChanged = value;
                OnPropertyChanged();
            }
        }

        private int groupId;

        public int GroupId
        {
            get { return groupId; }
            set
            {
                if (value == groupId)
                    return;
                groupId = value;
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

        private string expanderRotation = "270";
        public string ExpanderRotation
        {
            get { return expanderRotation; }
            set
            {
                if (expanderRotation != value)
                {
                    expanderRotation = value;
                    OnPropertyChanged();
                }
            }
        }

        private IntervalFilterItemViewModel intervalFilterItemViewModel;

        public IntervalFilterItemViewModel IntervalFilterItemViewModel
        {
            get { return intervalFilterItemViewModel; }
            set
            {
                if (value == intervalFilterItemViewModel)
                    return;
                intervalFilterItemViewModel = value;
                OnPropertyChanged();
            }
        }

        private Visibility contentVisibility = Visibility.Collapsed;
        public Visibility ContentVisibility
        {
            get { return contentVisibility; }
            set
            {
                if (contentVisibility != value)
                {
                    contentVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private Brush contentBackground = BackgroundColors.FiltreTitlu;
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

        private Visibility intervalVisibility =Visibility.Collapsed;

        public Visibility IntervalVisibility
        {
            get { return intervalVisibility; }
            set
            {
                if (value == intervalVisibility)
                    return;
                intervalVisibility = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public void PopulateFilterItems()
        {
             FilterItemList=new ObservableCollection<FilterItemViewModel>();
            foreach (var key in LocalCache.Instance.SettingsItems.Keys)
            {
                FilterItemList.Add(new FilterItemViewModel(this)
                {
                    SettingsItemName = key.ToString(),
                    SettingsItems = new ObservableCollection<SettingsItem>(LocalCache.Instance.SettingsItems[key])
                });
            }       
//            FilterItemList.Add(new FilterItemViewModel(this)
//            {
//                SettingsItemName = "Manopera",
//                SettingsItems = new ObservableCollection<SettingsItem>(LocalCache.Instance.Works)
//            });
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
                    OnPropertyChanged();
                    HasChanged(true);
                }
            }
        }

        public void HasChanged(bool hasChanged)
        {
            IsChanged = hasChanged;
        }

        public override void OnClick(bool shouldSelect)
        {
            if (shouldSelect)
            {
                ContentVisibility=Visibility.Visible;                
                ExpanderRotation = "90";
            }
            else
            {
                ContentVisibility=Visibility.Collapsed;
                ExpanderRotation = "270";
            }
        }

        public void MouseOn()
        {
            if (!IsSelected)
            {
                ContentBackGround = BackgroundColors.FiltreTitluMouseOn;
            }
        }

        public void MouseOff()
        {
            if (!IsSelected)
            {
                ContentBackGround = BackgroundColors.FiltreTitlu;
            }
        }

        public void SetFilterGroup(LocalFilterGroup localFilterGroup)
        {
            GroupId = localFilterGroup.Id;
            GroupName = localFilterGroup.Name;
            IsChecked = localFilterGroup.IsActive;
            foreach (var localFilter in localFilterGroup.Filters)
            {
                var filterItemModel = FilterItemList.FirstOrDefault(item => item.SettingsItemName == localFilter.Type);
                filterItemModel.SetFIlterItem(localFilter);
            }
            IntervalFilterItemViewModel.IsChecked = localFilterGroup.IsIntervalActive;
            IntervalFilterItemViewModel.StartDate = localFilterGroup.StartDate;
            IntervalFilterItemViewModel.EndDate = localFilterGroup.EndDate;
            HasChanged(false);
        }

        #endregion Methods
    }
}
