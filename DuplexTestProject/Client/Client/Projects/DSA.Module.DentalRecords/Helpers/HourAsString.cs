using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Common.Infrastructure.ViewModel;

namespace DSA.Module.DentalRecords.Helpers
{
   public class HourAsString:ViewModelBase
    {
//       public string Hour { get; set; }
//       public DateTime CorresponDateTime { get; set; }

       private string hour;
       public string Hour
       {
           get { return hour; }
           set
           {
               if (hour == value)
                   return;
               hour = value;
            // CorresponDateTime=  CorresponDateTime.AddHours(Convert.ToInt32(hour.Substring(0, 2)));
               var minutes = hour.GetLast(2);
          //   CorresponDateTime =  CorresponDateTime.AddMinutes(Convert.ToInt32(minutes));
               OnPropertyChanged();
           }
       }

       private DateTime corresponDateTime=DateTime.Today;
       public DateTime CorresponDateTime
       {
           get { return corresponDateTime; }
           set
           {
               if (corresponDateTime == value)
                   return;
               corresponDateTime = value;
               OnPropertyChanged();
           }
       }       

    }
}
