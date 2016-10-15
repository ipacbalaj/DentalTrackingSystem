using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ClientShell.Bootstrapper;
using ClientShell.Properties;
using log4net;

namespace ClientShell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnStartup(StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            Log.Info("Starting Dental Tracking System App...");
            base.OnStartup(e);
            var hostBootstrapper = new DSAClientBootstrapper();
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ro");        
            SetAppConfig();
            hostBootstrapper.Run();           
        }

        private void SetAppConfig()
        {
            var settings = ConfigurationManager.ConnectionStrings["dsaEntities"];
            var fi = typeof(ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            fi.SetValue(settings, false);
            var efConfigString =
                ConfigurationManager.ConnectionStrings["dsaEntities"].ConnectionString.Replace("AppData",Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DTS");
            if (efConfigString != null)
            {
                ConfigurationManager.ConnectionStrings["dsaEntities"].ConnectionString = efConfigString;
            }
        }
    }
}
