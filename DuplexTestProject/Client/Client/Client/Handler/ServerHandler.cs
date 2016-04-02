using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading.Tasks;
using Client.DualService;
using Client.Views.MainWindow;

namespace Client.Handler
{
    public class ServerHandler : IDuplexServiceCallback
    {
        private readonly DuplexServiceClient serverModel;
        private Guid clientID;

        public ServerHandler()
         {
            InstanceContext context=new InstanceContext(this);
            serverModel = new DualService.DuplexServiceClient(context, "WSDualHttpBinding_IDuplexService");
         }

        private static readonly ServerHandler instance = new ServerHandler();
        public static ServerHandler Instance
        {
            get { return instance; }
        }

        public void GetData()
        {
            cacheData = serverModel.GetData();
        }

        public async Task<String> Subscribe()
        {
            try
            {
                clientID = await serverModel.SubscribeAsync("TestMachineName");
                return "Connected to Server!";
            }
            catch (Exception e)
            {
                return "Could not connect to server! Err: " + e.Message;
            }
        }

        public async void Unsubscribe()
        {
            try
            {
                if (serverModel.State == CommunicationState.Opened)
                {
                    await serverModel.UnsubscribeAsync(clientID);
                    serverModel.Close();
                }
            }
            catch (Exception)
            {
                serverModel.Abort();
            }
        }

        public List<string> cacheData; 

        public void TestCallback(List<string> testData)
        {
            MainWindowViewModel.Instance.Collection = new ObservableCollection<string>(testData);
        }

    }
}
