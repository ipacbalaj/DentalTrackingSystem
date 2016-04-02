using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Common.Infrastructure.ViewModel;
using DSA.Module.DentalRecords.DentalRecordsScreen;
using DSA.Module.DentalRecords.Filters;
using DSA.Module.DentalRecords.Helpers;

namespace DSA.Module.DentalRecords.Options.DisplayOptions
{
   public class DisplayOptionsViewModel:ViewModelBase
   {
       #region Constructor

       public DisplayOptionsViewModel(DentalRecordsScreeViewModel parent)
       {
           FiltersViewModel=new FiltersViewModel(this);
           Parent = parent;
       }
       #endregion Constructor

       #region Properties

       public DentalRecordsScreeViewModel Parent { get; set; }

       private FiltersViewModel filtersViewModel;
       public FiltersViewModel FiltersViewModel
       {
           get { return filtersViewModel; }
           set
           {
               if (filtersViewModel != value)
               {
                   filtersViewModel = value;
                   OnPropertyChanged();
               }
           }
       }    

       #endregion Properties

       #region Methods

       #endregion Methods

   }
}
