using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OWP.OtherSport.Validation.Helpers
{
    public static class PropertyHelper
    {
        public static string GetDisplayName(PropertyInfo property)
        {
            var attribute = property.GetCustomAttributes(typeof(DisplayAttribute), true)
                  .Cast<DisplayAttribute>()
                  .FirstOrDefault();
            return attribute == null
                ? string.Empty
                : attribute.Name;
        }
    }
}
