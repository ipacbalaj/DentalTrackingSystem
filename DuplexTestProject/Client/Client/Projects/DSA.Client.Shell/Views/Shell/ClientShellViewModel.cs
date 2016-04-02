using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Client.Shell.Views.Tabs.ListView;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Subscription.Service;
using DSA.Subscription.Service.Repositories;

namespace DSA.Client.Shell.Views.Shell
{
    public class ClientShellViewModel:ViewModelBase
    {
        #region Properties

        private LeftListViewModel listViewModel;
        public LeftListViewModel ListViewModel
        {
            get { return listViewModel; }
            set
            {
                if (listViewModel == value)
                    return;
                listViewModel = value;
                OnPropertyChanged();
            }
        }

        public ServiceHandler ServiceHandler;

        #endregion Properties

        #region Methods

        private async void OnUserLoggedIn()
        {
            LocalCache.Instance.PatientsRepository=new PatientsRepository();
            LocalCache.Instance.PatientsRepository.AddPatients(await ServiceHandler.GetPatients());
        }

        private void CreateService()
        {
            
        }

        #endregion Methods
    }
}
