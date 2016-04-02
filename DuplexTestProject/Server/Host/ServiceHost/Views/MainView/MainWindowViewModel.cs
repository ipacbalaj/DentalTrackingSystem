using System.Collections.Generic;
using System.ServiceModel;
using DSA.Database.Model.Helpers;
using ServiceLib.Service;

namespace ServiceHost.Views.MainView
{
    public class MainWindowViewModel:ViewModelBase
    {

        private System.ServiceModel.ServiceHost host;
        private DuplexService subscriptionService = new DuplexService();

        #region Properties

        private string log = "";
        public string Log
        {
            get { return log; }
            set{
                if (log != value)
                {
                    log = value;
                    OnPropertyChanged();
                }}
        }

        #endregion

        #region Methods

        

        /// <summary>
        /// Starts the service.
        /// </summary>
        public void Start()
        {
            host = new System.ServiceModel.ServiceHost(subscriptionService);
            host.Open();
            Log += "\r\n Host is Open";
        }

        /// <summary>
        /// Stops the service
        /// </summary>
        public void Stop()
        {
            if (host != null)
            {
                //subscriptionService.UnsubscribeAllClients();
                host.Close();
                host = null;
            }
        }

        #endregion

        public void SendDataToClients()
        {
            foreach (var client in subscriptionService.GetClients())
            {
                subscriptionService.GetClients()[client.Key].TestCallback(new List<string>(){"List1","List2","List3"});
            }
        }

        public  void LoadDataFromExcel()
        {
            ExcelParser.Instance.GetConstants(@"D:\c#\DTS\finalProject\Resources\GOL.xlsx");
        }

        public void LoadExcelSettings()
        {
            ExcelParser.Instance.SetSettings(@"D:\c#\DTS\finalProject\Resources\GOL.xlsx");
        }
    }
}
