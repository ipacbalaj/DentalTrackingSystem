using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Controls.Buttons.OptionButton;
using DSA.Common.Infrastructure.Icos;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using Microsoft.Practices.Prism.Commands;

namespace DSA.Filters.Filters.FilterItem.IntervalFilterItem
{
    public class IntervalFilterItemViewModel : ViewModelBase
    {
        #region Constructor

        public IntervalFilterItemViewModel()
        {
            SetIcons();
            IconButton = new OptionButtonViewModel()
            {
                OnClick = new DelegateCommand(OnIconClick),
                Icon = UncheckedIcon
            };
        }

        #endregion Constructor

        #region Properties

        private DateTime startDate = DateTime.Now;
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if (value == startDate)
                    return;
                startDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime endDate = DateTime.Now;
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                if (value == endDate)
                    return;
                endDate = value;
                OnPropertyChanged();
            }
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
                    if (isChecked)
                    {
                        IconButton.Icon = CheckedIcon;
                        ContentBackground = BackgroundColors.ActiveFilter;
                    }
                    else
                    {
                        IconButton.Icon = UncheckedIcon;
                        ContentBackground = BackgroundColors.InactiveFilter;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private OptionButtonViewModel iconButton;
        public OptionButtonViewModel IconButton
        {
            get { return iconButton; }
            set
            {
                if (value == iconButton)
                    return;
                iconButton = value;
                OnPropertyChanged();
            }
        }

        private UIElement checkedIcon;

        public UIElement CheckedIcon
        {
            get { return checkedIcon; }
            set
            {
                if (value == checkedIcon)
                    return;
                checkedIcon = value;
                OnPropertyChanged();
            }
        }

        private UIElement uncheckedIcon;

        public UIElement UncheckedIcon
        {
            get { return uncheckedIcon; }
            set
            {
                if (value == uncheckedIcon)
                    return;
                uncheckedIcon = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        private void OnIconClick()
        {
            IsChecked = !IsChecked;
        }

        private void SetIcons()
        {
            CheckedIcon = new CloseIcon();
            UncheckedIcon = new BlueCancelIcon();
        }

        private Brush contentBackground = BackgroundColors.InactiveFilter;

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

        #endregion Methods
    }
}
