using System.Windows.Controls;
using System.Windows.Input;

namespace DSA.Filters.Filters.FilterItem.SelectedItemTile
{
    /// <summary>
    /// Interaction logic for SelectedItemTileView.xaml
    /// </summary>
    public partial class SelectedItemTileView : UserControl
    {
        public SelectedItemTileView()
        {
            InitializeComponent();
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (!DataContext.ToString().Equals("{DisconnectedItem}"))
            {
                ((SelectedItemTileViewModel)DataContext).MouseOn();
            }

        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!DataContext.ToString().Equals("{DisconnectedItem}"))
            {
                ((SelectedItemTileViewModel)DataContext).MouserOff();
            }

        }

        private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((SelectedItemTileViewModel)DataContext).OnDelete();
        }
    }
}
