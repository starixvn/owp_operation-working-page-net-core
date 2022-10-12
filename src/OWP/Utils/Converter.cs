namespace OWP.Utils
{
    using System;
    using System.Diagnostics;

    public static class Converter
    {
        [DebuggerStepThrough]
        public static T To<T>(object item)
        {
            if (!ReferenceEquals(item, null) && item != DBNull.Value)
            {
                return (T)Convert.ChangeType(item, typeof(T));
            }

            return default(T);
        }

        [DebuggerStepThrough]
        public static int ToInt(object item, int defaultValue = 0)
        {
            int result = defaultValue;
            bool b = false;

            if (!ReferenceEquals(item, null) && item != DBNull.Value)
            {
                b = int.TryParse(item.ToString(), out result);
            }

            return b ? result : defaultValue;
        }

        [DebuggerStepThrough]
        public static bool ToBool(object item, bool defaultValue = false)
        {
            bool result = defaultValue;
            bool b = false;

            if (!ReferenceEquals(item, null) && item != DBNull.Value)
            {
                b = bool.TryParse(item.ToString(), out result);
            }

            return b ? result : defaultValue;
        }

        [DebuggerStepThrough]
        public static decimal ToDecimal(object item, decimal defaultValue = 0)
        {
            decimal result = defaultValue;
            bool b = false;

            if (!ReferenceEquals(item, null) && item != DBNull.Value)
            {
                b = decimal.TryParse(item.ToString(), out result);
            }

            return b ? result : defaultValue;
        }

        [DebuggerStepThrough]
        public static Guid ToGuid(object item)
        {
            if (!ReferenceEquals(item, null) && item != DBNull.Value)
            {
                return Guid.Parse(item.ToString());
            }

            return Guid.NewGuid();
        }

        [DebuggerStepThrough]
        public static DateTime ToDateTime(object item)
        {
            return ToDateTime(item, DateTime.MinValue);
        }

        public static DateTime ToDateTime(object item, DateTime defaultValue)
        {
            DateTime result = defaultValue;
            bool b = false;

            if (!ReferenceEquals(item, null) && item != DBNull.Value)
            {
                b = DateTime.TryParse(item.ToString(), out result);
            }

            return b ? result : defaultValue;
        }

        [DebuggerStepThrough]
        public static char ToChar(object item, char defaultValue = char.MinValue)
        {
            char result = defaultValue;
            bool b = false;

            if (!ReferenceEquals(item, null) && item != DBNull.Value)
            {
                b = char.TryParse(item.ToString(), out result);
            }

            return b ? result : defaultValue;
        }

        [DebuggerStepThrough]
        public static byte[] ToByteArray(object item)
        {
            return To<byte[]>(item);
        }

        [DebuggerStepThrough]
        public static string ToString2(object item)
        {
            if (!ReferenceEquals(item, null) && item != DBNull.Value)
            {
                return item.ToString();
            }

            return string.Empty;
        }
    }
}