using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSA.Database.Model.Entities.Filters;
using DSA.Filters.FilterGroup;
using DSA.Filters.Filters;
using DSA.Filters.Filters.FilterItem;

namespace DSA.Filters
{
    public  static class ExtensionMethods
    {
        public static LocalFilter ToLocalFilter(this FilterItemViewModel filterItemViewModel)
        {
            var localFilter = new LocalFilter();
            localFilter.Id = filterItemViewModel.FilterId;
            localFilter.Type = filterItemViewModel.SettingsItemName;
            localFilter.IsActive = filterItemViewModel.IsChecked;            
            localFilter.FilterItems=new List<LocalFilterItem>();
            localFilter.ItemsToDeleteIds = filterItemViewModel.itemsToDelete;
            //var selectedItems = new List<LocalFilterItem>();
            foreach (var selectedItemTileViewModel in filterItemViewModel.SelectedItemTileViewModels)
            {
                var fI = new LocalFilterItem()
                {
                    Id = selectedItemTileViewModel.Id,
                    Name =selectedItemTileViewModel.Name
//                        filterItemViewModel.SettingsItems.FirstOrDefault(item => item.Id == selectedItemTileViewModel.Id)
//                            .Name
                };
                localFilter.FilterItems.Add(fI);
            }
//            foreach (var settingsItem in filterItemViewModel.SettingsItems)
//            {
//                localFilter.FilterItems.Add(new LocalFilterItem()
//                {
//                    Name = settingsItem.Name
//                });
//            }
            return localFilter;
        }

        public static LocalFilterGroup ToLocalFilterGroup(this FiltersViewModel filtersViewModel)
        {
            LocalFilterGroup localFilterGroup =new LocalFilterGroup();
            localFilterGroup.Id = filtersViewModel.GroupId;
            localFilterGroup.Name = filtersViewModel.GroupName;
            localFilterGroup.IsActive = filtersViewModel.IsChecked;
            localFilterGroup.Filters=new List<LocalFilter>();
            localFilterGroup.StartDate = filtersViewModel.IntervalFilterItemViewModel.StartDate;
            localFilterGroup.EndDate = filtersViewModel.IntervalFilterItemViewModel.EndDate;
            localFilterGroup.IsIntervalActive = filtersViewModel.IntervalFilterItemViewModel.IsChecked;
            foreach (var filterItemViewModel in filtersViewModel.FilterItemList)
            {
                localFilterGroup.Filters.Add(filterItemViewModel.ToLocalFilter());
            }
            return localFilterGroup;
        }

//        public static LocalFilterGroup ToLocalFilterGroup(this FilterGroupViewModel filterGroupViewModel)
//        {
//            LocalFilterGroup localFilterGroup =new LocalFilterGroup();
//            localFilterGroup.Name = filterGroupViewModel.GroupName;
//            localFilterGroup.Filters=new List<LocalFilter>();
//            foreach (var filtersViewModel in filterGroupViewModel.FiltersViewModels.List)
//            {
//                localFilterGroup.Filters.Add(filtersViewModel.);
//            }
//        }
    }
}
