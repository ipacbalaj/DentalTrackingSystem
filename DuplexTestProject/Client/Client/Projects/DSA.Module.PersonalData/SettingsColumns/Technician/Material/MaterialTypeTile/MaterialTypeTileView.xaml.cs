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

namespace DSA.Module.PersonalData.SettingsColumns.Technician.Material.MaterialTypeTile
{
    /// <summary>
    /// Interaction logic for MaterialTypeTileView.xaml
    /// </summary>
    public partial class MaterialTypeTileView : UserControl
    {
        public MaterialTypeTileView()
        {
            InitializeComponent();
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            
        }

        private void Name_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((MaterialTypeTileViewModel)DataContext).OnNameClick();
        }

        private void Cost_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((MaterialTypeTileViewModel)DataContext).OnCostClick();
        }

        private void deleteIcon_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((MaterialTypeTileViewModel)DataContext).DeleteIconMouseClick();
        }

        private void deleteIcon_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ((MaterialTypeTileViewModel)DataContext).DeleteIconMouseEnter();
        }

        private void deleteIcon_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!DataContext.ToString().Equals("{DisconnectedItem}"))
            {
                ((MaterialTypeTileViewModel)DataContext).DeleteIconMouseLeave();
            }
        }
    }
}
