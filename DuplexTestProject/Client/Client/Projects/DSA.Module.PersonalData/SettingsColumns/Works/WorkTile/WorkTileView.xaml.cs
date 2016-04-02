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
using DSA.Module.PersonalData.SettingsColumns.SettingsTile;

namespace DSA.Module.PersonalData.SettingsColumns.Works.WorkTile
{
    /// <summary>
    /// Interaction logic for WorkTileView.xaml
    /// </summary>
    public partial class WorkTileView : UserControl
    {
        public WorkTileView()
        {
            InitializeComponent();
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((WorkTileViewModel)DataContext).OnCloseOrExpandWorkTypes();
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (!DataContext.ToString().Equals("{DisconnectedItem}"))
            {
                ((WorkTileViewModel)DataContext).MouseOn();
            }

        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!DataContext.ToString().Equals("{DisconnectedItem}"))
            {
                ((WorkTileViewModel)DataContext).MouserOff();
            }

        }
    }
}
