using System.Collections.Generic;
using System.Linq;

namespace OWP.OtherSport.EventAssignment.Helper
{
    public static class DicHelper
    {
        public static IList<T> GetValueFromDicBykey<T>(
             Dictionary<int, List<T>> dic,
             int osId)
        {
            dic.TryGetValue(osId, out List<T> value);
            return value == null
                ? new List<T>()
                : value.ToList();
        }
    }
}
