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

namespace DSA.Module.PersonalData.SettingsColumns.Technician.Material.MaterialTile
{
    /// <summary>
    /// Interaction logic for MaterialTileView.xaml
    /// </summary>
    public partial class MaterialTileView : UserControl
    {
        public MaterialTileView()
        {
            InitializeComponent();
        }

        private void nameTxt_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((MaterialTileViewModel)DataContext).OnNameClick();
        }

        private void costTxt_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((MaterialTileViewModel)DataContext).OnCostClick();
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ((MaterialTileViewModel)DataContext).MouseEnter();
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!DataContext.ToString().Equals("{DisconnectedItem}"))
            {
                ((MaterialTileViewModel)DataContext).MouseLeave();
            }
        }
    }
}
