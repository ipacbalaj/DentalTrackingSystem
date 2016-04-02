using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using DevExpress.Data.Linq;
using DevExpress.Xpf.Grid;
using DSA.Database.Model;
using DSA.Database.Model.Entities;

namespace DSA.Module.DentalRecords.Interventions.InterventionsList.InterventionMonthControl
{
    /// <summary>
    /// Interaction logic for MonthInterventionsVIew.xaml
    /// </summary>
    public partial class MonthInterventionsVIew : UserControl
    {
        public MonthInterventionsVIew()
        {
            InitializeComponent();
            //TableView.ClearSelection();
            TableView.ClearSelection();            
            TableView.FocusedRowHandle = -1;
//            gridControlInt.Columns["Id"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
//            gridControlInt.Columns["Id"].Width = 50;
            gridControlInt.Columns["Date"].Width = 75;
            gridControlInt.Columns["PacientName"].Width = 200;
            gridControlInt.Columns["Location"].Width = 200;
            gridControlInt.Columns["WorkAndType"].Width = 200;
            gridControlInt.Columns["MaterialAndType"].Width = 100;
            gridControlInt.Columns["TechnicianName"].Width = 100;
            gridControlInt.Columns["Observations"].Width = 350;
            gridControlInt.Columns["StartH"].Width = 50;
            gridControlInt.Columns["StartH"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            gridControlInt.Columns["EndH"].Width = 50;
            gridControlInt.Columns["Area"].Width = 200;
            gridControlInt.Columns["Durata"].Width = 50;
            gridControlInt.Columns["Revenue"].Width = 50;
            gridControlInt.Columns["MaterialCost"].Width = 50;
            gridControlInt.Columns["Percent"].Width = 50;
            gridControlInt.Columns["WasPayedByDental"].Width = 40;
            gridControlInt.Columns["Delete"].Width = 40;     
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            ((MonthInterventionsViewModel)DataContext).MouseOn();
           // TableView.ClearSelection();
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!DataContext.ToString().Equals("{DisconnectedItem}"))
            {
                ((MonthInterventionsViewModel)DataContext).MouseOff();
            }
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MonthInterventionsViewModel monthInterventionsViewModel = (MonthInterventionsViewModel)DataContext;
            monthInterventionsViewModel.OnSelected(monthInterventionsViewModel);
            TableView.ClearSelection();  
//            TableView.BestFitColumns();
        }

        private void TableView12_OnCellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            TableView view = sender as TableView;
            var row = (InterventionDetails)view.FocusedRow;
            if (row!=null)
            {

                ((MonthInterventionsViewModel)DataContext).OnPaymentCellValueChanged((bool)e.Value,row.Id);
                row = null;
            }
        }

        private void TableView_OnRowDoubleClick(object sender, RowDoubleClickEventArgs e)
        {
            ((MonthInterventionsViewModel)DataContext).EditIntervention();
        }



        private void TableView_OnSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            //var selectedRows = e.Source.SelectedRows;
            if (DataContext != null)
            {
                ((MonthInterventionsViewModel)DataContext).SelectedInterv = e.Source.SelectedRows.Cast<InterventionDetails>().ToList();
            }
        }
    }
}
