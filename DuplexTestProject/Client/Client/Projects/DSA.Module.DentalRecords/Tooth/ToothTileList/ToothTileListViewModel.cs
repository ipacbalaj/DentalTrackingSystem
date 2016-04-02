using DSA.Common.Infrastructure.ObjectList;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Module.DentalRecords.Tooth.ToothTile;

namespace DSA.Module.DentalRecords.Tooth.ToothTileList
{
    public class ToothTileListViewModel:ViewModelBase
    {
        #region Constructor

        public ToothTileListViewModel()
        {
        }
        #endregion Constructor

        #region Properties

        private ObjectList<ToothTileViewModel> teethList;
        public ObjectList<ToothTileViewModel> TeethList
        {
            get { return teethList; }
            set
            {
                if (teethList != value)
                {
                    teethList = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObjectList<ToothTileViewModel> teethListDown;
        public ObjectList<ToothTileViewModel> TeethListDown
        {
            get { return teethListDown; }
            set
            {
                if (teethListDown != value)
                {
                    teethListDown = value;
                    OnPropertyChanged();
                }
            }
        }

        private ToothTileViewModel selectedTooth;
        public ToothTileViewModel SelectedTooth
        {
            get { return selectedTooth; }
            set
            {
                if (selectedTooth != value)
                {
                    selectedTooth = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion Properties

        #region Methods

        public void PopulateTeethList()
        {
            TeethList=new ObjectList<ToothTileViewModel>(false);
//            foreach (var localTooth in LocalCache.Instance.LocalTeethList)
//            {
//                TeethList.Add(localTooth.ToothTileViewModel(this));
//            }
//
//            TeethListDown = new ObjectList<ToothTileViewModel>(false);
//            foreach (var localTooth in LocalCache.Instance.LocalTeethList)
//            {
//                TeethListDown.Add(localTooth.ToothTileViewModel(this));
//            }

        }
        #endregion Methods

    }
}
