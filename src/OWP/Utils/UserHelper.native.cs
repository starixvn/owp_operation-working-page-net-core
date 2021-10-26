namespace OWP.Utils
{
    using System;

    public static class UserHelper
    {
        public static string GetFlagCodeFromLocationCode(string locationCode)
        {
            if (string.IsNullOrEmpty(locationCode))
            {
                return string.Empty;
            }

            if (locationCode.Equals("SMS", StringComparison.CurrentCultureIgnoreCase))
            {
                return "vn";
            }

            return locationCode.Equals("CTI", StringComparison.CurrentCultureIgnoreCase) ? "ph" : string.Empty;
        }
    }
}