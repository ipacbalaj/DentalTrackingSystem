using System.Windows.Controls;
using System.Windows.Input;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.Prism.Regions.ViewsInterfaces;

namespace DSA.Scheduler.Module.SchedulerScreen
{
    /// <summary>
    /// Interaction logic for SchedulerScreenView.xaml
    /// </summary>
    public partial class SchedulerScreenView : UserControl,IWorkAreaView
    {
        public SchedulerScreenView()
        {
            InitializeComponent();
            DataContext = new SchedulerScreenViewModel();
        }

        public ShellState RegionToken { get; set; }

        private void SchedulerScreenView_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    ((SchedulerScreenViewModel)DataContext).OnPrev();
                    break;
                case Key.Right:
                    ((SchedulerScreenViewModel)DataContext).OnNext();
                    break;
                default:break;
            }
        }
    }
}
