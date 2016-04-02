using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using DSA.Database.Model;
using DSA.Database.Model.Entities;
using DSA.Database.Model.Entities.SettingsEntities;
using ServiceLib.Client;

namespace ServiceLib.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DuplexService : IDuplexService
    {
        #region Clients

        private readonly Dictionary<ClientInfo, IClientCallback> clients = new Dictionary<ClientInfo, IClientCallback>();

        public List<string> GetData()
        {
            return Cache.Cache.Instance.Data;
        }

        public Guid Subscribe(string machineName)
        {
            RemoveClientForMachine(machineName);

            IClientCallback callback = OperationContext.Current.GetCallbackChannel<IClientCallback>();
            ClientInfo clientInfo = new ClientInfo(machineName);

            if (callback != null)
            {
                lock (clients)
                {
                    clients.Add(clientInfo, callback);
                }
            }
            return clientInfo.ID;
        }

        /// <summary>
        /// Removes the client for machine.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        private void RemoveClientForMachine(string machineName)
        {
            ClientInfo clientInfo = clients.Keys.SingleOrDefault(info => info.MachineName == machineName);
            if (clientInfo != null)
            {
                Unsubscribe(clientInfo.ID);
            }
        }

        public bool Unsubscribe(Guid clientID)
        {
            lock (clients)
            {
                try
                {
                    ClientInfo clientInfo = new ClientInfo(clientID);
                    if (clients.ContainsKey(clientInfo))
                    {
                        clients.Remove(clientInfo);
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public Dictionary<ClientInfo, IClientCallback> GetClients()
        {
            return clients;
        }

        #endregion Clients       

        #region Patients

        public List<LocalPatient> GetLocalPatients()
        {
            return DatabaseHandler.Instance.GetPatients();
        }

        public int AddPatient(LocalPatient localPatient)
        {
         return  DatabaseHandler.Instance.AddPatient(localPatient);
        }

        #endregion Patients        
        

        #region Weeks

        #endregion Weeks
        public List<LocalArea> GetAreas()
        {
            return DatabaseHandler.Instance.GetAreas();
        }

        public List<LocalWork> GetWorks()
        {
            return DatabaseHandler.Instance.GetWorks();
        }

        public List<LocalMaterial> GetMaterials()
        {
            return DatabaseHandler.Instance.GetMaterials();
        }

        public List<LocalLocation> GetLocations()
        {
            return DatabaseHandler.Instance.GetlLocations();
        }
    }
}
