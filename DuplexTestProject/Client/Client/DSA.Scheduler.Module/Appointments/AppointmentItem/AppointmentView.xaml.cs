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

namespace DSA.Scheduler.Module.Appointments.AppointmentItem
{
    /// <summary>
    /// Interaction logic for AppointmentView.xaml
    /// </summary>
    public partial class AppointmentView : UserControl
    {
        public AppointmentView()
        {
            InitializeComponent();
        }

        private void OnAppointmentClick(object sender, MouseButtonEventArgs e)
        {
            AppointmentViewModel appointmentViewModel = (AppointmentViewModel)DataContext;
            appointmentViewModel.OnSelected(appointmentViewModel);
        }

        private void MouseOver(object sender, MouseEventArgs e)
        {
            ((AppointmentViewModel)DataContext).MouseEnter();
        }

        private void MouseLeave(object sender, MouseEventArgs e)
        {
            ((AppointmentViewModel)DataContext).MouserLeave();
        }
    }
}
