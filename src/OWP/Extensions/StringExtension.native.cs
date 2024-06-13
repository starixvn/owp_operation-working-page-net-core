using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

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

    public static string ReplaceWith(this string text, string regex, string replacement)
    {
        return Regex.Replace(text, regex, replacement);
    }

    public static bool IsContainsAll(this string toBeSearchedString, params string[] searchStrings)
       => searchStrings.All(toBeSearchedString.IsContains);


    public static int? ConvertNullableInt(this string text)
    {
        if (int.TryParse(text, out var i)) return i;
        return null;
    }

    public static string RemoveNonAlphaCharacters(string source) => new(source?.Where(char.IsLetter).ToArray());

    public static string ConvertNullToEmpty(string value)
    {
        return string.IsNullOrEmpty(value)
            ? string.Empty
            : value.Trim();
    }

    public static T ParseXml<T>(this string xml) where T : class
    {
        var locker = new object();
        var stringReader = new StringReader(xml);
        var reader = new XmlTextReader(stringReader);
        try
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            lock (locker)
            {
                var item = (T)xmlSerializer.Deserialize(reader);
                reader.Close();
                return item;
            }
        }
        catch
        {
            return default(T);
        }
        finally
        {
            reader.Close();
        }
    }
}