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
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.Prism.Regions.ViewsInterfaces;

namespace DSA.Financial.FinancialScreen
{
    /// <summary>
    /// Interaction logic for FinancialScreenView.xaml
    /// </summary>
    public partial class FinancialScreenView : UserControl, IWorkAreaView
    {
        public FinancialScreenView()
        {
            InitializeComponent();
            DataContext = new FinancialScreenViewModel();
        }
        public ShellState RegionToken { get; set; }
    }
}
