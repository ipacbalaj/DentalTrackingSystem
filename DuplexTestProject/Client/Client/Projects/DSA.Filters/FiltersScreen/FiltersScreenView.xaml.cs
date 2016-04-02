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

namespace DSA.Filters.FiltersScreen
{
    /// <summary>
    /// Interaction logic for FiltersScreenView.xaml
    /// </summary>
    public partial class FiltersScreenView : UserControl, IWorkAreaView
    {
        public FiltersScreenView()
        {
            InitializeComponent();
            DataContext = new FIltersScreenViewModel();
            ((FIltersScreenViewModel) DataContext).ViewReference = this;
            FilterstGrid.Columns["Date"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            FilterstGrid.Columns["Date"].Width = 75;            
            FilterstGrid.Columns["Location"].Width = 200;
            FilterstGrid.Columns["WorkAndType"].Width = 200;
            FilterstGrid.Columns["MaterialAndType"].Width = 100;
            FilterstGrid.Columns["Area"].Width = 200;
            FilterstGrid.Columns["Observations"].Width = 300;
            FilterstGrid.Columns["StartH"].Width = 50;
            FilterstGrid.Columns["EndH"].Width = 50;
            FilterstGrid.Columns["Durata"].Width = 50;
            FilterstGrid.Columns["Revenue"].Width = 50;
            FilterstGrid.Columns["Percent"].Width = 50;
            FilterstGrid.Columns["WasPayedByDental"].Width = 40;
        }

        public void BestFitColumns()
        {
            tableView12.BestFitColumns();
        }

        public ShellState RegionToken { get; set; }        
    }
}
