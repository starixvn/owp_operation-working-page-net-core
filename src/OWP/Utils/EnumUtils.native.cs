using System.ComponentModel;
// ReSharper disable LoopCanBeConvertedToQuery

namespace OWP.Utils;

public static class EnumUtils<T>
{
    public static string GetDescription(T enumValue, string defDesc)
    {
        var fi = enumValue.GetType().GetField(enumValue.ToString());

        if (null == fi)
        {
            return defDesc;
        }

        var attrs = fi.GetCustomAttributes
            (typeof(DescriptionAttribute), true);
        return attrs.Length > 0 ? ((DescriptionAttribute)attrs[0]).Description : defDesc;
    }

    public static string GetDescription(T enumValue)
    {
        return GetDescription(enumValue, string.Empty);
    }

    public static string GetDescription(int value)
    {
        var enumValue = ToEnum(value);
        return GetDescription(enumValue, string.Empty);
    }

    public static T FromDescription(string description)
    {
        var t = typeof(T);
        foreach (var fi in t.GetFields())
        {
            var attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attrs.Length <= 0)
            {
                continue;
            }

            foreach (DescriptionAttribute attr in attrs)
            {
                if (attr.Description.Equals(description))
                {
                    return (T)fi.GetValue(null);
                }
            }
        }
        return default;
    }

    public static T ToEnum(string s)
    {
        return (T)Enum.Parse(typeof(T), s);
    }

    public static T ToEnum(int number)
    {
        return (T)Enum.ToObject(typeof(T), number);
    }
}