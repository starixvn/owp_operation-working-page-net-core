using OWP.Event.EventShared.Const;
using OWP.Event.EventShared.Models;
using System.Globalization;

namespace OWP.Event.EventShared.Helper;

public static class EventHelper
{
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

    public static string ToOWPMaxBetFormat(this decimal? maxbet)
    {
        return maxbet.HasValue ? (maxbet.Value / 1000).ToString("0.####", CultureInfo.InvariantCulture) + "K" : string.Empty;
    }

    public static string ToOWPSpreadFormat(this decimal? spread)
    {
        return spread.HasValue ? (spread.Value * 100).ToString("0.##", CultureInfo.InvariantCulture) : string.Empty;
    }

    public static EventInformation BuildEventInfoInPair(
        IEnumerable<Assignment> assignments,
        IEnumerable<MatchSetting> matchSettings,
        IEnumerable<Television> televisions,
        IEnumerable<MatchOffer> matchOffers,
        IEnumerable<TicketInfo> tickets,
        IEnumerable<MatchInfo> matchInfo)
    {
        var (assignmentsPair, minorAssignmentsPair) = GetAssignmentsInPair(assignments);
        var (matchSettingsPair, minorMatchSettingsPair) = GetMatchSettingsInPair(matchSettings);
        var televisionsPair = GetTelevisionsPair(televisions);
        var matchOffersPair = GetMatchOffersPair(matchOffers);
        var ticketsPair = GetTicketsPair(tickets);

        return new EventInformation
        {
            MatchInfo = matchInfo,
            AssignmentsPair = assignmentsPair,
            MinorAssignmentsPair = minorAssignmentsPair,
            MatchSettingsPair = matchSettingsPair,
            MinorMatchSettingsPair = minorMatchSettingsPair,
            TelevisionsPair = televisionsPair,
            MatchOffersPair = matchOffersPair,
            TicketInfosPair = ticketsPair
        };
    }

    private static Dictionary<int, List<Television>> GetTelevisionsPair(IEnumerable<Television> televisions)
        => televisions.GroupBy(a => a.OWPMatchId).ToDictionary(a => a.Key, a => a.ToList());

    private static Dictionary<int, List<MatchOffer>> GetMatchOffersPair(IEnumerable<MatchOffer> matchOffers)
        => matchOffers.GroupBy(a => a.OWPMatchId).ToDictionary(a => a.Key, a => a.ToList());

    private static Dictionary<int, List<TicketInfo>> GetTicketsPair(IEnumerable<TicketInfo> ticketInfos)
        => ticketInfos.GroupBy(a => a.OwpMatchId).ToDictionary(a => a.Key, a => a.ToList());

    private static (Dictionary<int, List<Assignment>>, Dictionary<int, List<Assignment>>) GetAssignmentsInPair(
        IEnumerable<Assignment> assignments)
    {
        var assignmentList = assignments.ToList();
        var assignmentsPair = assignmentList
            .GroupBy(a => a.OWPMatchId)
            .ToDictionary(a => a.Key, a => a.ToList());
        var minorAssignmentsPair = assignmentList
            .Where(x => x.ParentMatchId > 0)
            .GroupBy(a => a.ParentMatchId).ToDictionary(a => a.Key, a => a.ToList());

        return (assignmentsPair, minorAssignmentsPair);
    }

    private static (Dictionary<int, List<MatchSetting>>, Dictionary<int, List<MatchSetting>>) GetMatchSettingsInPair(
        IEnumerable<MatchSetting> matchSettings)
    {
        var matchSettingList = matchSettings.ToList();
        var matchSettingsPair = matchSettingList
            .GroupBy(a => a.OWPMatchId)
            .ToDictionary(a => a.Key, a => a.ToList());
        var minorMatchSettingsPair = matchSettingList
            .Where(x => x.ParentMatchId > 0)
            .GroupBy(a => a.ParentMatchId).ToDictionary(a => a.Key, a => a.ToList());

        return (matchSettingsPair, minorMatchSettingsPair);
    }

    public static bool IsSelectAllMatchType(List<string> matchType)
     => matchType.Exists(x => x == EventConst.MatchReal)
         && matchType.Exists(x => x == EventConst.MatchVirtual);

    public static string ToOWPTimeFormat(this TimeSpan time)
    {
        const string TimeFormat = "hh:mm tt";
        var date = new DateTimeOffset(1, 1, 1, time.Hours, time.Minutes, 0, TimeSpan.Zero);

        return date.ToString(TimeFormat, CultureInfo.InvariantCulture);
    }
}