using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.Prism.Regions.ViewsInterfaces;

namespace DSA.Module.PersonalInfo.PersonalInfoScreen
{
    /// <summary>
    /// Interaction logic for PersonalInfoScreenView.xaml
    /// </summary>
    public partial class PersonalInfoScreenView : UserControl,IWorkAreaView
    {
        public PersonalInfoScreenView()
        {
            InitializeComponent();
            DataContext = new PersonalInfoScreenViewModel();
            ((PersonalInfoScreenViewModel) DataContext).ViewReference = this;
        }

        public ShellState RegionToken { get; set; }

        private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
          //  ((PersonalInfoScreenViewModel)DataContext).OnShowInterventions();
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!DataContext.ToString().Equals("{DisconnectedItem}"))
            {
                ((PersonalInfoScreenViewModel)DataContext).MouseOff();
            }
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ((PersonalInfoScreenViewModel)DataContext).MouseOn();
        }

        public void BestFitColums()
        {
            tableView12.BestFitColumns();
        }
    }
}
