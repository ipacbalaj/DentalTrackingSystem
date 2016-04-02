using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DSA.Common.Controls.Buttons;
using DSA.Common.Infrastructure;
using DSA.Common.Infrastructure.Styles;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Database.Model;
using DSA.Database.Model.EfSQlLite;
using DSA.Module.PersonalData.SettingsColumns.SettingsTile;
using DSA.Module.PersonalData.SettingsColumns.Works.WorkTile;
using DSA.Module.PersonalData.SettingsColumns.Works.WorkTile.WorkTypeTile;
using DSA.Module.PersonalData.SettingsColumns.Works.WorkTile.WorkTypeTileList;
using Microsoft.Practices.Prism.Commands;

namespace DSA.Module.PersonalData.SettingsColumns.Works.WorkTileList
{
    public class WorkTileListViewModel : ViewModelBase
    {
        #region Constructor

        public WorkTileListViewModel()
        {
            PopulateWorks();
            AddWorkButton = new ActionButtonViewModel("Adaugă Manoperă", new DelegateCommand(OnAddWork), ImagePath.AddPatientPath);
            SaveButton = new ActionButtonViewModel("Salvează  ", new DelegateCommand(OnSave), ImagePath.SaveIconPath);
        }

        #endregion Constructor

        #region Properties

        private ObservableCollection<WorkTileViewModel> works;

        public ObservableCollection<WorkTileViewModel> Works
        {
            get { return works; }
            set
            {
                if (value == works)
                    return;
                works = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<WorkTileViewModel> seenWorks;

        public ObservableCollection<WorkTileViewModel> SeenWorks
        {
            get { return seenWorks; }
            set
            {
                if (value == seenWorks)
                    return;
                seenWorks = value;
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

        private ActionButtonViewModel addWorkButton;
        public ActionButtonViewModel AddWorkButton
        {
            get { return addWorkButton; }
            set
            {
                if (value == addWorkButton)
                    return;
                addWorkButton = value;
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

        private bool wasChanged;

        public bool WasChanged
        {
            get { return wasChanged; }
            set
            {
                wasChanged = value;
                SaveStateColor = WasChanged ? BackgroundColors.BackgroundFilled : null;
                OnPropertyChanged();
            }
        }

        private Brush saveStateColor;

        public Brush SaveStateColor
        {
            get { return saveStateColor; }
            set
            {
                if (value == saveStateColor)
                    return;
                saveStateColor = value;
                OnPropertyChanged();
            }
        }


        public List<int> workTypeToDeleteIds = new List<int>();
        public List<int> worksToDeleteIds = new List<int>();

        #endregion Properties

        #region Methods

        private void PopulateWorks()
        {
            Works = new ObservableCollection<WorkTileViewModel>();
            foreach (var work in LocalCache.Instance.Works)
            {
                var worktile = new WorkTileViewModel(this)
                {
                    Id = work.Id,
                    Name = work.Name,
                    Cost = work.Cost,
                    Percent = work.Percent,
                    IncludedINFinancial = work.IncludedINFinancial
                };
                worktile.WorkTypeTileList = new WorkTypeTileListViewModel(worktile);
                foreach (var workType in work.WorkTypes)
                {
                    worktile.WorkTypeTileList.WorkTypes.Add(new WorkTypeTileViewModel(worktile.WorkTypeTileList)
                    {
                        Id = workType.Id,
                        Name = workType.Name,
                        Cost = workType.Cost,
                        Percent = workType.Percent
                    });
                }
                Works.Add(worktile);
            }
            SeenWorks = Works;
            WasChanged = false;
        }

        private void OnAddWork()
        {
            var newWork = new WorkTileViewModel(this);
            Works.Add(newWork);
            newWork.OnEdit();
            WasChanged = true;
        }

        private async void OnSave()
        {
            await DatabaseHandler.Instance.DeleteWorkTypesById(workTypeToDeleteIds);
            await DatabaseHandler.Instance.DeleteWorksById(worksToDeleteIds);
            try
            {
                LocalCache.Instance.Works = await DatabaseHandler.Instance.SaveEditWorks(Works.Where(item => item.WasChanged).ToList().TolLocalWorks());
            }
            catch (Exception)
            {
                MessageBox.Show("A intervenit o eroare.Adaugă nume la toate manoperele");
                return;
            }
            foreach (var workTileViewModel in Works)
            {
                workTileViewModel.OnSave();
            }
            WasChanged = false;
            PopulateWorks();
            workTypeToDeleteIds = new List<int>();
            worksToDeleteIds = new List<int>();
        }

        private void Search(string searchedWord)
        {
            searchedWord = searchedWord.ToLower();
            int wordLength = searchedWord.Length;
            // CurrentPatients = new ObservableCollection<PatientViewModel>(PatientsList.Where(item => item.Name.ToLower().Contains(searchedWord)));
            SeenWorks = new ObservableCollection<WorkTileViewModel>(Works.Where(item => (item.Name != null && item.Name.Length >= wordLength && item.Name.ToLower().Substring(0, wordLength) == searchedWord)));
            //            SelectedPatient = CurrentPatients.FirstOrDefault();
        }

        public void RemoveItem(WorkTileViewModel workToRemove)
        {
            Works.Remove(workToRemove);
            if (workToRemove.Id != 0)
            {
                worksToDeleteIds.Add(workToRemove.Id);
            }
            WasChanged = true;
        }

        #endregion Methods()

    }
}
