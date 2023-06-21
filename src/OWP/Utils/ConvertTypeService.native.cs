using System.Globalization;

namespace OWP.Utils;

public static class ConvertTypeService
{
    private const string DateFormat = "MMM dd, yyyy";
    private const string TimeFormat = "hh:mm tt";
    private const string LastUpdatedFormat = "MMM dd hh:mm tt";

    public static string BuildDisplayMaxbetSpread(decimal? maxBet, decimal? spread)
    {
        var maxbetDisplay = maxBet.ToOWPMaxBetFormat();
        var spreadDisplay = spread.ToOWPSpreadFormat();
        if (string.IsNullOrEmpty(maxbetDisplay) && string.IsNullOrEmpty(spreadDisplay))
        {
            return string.Empty;
        }

        return string.Format(
            CultureInfo.InvariantCulture,
            "{0} {1}",
            maxbetDisplay,
            !string.IsNullOrWhiteSpace(spreadDisplay) ? string.Format(CultureInfo.InvariantCulture, "({0}%)", spreadDisplay) : string.Empty);
    }

    public static string ToOWPDateFormat(this DateTime date)
    {
        return date.ToString(DateFormat, CultureInfo.InvariantCulture);
    }

    public static string ToLastUpdatedFormat(this DateTime date)
    {
        return date.ToString(LastUpdatedFormat, CultureInfo.InvariantCulture);
    }

    public static string ToOWPTimeFormat(this TimeSpan time)
    {
        var date = new DateTime(1, 1, 1, time.Hours, time.Minutes, 0);

        return date.ToString(TimeFormat, CultureInfo.InvariantCulture);
    }

    public static string ToKOTimeDateFormat(this DateTime date)
    {
        return date.ToString("dd-MMM", CultureInfo.InvariantCulture);
    }

    public static string GetCompanyFromLocationCode(string locationCode)
    {
        var company = string.Empty;
        if (!string.IsNullOrEmpty(locationCode))
        {
            var codes = locationCode.Split('-');
            if (codes.Length == 2)
            {
                company = codes[0].ToUpper(CultureInfo.InvariantCulture);
            }
        }

        return company;
    }

    public static string ToOWPMaxBetFormat(this decimal? maxbet)
    {
        return maxbet.HasValue ? (maxbet.Value / 1000).ToString("0.####", CultureInfo.InvariantCulture) + "K" : string.Empty;
    }

    public static string ToOWPSpreadFormatPercent(this decimal? spread)
        => spread.HasValue
            ? $"{ToOWPSpreadFormat(spread)}%"
            : string.Empty;

    public static string ToOWPSpreadFormat(this decimal? spread)
    {
        return spread.HasValue ? (spread.Value * 100).ToString("0.##", CultureInfo.InvariantCulture) : string.Empty;
    }

    public static string ReplaceFirstOccurrence(this string sourceText, string searchText, string replaceText)
    {
        var index = sourceText.IndexOf(searchText, StringComparison.InvariantCulture);
        if (index < 0)
        {
            return sourceText;
        }

        var result = sourceText.Remove(index, searchText.Length).Insert(index, replaceText);

        return result;
    }

    public static bool IsBasicType(Type t)
    {
        var temp = t;

        if (t != null && t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            temp = t.GetGenericArguments()[0];
        }

        return temp == typeof(int) || temp == typeof(double) || temp == typeof(string) || temp == typeof(DateTime);
    }
}