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

namespace DSA.Module.DentalRecords.Interventions.InterventionsGeneralDetails.BirthDays
{
    /// <summary>
    /// Interaction logic for BirthdaysView.xaml
    /// </summary>
    public partial class BirthdaysView : UserControl
    {
        public BirthdaysView()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
           
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ((BirthDaysViewModel)DataContext).MouseEnter();
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            ((BirthDaysViewModel)DataContext).MouseLeave();
        }
    }
}
