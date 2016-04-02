using System.Windows.Controls;
using System.Windows.Input;

namespace DSA.Filters.Filters
{
    /// <summary>
    /// Interaction logic for FiltersView.xaml
    /// </summary>
    public partial class FiltersView : UserControl
    {
        public FiltersView()
        {
            InitializeComponent();
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ((FiltersViewModel)DataContext).MouseOn();
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!DataContext.ToString().Equals("{DisconnectedItem}"))
            {
                ((FiltersViewModel)DataContext).MouseOff();
            }
        }

        private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FiltersViewModel filtersViewModel = (FiltersViewModel)DataContext;
            filtersViewModel.OnSelected(filtersViewModel);
        }
    }
}
