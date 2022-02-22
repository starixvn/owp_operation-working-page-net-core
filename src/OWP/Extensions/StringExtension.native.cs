using System.Globalization;

namespace OWP.Extensions;

public static class StringExtension
{
    public static bool IsContains(this string toBeSearchedString, string searchString)
    {
        if (string.IsNullOrEmpty(toBeSearchedString))
        {
            return false;
        }

        var notNullSearchString = searchString ?? string.Empty;
        return toBeSearchedString.Contains(notNullSearchString, StringComparison.OrdinalIgnoreCase);
    }

    public static string ToDashes(this int @this)
        => @this == 0 ? "-" : $"{@this}";

    public static string ToFormatVn(this DateTime @this)
        => @this.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

    public static string ToFormatDDMMyyyy(this DateTime @this)
        => @this.ToString("ddMMyyyy", CultureInfo.InvariantCulture);
}