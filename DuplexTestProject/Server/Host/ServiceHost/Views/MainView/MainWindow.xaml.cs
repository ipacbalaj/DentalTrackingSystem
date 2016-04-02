using System.Windows;

namespace ServiceHost.Views.MainView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void StartHost_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).Start();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).SendDataToClients();
        }

        private void LoadExcelData(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).LoadDataFromExcel();
        }

        private void LoadExcelSettings(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).LoadExcelSettings();
        }
    }
}
