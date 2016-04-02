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
using DSA.Client.Shell.Bootstrapper;

namespace DSA.Client.Shell.Views.Shell
{
    /// <summary>
    /// Interaction logic for ClientShellView.xaml
    /// </summary>
    public partial class ClientShellView : Window
    {
        public ClientShellView(RegionHandlers regionHandlers, ClientShellViewModel activeClientShellViewModel)
        {
            InitializeComponent();
            DataContext = activeClientShellViewModel;
        }
    }
}
