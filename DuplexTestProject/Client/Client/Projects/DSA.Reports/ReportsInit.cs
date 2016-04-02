using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.Prism.Regions;
using DSA.Common.Infrastructure.Prism.Regions.ViewsInterfaces;
using DSA.Reports.ReportsScreen;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DSA.Reports
{
    public class ReportsInit : IModule
    {
        #region Constructor
        [InjectionConstructor]
        public ReportsInit(IUnityContainer unityContainer, IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            this.unityContainer = unityContainer;
        }
        #endregion Constructor

        #region Properties

        private readonly IUnityContainer unityContainer;
        private readonly IRegionManager regionManager;

        #endregion Properties

        #region Methods
        public void Initialize()
        {
            //            if (unityContainer == null) return;
            //            unityContainer.RegisterInstance(typeof(ReportsScreenViewModel));
            //            unityContainer.RegisterType<IWorkAreaView, ReportsScreenView>(ShellState.Reports.ToString());
            //            RegisterMeWithRegions.RegisterWithRegionAndState<IWorkAreaView>(MainScreenRegions.WorkRegion, ShellState.Reports);
        }
        #endregion Methods
    }
}
