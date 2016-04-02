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

namespace DSA.Module.PersonalData.SettingsColumns.Technician.TechnicianTile
{
    /// <summary>
    /// Interaction logic for TechnicianTileView.xaml
    /// </summary>
    public partial class TechnicianTileView : UserControl
    {
        public TechnicianTileView()
        {
            InitializeComponent();
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ((TechnicianTileViewModel)DataContext).MouseOn();
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            ((TechnicianTileViewModel)DataContext).MouseOff();
        }

        private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TechnicianTileViewModel technicianTileViewModel = (TechnicianTileViewModel)DataContext;
            technicianTileViewModel.OnSelected(technicianTileViewModel);
        }
    }
}
