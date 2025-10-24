using OWP.Constants;
using static System.DateTime;

namespace OWP.EventPicking.Api.Helper;

public static class SpecialProductHelper
{
    private const int SpreadPercentageMultiplier = 100;
    private const int MaxbetKiloDivisor = 1000;
    public static void BuildSpecialProduct<T>(IList<T> events) where T : class, EventProductCommon
    {
        foreach (var @event in events)
        {
            var (leaguePostfix, teamPostfix) = BuildPostFixSpecialProduct(@event.MinorBettypeId);
            @event.LeagueName = $"{@event.LeagueName} {leaguePostfix}";
            @event.HomeName = $"{@event.HomeName} {teamPostfix}";
            @event.AwayName = $"{@event.AwayName} {teamPostfix}";
        }
    }

    public static List<T> CombineMainAndChildMatchAndAddMatchNo<T>(IList<T> mainMatches, IList<T> childMatches) where T : class, EventProductCommon
    {
        var matches = new List<T>();

        var matchNo = 1;
        foreach (var mainMatch in mainMatches)
        {
            var childMatchesOfThisMainMatch = childMatches.Where(childMatch => childMatch.InfoSiteMatchId == mainMatch.InfoSiteMatchId).ToList();
            AssignMatchNo(mainMatch, matchNo, childMatchesOfThisMainMatch);
            matchNo++;

            matches.Add(mainMatch);
            matches.AddRange(childMatchesOfThisMainMatch);
        }

        return matches;
    }

    private static void AssignMatchNo<T>(T mainMatch, int matchNo, List<T> childMatchesOfThisMainMatch) where T : class, EventProductCommon
    {
        mainMatch.MatchNo = MatchNoHelper.BuildMainMatchNo(matchNo);
        foreach (var childMatch in childMatchesOfThisMainMatch)
        {
            childMatch.MatchNo = MatchNoHelper.BuildChildMatchNo(matchNo, childMatch.MinorBettypeId);
        }

        childMatchesOfThisMainMatch.Sort((x, y) => string.CompareOrdinal(x.MatchNo, y.MatchNo));
    }

    public static (string leaguePostfix, string teamPostfix) BuildPostFixSpecialProduct(int? minorBettypeId)
    {
        return minorBettypeId switch
        {
            Bettype.NoOfCorner => ("- CORNERS", "- No.of Corners"),
            Bettype.NextCorner => ("- CORNERS", "- Next Corners"),
            Bettype.TotalBooking => ("- BOOKING", "- Total Bookings"),
            Bettype.NextBooking => ("- BOOKING", "- Next Booking"),
            _ => (string.Empty, string.Empty)
        };
    }

    public static bool CheckMatchHaveBeenExclude(string homeName)
        => !string.IsNullOrEmpty(homeName) && (homeName.IsContains("(ET)") || homeName.IsContains("(PEN)"));

    public static string GetCleanedTeamPostfix(int? minorBettypeId)
    {
        var (_, teamPostfix) = BuildPostFixSpecialProduct(minorBettypeId);
        return string.IsNullOrEmpty(teamPostfix)
            ? teamPostfix
            : teamPostfix.TrimStart('-', ' ').Trim();
    }

    public static LoggedChangeModel CreateLoggedChangeModel(
        int infoSiteMatchId,
        string modifiedBy,
        string action,
        string oldValue = null,
        string newValue = null)
    {
        return new LoggedChangeModel
        {
            InfoSiteMatchId = infoSiteMatchId,
            UpdateTypeId = null,
            ModifiedBy = modifiedBy,
            Action = action,
            OldValue = oldValue,
            NewValue = newValue,
            UpDateTime = Now.ToGmt8(),
        };
    }

    public static async Task SaveEventLogAsync(Repository repository, List<LoggedChangeModel> logData, CancellationToken cancellationToken = default)
    {
        if (logData is not { Count: > 0 })
        {
            return;
        }

        var xml = XmlConverter.Serialize(logData);
        var criteria = new SaveEditEventLogsCriteria { Xml = xml };
        await repository.SaveEventLog(criteria).ConfigureAwait(false);
    }

    public static string FormatSpread(decimal? spread)
        => spread.HasValue ? $"{Math.Round(spread.Value * SpreadPercentageMultiplier)}%" : string.Empty;

    public static string FormatMaxbet(decimal? maxbet)
        => maxbet.HasValue ? $"{Math.Round(maxbet.Value / MaxbetKiloDivisor)}K" : string.Empty;
}
