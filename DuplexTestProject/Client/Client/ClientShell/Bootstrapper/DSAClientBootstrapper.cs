using System.Windows;
using ClientShell.Views.Shell;
using DSA.Filters;
using DSA.Financial;
using DSA.Financial.FinancialScreen;
using DSA.Login;
using DSA.Module.DentalRecords;
using DSA.Module.PersonalData;
using DSA.Module.PersonalInfo;
using DSA.Reports;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;

namespace ClientShell.Bootstrapper
{
    public class DSAClientBootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return ServiceLocator.Current.GetInstance<ClientShellView>();
        }
        protected override void InitializeShell()
        {
            base.InitializeShell();
            Application.Current.MainWindow = (Window) Shell;
            Application.Current.MainWindow.Show();
        }

        /// <summary>
        /// Add the modules that we use in the Catalog in order to be accesed when needed.
        /// Configures the <see cref="T:Microsoft.Practices.Prism.Modularity.IModuleCatalog" /> used by Prism.
        /// </summary>
        protected override void ConfigureModuleCatalog()
        {
            //Login module
            var myLogin = typeof(LoginModuleInit);
            ModuleCatalog.AddModule(new ModuleInfo(myLogin.Name, myLogin.AssemblyQualifiedName));
//            var myScheduler = typeof(SchedulerModuleInit);
//            ModuleCatalog.AddModule(new ModuleInfo(myScheduler.Name, myScheduler.AssemblyQualifiedName));
            var myDentalRec  = typeof(DentalRecordsInit);
            ModuleCatalog.AddModule(new ModuleInfo(myDentalRec.Name, myDentalRec.AssemblyQualifiedName));
            var mySettings = typeof(SettingsInit);
            ModuleCatalog.AddModule(new ModuleInfo(mySettings.Name, mySettings.AssemblyQualifiedName));
            var myFilters = typeof(FiltersInit);
            ModuleCatalog.AddModule(new ModuleInfo(myFilters.Name, myFilters.AssemblyQualifiedName));
//            var myReports = typeof (ReportsInit);
//            ModuleCatalog.AddModule(new ModuleInfo(myReports.Name,myReports.AssemblyQualifiedName));
            var myPersonalInfo = typeof(PersonalInfoInit);
            ModuleCatalog.AddModule(new ModuleInfo(myPersonalInfo.Name, myPersonalInfo.AssemblyQualifiedName));
            var myFinancial = typeof(FinancialInit);
            ModuleCatalog.AddModule(new ModuleInfo(myFinancial.Name, myFinancial.AssemblyQualifiedName));
            
        }
    }
}
