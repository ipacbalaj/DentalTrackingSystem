using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Controls.Buttons;
using DSA.Common.Infrastructure;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Module.PersonalData.SettingsColumns.SettingsItemList;
using DSA.Module.PersonalData.SettingsColumns.SettingsTile;
using DSA.Module.PersonalData.SettingsColumns.SettingsTile.EditSettingsTile;
 
using Microsoft.Practices.Prism.Commands;

namespace DSA.Module.PersonalData.SettingsColumns.SettingsTileList
{
    public class SettingsTileListViewModel : ViewModelBase
    {
        #region Constructor

        public SettingsTileListViewModel(LocalCache.SettingsItemEnum settingType)
        {
            SettingType = settingType;
            PopulateSettingsItems(settingType);
            Name = settingType.ToString();
            EditSettingsTileViewModel = new EditSettingsTileViewModel(this, settingsType);
            AddSettingButton = new ActionButtonViewModel("Adaugă " + Name, new DelegateCommand(OnAddSetting), ImagePath.AddPatientPath);
            SaveButton = new ActionButtonViewModel("Salvează  ", new DelegateCommand(OnSave), ImagePath.SaveIconPath);
            deletedSettingsIds = new List<int>();
        }

        #endregion Constructor

        #region Properties

        public SettingsTileListView ViewReference { get; set; }

