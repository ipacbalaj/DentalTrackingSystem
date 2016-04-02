using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.Prism.Regions;
using DSA.Common.Infrastructure.Prism.Regions.ViewsInterfaces;
using DSA.Module.DentalRecords.DentalRecordsScreen;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DSA.Module.DentalRecords
{
    public class DentalRecordsInit:IModule
    {
        #region Constructor
        [InjectionConstructor]
        public DentalRecordsInit(IUnityContainer unityContainer, IRegionManager regionManager)
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
            if (unityContainer == null) return;
            unityContainer.RegisterInstance(typeof(DentalRecordsScreeViewModel));
            unityContainer.RegisterType<IWorkAreaView, DentalRecordsScreenView>(ShellState.DentalRecords.ToString());
            RegisterMeWithRegions.RegisterWithRegionAndState<IWorkAreaView>(MainScreenRegions.WorkRegion, ShellState.DentalRecords);
        }
        #endregion Methods
    }
}
