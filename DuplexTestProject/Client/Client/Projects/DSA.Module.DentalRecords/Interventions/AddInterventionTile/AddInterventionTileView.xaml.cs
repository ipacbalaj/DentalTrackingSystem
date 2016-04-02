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
using DevExpress.Xpf.Editors;
using DevExpress.Xpo.DB;

namespace DSA.Module.DentalRecords.Interventions.AddInterventionTile
{
    /// <summary>
    /// Interaction logic for AddInterventionTileView.xaml
    /// </summary>
    public partial class AddInterventionTileView : UserControl
    {
        public AddInterventionTileView()
        {
            InitializeComponent();
        }

        private void OnEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.RightCtrl ||e.Key==Key.LeftCtrl)
            {
                 ((AddInterventionTileViewModel)DataContext).Save();
            }
        }

     //   private int count = 1;

        private void UIElement_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource == PatientComboBoxEdit)
            {
                ComboBoxEdit editor = (ComboBoxEdit)e.Source;
                string displayText = editor.DisplayText;
                if (!string.IsNullOrEmpty(displayText))
                {
                    ((AddInterventionTileViewModel)DataContext).AddNewPatient(displayText);

                }
               // DurataTextBox.Text = "";
                //IncasariTextBox.Text = "";   
            }
        }
    }

    public static class FocusExtension
    {
        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }


        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }


        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.RegisterAttached(
             "IsFocused", typeof(bool), typeof(FocusExtension),
             new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));


        private static void OnIsFocusedPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var uie = (UIElement)d;
            if ((bool)e.NewValue)
            {
                uie.Focus(); // Don't care about false values.
            }
        }
    }
}
