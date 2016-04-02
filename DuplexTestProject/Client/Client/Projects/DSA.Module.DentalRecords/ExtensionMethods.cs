using DSA.Database.Model.Entities;
using DSA.Module.DentalRecords.Helpers;

namespace DSA.Module.DentalRecords
{
    public static class ExtensionMethods
    {
//        public static ToothTileViewModel ToothTileViewModel(this LocalTooth localTooth,ToothTileListViewModel parent)
//        {
//            return new ToothTileViewModel(parent)
//            {
//                Id = localTooth.Id,
//                Name = localTooth.Name,
//                //Icon = new Common.Infrastructure.Icos.ToothTest()
//            };
//        }

        public static string GetLast(this string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }



        }
}
