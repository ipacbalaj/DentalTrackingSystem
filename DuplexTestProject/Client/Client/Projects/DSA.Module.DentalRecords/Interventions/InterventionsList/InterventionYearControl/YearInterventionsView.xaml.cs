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

namespace DSA.Module.DentalRecords.Interventions.InterventionsList.InterventionYearControl
{
    /// <summary>
    /// Interaction logic for YearInterventionsView.xaml
    /// </summary>
    public partial class YearInterventionsView : UserControl
    {
        public YearInterventionsView()
        {
            InitializeComponent();
        }


        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            ((YearInterventionsViewModel)DataContext).MouseOn();
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!DataContext.ToString().Equals("{DisconnectedItem}"))
            {
                ((YearInterventionsViewModel)DataContext).MouseOff();
            }
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            YearInterventionsViewModel yearInterventionsViewModel = (YearInterventionsViewModel)DataContext;
            yearInterventionsViewModel.OnSelected(yearInterventionsViewModel);

        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                if (e.Key == Key.Up || e.Key == Key.Down)
                    return;
            }
            base.OnKeyDown(e);
        }
    }
}
