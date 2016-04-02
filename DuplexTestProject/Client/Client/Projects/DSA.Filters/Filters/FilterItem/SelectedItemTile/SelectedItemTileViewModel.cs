using System.Windows;
using System.Windows.Media;
using DSA.Common.Controls.Buttons;
using DSA.Common.Infrastructure;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model.Entities.Filters;
using Microsoft.Practices.Prism.Commands;

namespace DSA.Filters.Filters.FilterItem.SelectedItemTile
{
    public class SelectedItemTileViewModel:ViewModelBase
    {
        #region Constructor

        public SelectedItemTileViewModel(FilterItemViewModel parent)
        {
            Parent = parent;
            DeleteBtn = new ActionButtonViewModel("X", new DelegateCommand(OnDelete), ImagePath.CancelIconPath);            
        }

        #endregion Constructor

        #region Properties

        public FilterItemViewModel Parent { get; set; }

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

        private string symbol = "✔ ";

        public string Symbol
        {
            get { return symbol; }
            set
            {
                if (value == symbol)
                    return;
                symbol = value;
                OnPropertyChanged();
            }
        }

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (value == isChecked)
                    return;
                isChecked = value;
                OnPropertyChanged();
            }
        }

        private Visibility deleteBtnVisibility=Visibility.Collapsed;
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

        private Brush foregroundColor = BackgroundColors.green;
        public Brush ForegroundColor
        {
            get { return foregroundColor; }
            set
            {
                if (value == foregroundColor)
                    return;
                foregroundColor = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel deleteBtn;
        public ActionButtonViewModel DeleteBtn
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

        #endregion Properties

        #region Methods

        public void OnDelete()
        {
            Parent.RemoveItem(this);
        }

        public void MouseOn()
        {
            ContentBackGround = BackgroundColors.MonthTileColorMouseOver;
            DeleteBtnVisibility=Visibility.Visible;
            ForegroundColor = BackgroundColors.red;
            Symbol = "✘ ";
        }

        public void MouserOff()
        {
            ContentBackGround = BackgroundColors.MonthTileColor;
            DeleteBtnVisibility=Visibility.Collapsed;            
            ForegroundColor = BackgroundColors.green;
            Symbol = "✔ ";
        }

        #endregion Methods

    }
}
