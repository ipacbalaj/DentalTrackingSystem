using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientShell.Views.Tabs.HorizontalTab;
using DSA.Common.Infrastructure.Icos;
using DSA.Common.Infrastructure.ObjectList;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.ViewModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using TULIP.Common.Controls.Icons;
using TULIP.Common.Controls.Icons.TabsIcons;

namespace ClientShell.Views.Tabs.HorizontalTabs
{
    public class HorizontalTabsViewModel:ViewModelBase
    {
        #region Constructor

        public HorizontalTabsViewModel()
        {
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            PopulateTabs();
        }
        #endregion Constructor

        #region Properties
        private readonly IEventAggregator eventAggregator;

        private ObjectList<HorizontalTabViewModel> tabList = new ObjectList<HorizontalTabViewModel>(false);
        public ObjectList<HorizontalTabViewModel> TabList
        {
            get { return tabList; }
            set
            {
                if (tabList != value)
                {
                    tabList = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion Properties

        #region Methods

        public void PopulateTabs()
        {        
            TabList.Add(new HorizontalTabViewModel() { Name = "Jurnal", Icon = new Tooth(), Command = new DelegateCommand(OnDetalPaper) });
            TabList.Add(new HorizontalTabViewModel() { Name = "Filtre", Icon = new SettingsIcon(), Command = new DelegateCommand(OnFilters) });
            TabList.Add(new HorizontalTabViewModel() { Name = "Rapoarte", Icon = new SettingsIcon(), Command = new DelegateCommand(OnReports) }); 
            TabList.Add(new HorizontalTabViewModel() { Name = "Setari", Icon = new SettingsIcon(), Command = new DelegateCommand(OnSettingsCommand)});
            TabList.Add(new HorizontalTabViewModel() { Name = "Fisa Pacient", Icon = new SettingsIcon(), Command = new DelegateCommand(OnPersonalData)});
            TabList.Add(new HorizontalTabViewModel() { Name = "Financiar", Icon = new SettingsIcon(), Command = new DelegateCommand(OnFinancial)});
            var selectedTab = TabList.List.FirstOrDefault();
            selectedTab.OnSelected(selectedTab);
        }

        private void OnSettingsCommand()
        {
            eventAggregator.GetEvent<ShellStateChangeEvent>().Publish(ShellState.Settings);
        }

        private void OnPersonalData()
        {
            eventAggregator.GetEvent<ShellStateChangeEvent>().Publish(ShellState.PersonalData);
        }

        private void OnDetalPaper()
        {
            eventAggregator.GetEvent<ShellStateChangeEvent>().Publish(ShellState.DentalRecords);
        }

        private void OnFilters()
        {
            eventAggregator.GetEvent<ShellStateChangeEvent>().Publish(ShellState.Filters);
        }

        private void OnReports()
        {
            eventAggregator.GetEvent<ShellStateChangeEvent>().Publish(ShellState.Reports);
        }

        private void OnFinancial()
        {
            eventAggregator.GetEvent<ShellStateChangeEvent>().Publish(ShellState.Financial);
        }

        public void OnSelectReportsTab(string tabToSelect)
        {
           var selectedTab =  TabList.List.FirstOrDefault(item => item.Name == tabToSelect);
           selectedTab.OnSelected(selectedTab);
        }

        #endregion Methods

    }
}
