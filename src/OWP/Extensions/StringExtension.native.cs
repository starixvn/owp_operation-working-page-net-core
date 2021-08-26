namespace OWP.Extensions
{
    using System;

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
    }
}