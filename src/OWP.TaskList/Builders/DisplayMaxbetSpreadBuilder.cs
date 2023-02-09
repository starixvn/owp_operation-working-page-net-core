namespace OWP.TaskList;

using System.Globalization;

public static class DisplayMaxbetSpreadBuilder
{
    public static string Build(decimal? maxBet, decimal? spread)
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

    public static string ToOWPMaxBetFormat(this decimal? maxbet)
    {
        return maxbet.HasValue ? (maxbet.Value / 1000).ToString("0.####", CultureInfo.InvariantCulture) + "K" : string.Empty;
    }

    public static string ToOWPSpreadFormat(this decimal? spread)
    {
        return spread.HasValue ? (spread.Value * 100).ToString("0.##", CultureInfo.InvariantCulture) : string.Empty;
    }
}

