using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Subscription.Service.DuplexService;
using DSA.Subscription.Service.Repositories;

namespace DSA.Subscription.Service
{
    public class LocalCache
    {
        #region instance
        private static readonly LocalCache instance = new LocalCache();
        public static LocalCache Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion instance

        #region Fields

        public ServiceHandler ServiceHandler;
        public LocalPatient SelectedPatient { get; set; }

        #endregion Fields

        public PatientsRepository PatientsRepository { get; set; }
        public WeekRepository WeekRepository { get; set; }

        public Dictionary<SettingsItemEnum, List<SettingsItem>> SettingsItems { get; set; } 

//        public List<LocalLocation> Locations { get; set; }
//        public List<LocalWork> Works { get; set; }
//        public List<LocalMaterial> Materials { get; set; }
//        public List<LocalArea> Areas { get; set; } 

        #region Methods

        public void CreateService()
        {
            ServiceHandler=new ServiceHandler();
        }

        public void InitRepositories()
        {
            PatientsRepository=new PatientsRepository();
            WeekRepository=new WeekRepository();

        }

        #endregion Methods

        public enum SettingsItemEnum { Locatie, Material, Lucrare, Zona,Luna,An };
    }
}
