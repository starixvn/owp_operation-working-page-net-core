using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace OWP.OtherSport.Validation.Helpers;

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