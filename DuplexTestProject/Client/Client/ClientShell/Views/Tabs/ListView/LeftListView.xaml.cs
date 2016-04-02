using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Xpf.Grid;

namespace ClientShell.Views.Tabs.ListView
{
    /// <summary>
    /// Interaction logic for LeftListView.xaml
    /// </summary>
    public partial class LeftListView : UserControl
    {
        public LeftListView()
        {
            InitializeComponent();
            patiendsGRid.Columns["AllName"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
        }

        private void ViewPatientGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((LeftListViewModel)DataContext).OnPatientDoubleClick();
        }

        private void MergePat_OnRowDoubleClick(object sender, RowDoubleClickEventArgs e)
        {
            ((LeftListViewModel)DataContext).RemovePatientFromMergeList();
        }
    }
}
