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

            var notNullsearchString = searchString ?? string.Empty;
            return toBeSearchedString.IndexOf(notNullsearchString, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}