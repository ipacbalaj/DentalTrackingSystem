using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Database.Model.Entities.SettingsEntities;
using DSA.Module.PersonalData.SettingsColumns.Works.WorkTile;
using DSA.Module.PersonalData.SettingsColumns.Works.WorkTile.WorkTypeTile;

namespace DSA.Module.DentalRecords.Helpers
{
    public static class ExtensionMethods
    {

        public static LocalWorkType ToLocalWorkType(this WorkTypeTileViewModel workTypeTile)
        {
            return new LocalWorkType()
            {
                Cost = workTypeTile.Cost.HasValue?workTypeTile.Cost.Value:0,
                Name = workTypeTile.Name,
                Percent = workTypeTile.Percent,
                Id = workTypeTile.Id
            };
        }

        public static LocalWork ToLocalWork(this WorkTileViewModel workTileViewModel)
        {
            var work = new LocalWork();
            work.Name = workTileViewModel.Name;
            work.Cost = workTileViewModel.Cost.HasValue?workTileViewModel.Cost.Value:0;
            work.Percent = workTileViewModel.Percent.HasValue?workTileViewModel.Percent.Value:0;
            work.IncludedINFinancial = workTileViewModel.IncludedINFinancial;
            work.WorkTypes=new List<LocalWorkType>();
            foreach (var workTypeTileViewModel in workTileViewModel.WorkTypeTileList.WorkTypes)
            {
                work.WorkTypes.Add(workTypeTileViewModel.ToLocalWorkType());
            }
            return work;
        }
    }
}
