using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Client.Handler;


namespace Client.Views.MainWindow
{
    public class MainWindowViewModel:ViewModelBase
    {

        private string updates;

        #region Constructor

        private MainWindowViewModel()
        {

            try
            {
                ConnectToServer();
            }
            catch
            {
                MessageBox.Show("Client Application could not connect to the server. Please make sure the server is up and running.", "Server Connection",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }

        #endregion

        #region Properties

        private static MainWindowViewModel instance = new MainWindowViewModel();
        public static MainWindowViewModel Instance
        {
            get { return instance; }
        }

        private ObservableCollection<string> collection = new ObservableCollection<string>();
        public ObservableCollection<string> Collection
        {
            get { return collection; }
            set
            {
                if (collection != value)
                {
                    collection = value;
                    OnPropertyChanged();
                }
            }
        }

        private string connectionStatus;
        public string ConnectionStatus
        {
            get { return connectionStatus; }
            set
            {
                connectionStatus = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        private async void ConnectToServer()
        {
            ConnectionStatus = "Connecting...";
            ConnectionStatus = await ServerHandler.Instance.Subscribe();
        }

        public void DisconnectFromServer()
        {
            ServerHandler.Instance.Unsubscribe();
        }

        #endregion
    }
}