        private List<int> deletedSettingsIds;

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (value == name)
                    return;
                name = value;
                OnPropertyChanged();
            }
        }

        private Visibility addButtonVisibility = Visibility.Visible;

        public Visibility AddButtonVisibility
        {
            get { return addButtonVisibility; }
            set
            {
                if (value == addButtonVisibility)
                    return;
                addButtonVisibility = value;
                OnPropertyChanged();
            }
        }

        private LocalCache.SettingsItemEnum settingsType;

        public LocalCache.SettingsItemEnum SettingType
        {
            get { return settingsType; }
            set
            {
                if (value == settingsType)
                    return;
                settingsType = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel saveButton;

        public ActionButtonViewModel SaveButton
        {
            get { return saveButton; }
            set
            {
                if (value == saveButton)
                    return;
                saveButton = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SettingsTileViewModel> settingsTileListModels;

        public ObservableCollection<SettingsTileViewModel> SettingsTileListModels
        {
            get { return settingsTileListModels; }
            set
            {
                if (value == settingsTileListModels)
                    return;
                settingsTileListModels = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SettingsTileViewModel> allSettingsTiles;

        public ObservableCollection<SettingsTileViewModel> AllsettingsTiles
        {
            get { return allSettingsTiles; }
            set
            {
                if (value == allSettingsTiles)
                    return;
                allSettingsTiles = value;
                OnPropertyChanged();
            }
        }

        private String searchedItem;
        public String SearchedItem
        {
            get { return searchedItem; }
            set
            {
                if (searchedItem != value)
                {
                    searchedItem = value;
                    Search(searchedItem);
                    OnPropertyChanged();
                }
            }
        }

        private EditSettingsTileViewModel editSettingsTileViewModel;
        public EditSettingsTileViewModel EditSettingsTileViewModel
        {
            get { return editSettingsTileViewModel; }
            set
            {
                if (value == editSettingsTileViewModel)
                    return;
                editSettingsTileViewModel = value;
                OnPropertyChanged();
            }
        }

        private Visibility editControlVisibility = Visibility.Collapsed;

        public Visibility EditControlVisibility
        {
            get { return editControlVisibility; }
            set
            {
                if (value == editControlVisibility)
                    return;
                editControlVisibility = value;
                OnPropertyChanged();
            }
        }

        private ActionButtonViewModel addSettingButton;

        public ActionButtonViewModel AddSettingButton
        {
            get { return addSettingButton; }
            set
            {
                if (value == addSettingButton)
                    return;
                addSettingButton = value;
                OnPropertyChanged();
            }
        }

        private bool wasChanged;

        public bool WasChanged
        {
            get { return wasChanged; }
            set
            {
                wasChanged = value;
                StackColor = wasChanged ? BackgroundColors.BackgroundFilled : null;
                OnPropertyChanged();
            }
        }

        private Brush stackColor;

        public Brush StackColor
        {
            get { return stackColor; }
            set
            {
                if (value == stackColor)
                    return;
                stackColor = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public void PopulateSettingsItems(LocalCache.SettingsItemEnum setingsItemName)
        {
            AllsettingsTiles = new ObservableCollection<SettingsTileViewModel>();
            foreach (var settingsSubItem in LocalCache.Instance.SettingsItems[setingsItemName])
            {
                AllsettingsTiles.Add(new SettingsTileViewModel(this, SettingType)
                {
                    Id = settingsSubItem.Id,
                    Name = settingsSubItem.Name,
                    Cost = settingsSubItem.Cost,
                    Percent = settingsSubItem.Percent,
                    IncludedINFinancial = settingsSubItem.IncludedINFinancial
                });
            }
            SettingsTileListModels = AllsettingsTiles;
        }

        public void RemoveItem(SettingsTileViewModel itemToDelete)
        {
            SettingsTileListModels.Remove(itemToDelete);
            AllsettingsTiles.Remove(itemToDelete);
            var settingToDelete =
                LocalCache.Instance.SettingsItems[SettingType].FirstOrDefault(item => item.Id == itemToDelete.Id);
            LocalCache.Instance.SettingsItems[SettingType].Remove(settingToDelete);
            deletedSettingsIds.Add(itemToDelete.Id);
            WasChanged = true;
        }

        private void OnAddSetting()
        {
            var newAddedSetting = new SettingsTileViewModel(this, settingsType);
            newAddedSetting.OnEdit();
            newAddedSetting.ContentBackGround = BackgroundColors.SuccessfulColor;
            AllsettingsTiles.Add(newAddedSetting);
            ViewReference.scrollViewerSettingsTileList.ScrollToBottom();
            WasChanged = true;
        }

//        public async void OnSave(int id, String name, double cost, double percent, bool includedINFinancial)
//        {
//           
//        }

        private void Search(string searchedWord)
        {
            searchedWord = searchedWord.ToLower();
            int wordLength = searchedWord.Length;
            // CurrentPatients = new ObservableCollection<PatientViewModel>(PatientsList.Where(item => item.Name.ToLower().Contains(searchedWord)));
            SettingsTileListModels = new ObservableCollection<SettingsTileViewModel>(AllsettingsTiles.Where(item => (item.Name != null && item.Name.Length >= wordLength && item.Name.ToLower().Substring(0, wordLength) == searchedWord)));
            //            SelectedPatient = CurrentPatients.FirstOrDefault();
        }

        private async void OnSave()
        {
            foreach (var settingsTileListModel in SettingsTileListModels)
            {
                settingsTileListModel.OnSave();
            }
            switch (SettingType)
            {
                case LocalCache.SettingsItemEnum.Manopera:
                    List<string> unableToDeleteWorks = await DatabaseHandler.Instance.DeleteWorks(deletedSettingsIds);
                    string name = unableToDeleteWorks.Count > 1 ? "Manoperele" : "Manopera";
                 //   GetUnableToDeleteItems(unableToDeleteWorks,name);
                    break;
                case LocalCache.SettingsItemEnum.Zona:
                    List<string> unableToDeleteareas = await DatabaseHandler.Instance.DeleteAreas(deletedSettingsIds);
                    name = unableToDeleteareas.Count > 1 ? "Zonele" : "Zona";
                //    GetUnableToDeleteItems(unableToDeleteareas, name);
                    //   DatabaseHandler.Instance.DeleteAreas(deletedSettingsIds);
                    break;
                case LocalCache.SettingsItemEnum.Locatie:
                    List<string> unableToDeleteLocations = await DatabaseHandler.Instance.DeleteLocations(deletedSettingsIds);
                    name = unableToDeleteLocations.Count > 1 ? "Locatiile" : "Locatia";
                 //   GetUnableToDeleteItems(unableToDeleteLocations, name);
                   // DatabaseHandler.Instance.DeleteLocations(deletedSettingsIds);
                    break;
            }
            deletedSettingsIds=new List<int>();
            WasChanged = false;
        }

        private void GetUnableToDeleteItems(List<string> unableToDeleteNames, string name)
        {
            if (unableToDeleteNames.Count > 0)
            {
//                List<SettingsTileViewModel> unableToDeleteItems = new List<SettingsTileViewModel>();
//                foreach (var id in unableToDeleteIds)
//                {
//                    unableToDeleteItems.Add(SettingsTileListModels.FirstOrDefault(item => item.Id == id));
//                }
                string messagePred = "";
                if (unableToDeleteNames.Count > 1)
                {
                    messagePred = " nu pot fi sterse . Exista interventii care depind de ele";
                }
                else
                {
                    messagePred = "nu poate fi sterarsa . Exista interventii care depind de ea";
                }
                //            string oneItem = "nu poate fi sterarsa . Exista interventii cu avand aceast ";
                //            string multipleItems = "nu pot fi sterse . Exista interventii cu avand aceaste ";
                string names = "";
                foreach (var nameitem in unableToDeleteNames)
                {
                    names += nameitem + ",";

                }
                MessageBox.Show(name + " " + names + messagePred);   
            }
        }

        #endregion Methods

    }
}
