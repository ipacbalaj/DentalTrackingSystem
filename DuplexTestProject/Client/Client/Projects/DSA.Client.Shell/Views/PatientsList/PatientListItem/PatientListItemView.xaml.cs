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

namespace ClientShell.Views.PatientsList.PatientListItem
{
    /// <summary>
    /// Interaction logic for PatientListItemView.xaml
    /// </summary>
    public partial class PatientListItemView : UserControl
    {
        public PatientListItemView()
        {
            InitializeComponent();
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            ((PatientListItemViewModel)DataContext).MouseOn();
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!DataContext.ToString().Equals("{DisconnectedItem}"))
            {
                ((PatientListItemViewModel)DataContext).MouseOff();
            }
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PatientListItemViewModel patientListItemViewModel = (PatientListItemViewModel)DataContext;
            patientListItemViewModel.OnSelected(patientListItemViewModel);
        }
    }
}
