using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Common.Infrastructure.ViewModel;

namespace DSA.Module.DentalRecords.Filters.FilterItem.SelectedItemTile
{
    public class SelectedItemTileViewModel:ViewModelBase
    {
        #region Constructor

        public SelectedItemTileViewModel()
        {
            
        }

        #endregion Constructor

        #region Properties

        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion Properties

        #region Methods

        #endregion Methods

    }
}
