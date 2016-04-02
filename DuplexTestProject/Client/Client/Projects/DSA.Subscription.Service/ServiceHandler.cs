using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DSA.Subscription.Service.DuplexService;

namespace DSA.Subscription.Service
{
    public class ServiceHandler:IDuplexServiceCallback
    {
        #region Constructor

        public ServiceHandler()
        {
            InstanceContext context = new InstanceContext(this);
            serverModel = new DuplexServiceClient(context, "WSDualHttpBinding_IDuplexService");
        }

        #endregion Constructor

        #region Fields

        private readonly DuplexServiceClient serverModel;

        #endregion Fields

        #region Properties

        private Guid clientID;
        public Guid ClientID
        {
            get { return clientID; }
        }

        #endregion Properties

        #region CallbackMethods

        public void TestCallback(List<string> testString)
        {
            throw new NotImplementedException();
        }

        #endregion CallbackMethods

        #region Methods

        public async Task<String> Subscribe()
        {
            try
            {
                clientID = await serverModel.SubscribeAsync(Environment.MachineName);
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

        #endregion Methods

        #region Patients

        public Task<List<LocalPatient>> GetPatients()
        {
          return serverModel.GetLocalPatientsAsync();
        }

        public Task<int> AddPatient(LocalPatient addedPatient)
        {
            return serverModel.AddPatientAsync(addedPatient);
        }

        public Task<int> AddAppointment(LocalAppointment localAppointment)
        {
          return  serverModel.AddAppointmetAsync(localAppointment);
        }

        #endregion Patients

        #region Weeks

        public Task<List<LocalWeek>> GetWeeks()
        {
            return serverModel.GetWeeksAsync();
        }
        #endregion Weeks

        #region Tooth

//        public Task<List<LocalTooth>> GetToothList()
//        {
//           return serverModel.GetTeethListAsync();
//        }

        #endregion Tooth

        #region InterventionDetails

        public Task<List<LocalArea>> GetAreas()
        {
            return serverModel.GetAreasAsync();
        }

        public Task<List<LocalLocation>> GetLocations()
        {
            return serverModel.GetLocationsAsync();
        }

        public Task<List<LocalMaterial>> GetMaterials()
        {
            return serverModel.GetMaterialsAsync();
        }

        public Task<List<LocalWork>> GetWorks()
        {
            return serverModel.GetWorksAsync();
        }
        #endregion InterventionDetails
    }
}
