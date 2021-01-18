using System.Collections.Generic;
using System.Linq;

namespace OWP.OtherSport.Helper
{
    public static class DicHelper
    {
        public static IList<T> GetValueFromDicBykey<T>(
             Dictionary<int, List<T>> dic,
             int osId)
        {
            List<T> value;
            dic.TryGetValue(osId, out value);
            return value == null
                ? new List<T>()
                : value.ToList();
        }
    }
}
