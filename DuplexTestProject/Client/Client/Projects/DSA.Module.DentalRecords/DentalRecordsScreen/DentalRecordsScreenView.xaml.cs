using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.Prism.Regions.ViewsInterfaces;

namespace DSA.Module.DentalRecords.DentalRecordsScreen
{
    /// <summary>
    /// Interaction logic for DentalRecordsScreenView.xaml
    /// </summary>
    public partial class DentalRecordsScreenView : UserControl, IWorkAreaView
    {
        public DentalRecordsScreenView()
        {
            InitializeComponent();
            DataContext = new DentalRecordsScreeViewModel();
        }

        public ShellState RegionToken { get; set; }

        private void DentalRecordsScreenView_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.K)
            {
                ((DentalRecordsScreeViewModel)DataContext).SetPayedUnpayed();
            }
        }
    }
}
