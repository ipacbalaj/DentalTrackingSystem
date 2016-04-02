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

        private void ClientShellView_OnClosing(object sender, CancelEventArgs e)
        {
            LocalCache.Instance.SaveDatabase();
        }

        private void ClientShellView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;
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
