using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Infrastructure.Icos;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Module.DentalRecords.Tooth.ToothTileList;

namespace DSA.Module.DentalRecords.Tooth.ToothTile
{
    public class ToothTileViewModel:BindableObjectListBase<ToothTileViewModel>
    {
        #region Constructor

        public ToothTileViewModel(ToothTileListViewModel parent)
        {
            Parent = parent;
        }

        #endregion Constructor

        #region Properties

        public ToothTileListViewModel Parent { get; set; }

        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    SetImageIcon();
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

        private UIElement icon;
        public UIElement Icon
        {
            get { return icon; }
            set
            {
                if (icon != value)
                {
                    icon = value;
                    OnPropertyChanged();
                }
            }
        }

        private Brush contentBackground=BackgroundColors.whiteSmoke;
        public Brush ContentBackground
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

        #endregion Properties

        #region Methods
        public override void OnClick(bool shouldSelect)
        {
            IsSelected = shouldSelect;
            if (IsSelected)
            {
                ContentBackground = BackgroundColors.orange;
                //Command.Execute(null);
            }
            else
            {
                ContentBackground = BackgroundColors.gray;
            }
        }

        public void MouseOn()
        {
            if (!IsSelected)
            {
                ContentBackground = BackgroundColors.addInformationTitleCompleted;
            }
//            else
//            {
//                ContentBackground = BackgroundColors.whiteSmoke;
//            }
        }

        public void MouseOff()
        {
            if (!IsSelected)
            {
                ContentBackground = BackgroundColors.whiteSmoke;
            }
        }

        private void SetImageIcon()
        {
            switch (Id)
            {
                case 1:
                    Icon=new ToothTest();
                    break;
                case 2:
                    Icon = new Tooth1();
                    break;
                case 3:
                    Icon = new Tooth2();
                    break;
                case 4:
                    Icon = new Tooth3();
                    break;
                case 5:
                    Icon = new Tooth5();
                    break;
                case 6:
                    Icon = new Tooth4();
                    break;
                case 7:
                    Icon = new Tooth2();
                    break;
                case 8:
                    Icon = new Tooth3();
                    break;
                case 9:
                    Icon = new Tooth4();
                    break;
            }
        }

        #endregion Methods



    }
}
