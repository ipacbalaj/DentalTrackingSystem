using System;
using System.Collections.Generic;
using System.ServiceModel;
using DSA.Database.Model.Entities;
using DSA.Database.Model.Entities.SettingsEntities;
using ServiceLib.Client;

namespace ServiceLib.Service
{
    [ServiceContract(CallbackContract = typeof(IClientCallback))]
    public interface IDuplexService
    {
        #region Clients

        [OperationContract(IsOneWay = false)]
        List<string> GetData();

        [OperationContract(IsOneWay = false)]
        Guid Subscribe(string machineName);

        [OperationContract]
        bool Unsubscribe(Guid clientID);

        #endregion Clients

        #region Patients


        [OperationContract(IsOneWay = false)]
        List<LocalPatient> GetLocalPatients();

        [OperationContract(IsOneWay = false)]
        int AddPatient(LocalPatient localPatient);
        #endregion Patients

        #region Settings
                [OperationContract(IsOneWay = false)]
        List<LocalArea> GetAreas();

        [OperationContract(IsOneWay = false)]
        List<LocalWork> GetWorks();

        [OperationContract(IsOneWay = false)]
        List<LocalMaterial> GetMaterials();

        [OperationContract(IsOneWay = false)]
        List<LocalLocation> GetLocations();
        #endregion Settings
    }
}
