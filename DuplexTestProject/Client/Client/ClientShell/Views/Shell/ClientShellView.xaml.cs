using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ClientShell.Bootstrapper;
using DSA.Database.Model;

namespace ClientShell.Views.Shell
{
    /// <summary>
    /// Interaction logic for ClientShellView.xaml
    /// </summary>
    public partial class ClientShellView : Window
    {
        public ClientShellView(ClientShellViewModel activeClientShellViewModel)
        {
            InitializeComponent();
            DataContext = activeClientShellViewModel;

        }

        private async void ClientShellView_OnClosing(object sender, CancelEventArgs e)
        {
            LocalCache.Instance.SaveDatabase();
            await ((ClientShellViewModel)DataContext).SaveDatabaseFileToFtp();
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            //Do work
            if (Keyboard.IsKeyDown(Key.LeftShift))
            {

            }
        }
    }
}
