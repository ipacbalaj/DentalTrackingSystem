using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Controls.Buttons.SymbolButton;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Module.PersonalData.SettingsColumns.SettingsTileList;
using DSA.Module.PersonalData.SettingsColumns.Works.WorkTile.WorkTypeTile;
using DSA.Module.PersonalData.SettingsColumns.Works.WorkTile.WorkTypeTileList;
using DSA.Module.PersonalData.SettingsColumns.Works.WorkTileList;
using Microsoft.Practices.Prism.Commands;

namespace DSA.Module.PersonalData.SettingsColumns.Works.WorkTile
{
    public class WorkTileViewModel : ViewModelBase
    {

        #region Constructor

        public WorkTileViewModel(WorkTileListViewModel parent)
        {
            Parent = parent;
            DeleteBtn = new SymbolIconButtonViewModel(new DelegateCommand(OnDelete), ViewConstants.DeleteSymbol);
            EditButton = new SymbolIconButtonViewModel(new DelegateCommand(OnEdit), ViewConstants.EditSymbol);
            // SaveBtn = new SymbolIconButtonViewModel(new DelegateCommand(OnSave), ViewConstants.SaveSymbol);
        }

        #endregion Constructor

        #region Properties

        public WorkTileListViewModel Parent { get; set; }

        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool includedINFinancial;

        public bool IncludedINFinancial
        {
            get { return includedINFinancial; }
            set
            {
                if (value == includedINFinancial)
                    return;
                includedINFinancial = value;
                WasChanged = true;
                OnPropertyChanged();
            }
        }

        private LocalCache.SettingsItemEnum settingsItemType;
        public LocalCache.SettingsItemEnum SettingsItemType
        {
            get { return settingsItemType; }
            set
            {
                if (value == settingsItemType)
                    return;
                settingsItemType = value;
                OnPropertyChanged();
            }
        }

        private Visibility editDisplayVisibility = Visibility.Collapsed;
        public Visibility EditDisplayVisibility
        {
            get { return editDisplayVisibility; }
            set
            {
                if (value == editDisplayVisibility)
                    return;
                editDisplayVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility normalDisplayVisibility = Visibility.Visible;
        public Visibility NormalDisplayVisibility
        {
            get { return normalDisplayVisibility; }
            set
            {
                if (value == normalDisplayVisibility)
                    return;
                normalDisplayVisibility = value;
                OnPropertyChanged();
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string extraText;
        public string ExtraText
        {
            get { return extraText; }
            set
            {
                if (value == extraText)
                    return;
                extraText = value;
                OnPropertyChanged();
            }
        }

        private string typeName;
        public string TypeName
        {
            get { return typeName; }
            set
            {
                if (value == typeName)
                    return;
                typeName = value;
                OnPropertyChanged();
            }
        }

        private double? cost;
        public double? Cost
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

        private double? percent;

        public double? Percent
        {
            get { return percent; }
            set
            {
                if (value == percent)
                    return;
                percent = value;
                OnPropertyChanged();
            }
        }


        //        private bool hasCost;
        //        public bool HasCost
        //        {
        //            get { return hasCost; }
        //            set
        //            {
        //                if (value == hasCost)
        //                    return;
        //                hasCost = value;
        //                if(hasCost)CostVisibility=Visibility.Visible;
        //                OnPropertyChanged();
        //            }
        //        }

        private Visibility costVisibility = Visibility.Collapsed;
        public Visibility CostVisibility
        {
            get { return costVisibility; }
            set
            {
                if (value == costVisibility)
                    return;
                costVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility deleteBtnVisibility = Visibility.Collapsed;
        public Visibility DeleteBtnVisibility
        {
            get { return deleteBtnVisibility; }
            set
            {
                if (deleteBtnVisibility != value)
                {
                    deleteBtnVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private Brush contentBackground = BackgroundColors.MonthTileColor;
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

        private string normalViewHeight = ViewConstants.DimenstionStar;

        public string NormalViewHeight
        {
            get { return normalViewHeight; }
            set
            {
                if (value == normalViewHeight)
                    return;
                normalViewHeight = value;
                OnPropertyChanged();
            }
        }

        private string editViewHeight = ViewConstants.DimenstionAuto;
        public string EditViewHeight
        {
            get { return editViewHeight; }
            set
            {
                if (value == editViewHeight)
                    return;
                editViewHeight = value;
                OnPropertyChanged();
            }
        }

        private SymbolIconButtonViewModel saveBtn;
        public SymbolIconButtonViewModel SaveBtn
        {
            get { return saveBtn; }
            set
            {
                if (value == saveBtn)
                    return;
                saveBtn = value;
                OnPropertyChanged();
            }
        }

        private Visibility isWorkExpanded = Visibility.Collapsed;
        public Visibility IsWorkExpanded
        {
            get { return isWorkExpanded; }
            set
            {
                if (value == isWorkExpanded)
                    return;
                isWorkExpanded = value;
                OnPropertyChanged();
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

        private bool isExpanded;

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (value == isExpanded)
                    return;
                isExpanded = value;
                OnPropertyChanged();
            }
        }

        private bool wasChanged;

        public bool WasChanged
        {
            get { return wasChanged; }
            set
            {
                wasChanged = value;
                Parent.WasChanged = wasChanged;
                OnPropertyChanged();
            }
        }

        private WorkTypeTileListViewModel workTypeTileList;

        public WorkTypeTileListViewModel WorkTypeTileList
        {
            get { return workTypeTileList; }
            set
            {
                if (value == workTypeTileList)
                    return;
                workTypeTileList = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public void MouseOn()
        {
            ContentBackGround = BackgroundColors.MonthTileColorMouseOver;
            DeleteBtnVisibility = Visibility.Visible;

        }

        public void MouserOff()
        {
            ContentBackGround = BackgroundColors.MonthTileColor;
            DeleteBtnVisibility = Visibility.Collapsed;
        }

        public void OnDelete()
        {
            Parent.RemoveItem(this);
        }

        public void OnEdit()
        {
            // Parent.EditSettingsTileViewModel.SetData(this.Name, 0, Id);
            NormalViewHeight = ViewConstants.DimenstionAuto;
            EditViewHeight = ViewConstants.DimenstionStar;
            NormalDisplayVisibility = Visibility.Collapsed;
            EditDisplayVisibility = Visibility.Visible;
            WasChanged = true;
        }

        public void OnCloseOrExpandWorkTypes()
        {
            if (IsExpanded)
            {
                IsExpanded = false;
                ExpanderRotation = "90";
                IsWorkExpanded = Visibility.Collapsed;
            }
            else
            {
                IsExpanded = true;
                IsWorkExpanded = Visibility.Visible;
                ExpanderRotation = "270";
            }
        }

        public void OnSave()
        {
            NormalViewHeight = ViewConstants.DimenstionStar;
            EditViewHeight = ViewConstants.DimenstionAuto;
            NormalDisplayVisibility = Visibility.Visible;
            EditDisplayVisibility = Visibility.Collapsed;
            if(WorkTypeTileList!=null)
            foreach (var workTypeTileViewModel in WorkTypeTileList.WorkTypes)
            {
                workTypeTileViewModel.OnSave();
            }
            WasChanged = false;
        }

        #endregion Methods


    }
}
