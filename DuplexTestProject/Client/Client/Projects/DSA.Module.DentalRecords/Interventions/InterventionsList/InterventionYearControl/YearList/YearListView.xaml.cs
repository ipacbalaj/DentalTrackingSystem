using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DSA.Database.Model;

namespace DSA.Module.DentalRecords.Interventions.InterventionsList.InterventionYearControl.YearList
{
    /// <summary>
    /// Interaction logic for YearListView.xaml
    /// </summary>
    public partial class YearListView : UserControl
    {
        public YearListView()
        {
            InitializeComponent();
        }

        private void YearListView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            //Do work
            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                LocalCache.Instance.IntervalIntervention.IsShiftHold = true;
            }
            if (Keyboard.IsKeyUp(Key.LeftShift))
            {
                LocalCache.Instance.IntervalIntervention.IsShiftHold = false;
            }
        }

//                    var point = e.GetPosition(yearScrollViewer);
//            double scrollTarget = point.Y;
//            yearScrollViewer.ScrollToVerticalOffset(scrollTarget);
    }
}
