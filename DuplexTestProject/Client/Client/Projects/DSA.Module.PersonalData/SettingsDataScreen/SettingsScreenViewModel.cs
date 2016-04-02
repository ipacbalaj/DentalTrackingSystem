using System;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Controls.Buttons.OptionButton;
using DSA.Common.Infrastructure.Prism.EventAggregator.Events;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Module.PersonalData.OptionsMenu;
using DSA.Module.PersonalData.SettingsColumns.SettingsItem;
//using DSA.Module.PersonalData.SettingsColumns.SettingsItemList;
using DSA.Module.PersonalData.SettingsColumns.SettingsItemList;
using DSA.Module.PersonalData.SettingsColumns.SettingsTileList;
 
using DSA.Module.PersonalData.SettingsColumns.Works.WorkTileList;
using DSA.Module.PersonalData.UserDetails;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace DSA.Module.PersonalData.SettingsDataScreen
{
   public class SettingsScreenViewModel:ViewModelBase
   {
       #region Constructor
       public SettingsScreenViewModel()
       {
           eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
           unityContainer = ServiceLocator.Current.GetInstance<IUnityContainer>();
           eventAggregator.GetEvent<UpdateSettingsEvent>().Subscribe(UpdatePersonalDataScreen);         
           OptionsMenuViewModel=new OptionsMenuViewModel(this);
       }
       #endregion Constructor

       #region Properties

       private bool initialized;

       private readonly IEventAggregator eventAggregator;
       private readonly IUnityContainer unityContainer;


       private UserDetailsViewModel userDetailsViewModel;
       public UserDetailsViewModel UserDetailsViewModel
        {
            get { return userDetailsViewModel; }
            set
            {
                if (userDetailsViewModel != value)
                {
                    userDetailsViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

       private Brush contentBackground = BackgroundColors.SettingsColor;
       public Brush ContentBackground
       {
           get { return contentBackground; }
           set
           {
               if (value == contentBackground)
                   return;
               contentBackground = value;
               OnPropertyChanged();
           }
       }

       private WorkTileListViewModel workTileListViewModel;
       public WorkTileListViewModel WorkTileListViewModel
       {
           get { return workTileListViewModel; }
           set
           {
               if (value == workTileListViewModel)
                   return;
               workTileListViewModel = value;
               OnPropertyChanged();
           }
       }

       private SettingsTileListViewModel settingsTileListViewModel;
       public SettingsTileListViewModel SettingsTileListViewModel
       {
           get { return settingsTileListViewModel; }
           set
           {
               if (value == settingsTileListViewModel)
                   return;
               settingsTileListViewModel = value;
               OnPropertyChanged();
           }
       }

       private OptionsMenuViewModel optionsMenuViewModel;
       public OptionsMenuViewModel OptionsMenuViewModel
       {
           get { return optionsMenuViewModel; }
           set
           {
               if (value == optionsMenuViewModel)
                   return;
               optionsMenuViewModel = value;
               OnPropertyChanged();
           }
       }

       private Visibility settingsListVisibility=Visibility.Visible;

       public Visibility SettingsListVisibility
       {
           get { return settingsListVisibility; }
           set
           {
               if (value == settingsListVisibility)
                   return;
               settingsListVisibility = value;
               OnPropertyChanged();
           }
       }

       private Visibility technicianListVisibility=Visibility.Visible;

       public Visibility TechnicianListVisibility
       {
           get { return technicianListVisibility; }
           set
           {
               if (value == technicianListVisibility)
                   return;
               technicianListVisibility = value;
               OnPropertyChanged();
           }
       }

       private Visibility worksVisibility;

       public Visibility WorksVisibility
       {
           get { return worksVisibility; }
           set
           {
               if (value == worksVisibility)
                   return;
               worksVisibility = value;
               OnPropertyChanged();
           }
       }


       #endregion Properties

       #region Methods

       private void UpdatePersonalDataScreen(Object anObj)
       {
           if (!initialized)
           {
               initialized = true;
               OptionsMenuViewModel.OnWorks();
           }
       }

       private void InitButtons()
       {
           
       }

       #endregion Methods

   }
}
