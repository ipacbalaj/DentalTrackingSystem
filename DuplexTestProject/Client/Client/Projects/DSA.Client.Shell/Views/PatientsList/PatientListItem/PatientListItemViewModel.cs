using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;

namespace ClientShell.Views.PatientsList.PatientListItem
{
    public class PatientListItemViewModel : BindableObjectListBase<PatientListItemViewModel>
    {
        #region Constructor

        public PatientListItemViewModel(PatientListViewModel parent)
        {
            Parent = parent;
          
        }

        #endregion Constructor          

        #region Properties

        private Visibility iconVisibility=Visibility.Collapsed;

        public Visibility IconVisibility
        {
            get { return iconVisibility; }
            set
            {
                if (value == iconVisibility)
                    return;
                iconVisibility = value;
                OnPropertyChanged();
            }
        }

        private Thickness borderThickness=new Thickness(0,0,1.5,0);

        public Thickness BorderThickness
        {
            get { return borderThickness; }
            set
            {
                if (value == borderThickness)
                    return;
                borderThickness = value;
                OnPropertyChanged();
            }
        }

        private Thickness rightBorderThickness = new Thickness(0, 0, 0, 0);
        public Thickness RightBorderThickness
        {
            get { return rightBorderThickness; }
            set
            {
                if (value == rightBorderThickness)
                    return;
                rightBorderThickness = value;
                OnPropertyChanged();
            }
        }

        public PatientListViewModel Parent { get; set; }

        private string patientName;

        private int id;

        public int Id
        {
            get { return id; }
            set
            {
                if (value == id)
                    return;
                id = value;
                OnPropertyChanged();
            }
        }

        public string PatientName
        {
            get { return patientName; }
            set
            {
                if (value == patientName)
                    return;
                patientName = value;
                OnPropertyChanged();
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

        #endregion Properties

        #region Methods



        public override void OnClick(bool shouldSelect)
        {
            IsSelected = shouldSelect;
            if (shouldSelect)
            {                
                IconVisibility = Visibility.Visible;
                ContentBackGround = BackgroundColors.orange;
                BorderThickness = new Thickness(1.5, 1.5, 0, 1.5);
                RightBorderThickness = new Thickness(0, 0, 2, 0);      
                Parent.SetSelectedPatient(this.Id);
            }
            else
            {
                IconVisibility=Visibility.Collapsed;
                BorderThickness = new Thickness(0, 0, 1.5, 0);
                RightBorderThickness = new Thickness(0, 0, 0, 0);
                ContentBackGround = BackgroundColors.MonthTileColor;
            }
            //IsSelected = shouldSelect;
        }

        public void MouseOn()
        {
            if (!IsSelected)
            {
                ContentBackGround = BackgroundColors.MonthTileColorMouseOver;
            }
        }

        public void MouseOff()
        {
            if (!IsSelected)
            {
                ContentBackGround = BackgroundColors.MonthTileColor;
            }
        }

        #endregion Methods 
        
    }
}
