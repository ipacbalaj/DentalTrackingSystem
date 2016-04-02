using System.Windows.Controls;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.Prism.Regions.ViewsInterfaces;


namespace DSA.Reports.ReportsScreen
{
    /// <summary>
    /// Interaction logic for ReportsScreenView.xaml
    /// </summary>
    public partial class ReportsScreenView : UserControl, IWorkAreaView
    {
        public ReportsScreenView()
        {
            InitializeComponent();
            DataContext = new ReportsScreenViewModel();
        }

        public ShellState RegionToken { get; set; }
    }
}
