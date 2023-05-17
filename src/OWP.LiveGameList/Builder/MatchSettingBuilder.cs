using OWP.LiveGameList.Enum;
using OWP.LiveGameList.Models;

namespace OWP.LiveGameList.Builder;

public static class MatchSettingBuilder
{
    public static MatchSettingModel Build(MatchInfo matchInfo, IList<MatchSetting> matchSettings)
        => new()
        {
            Handicap = GetMatchSetting(matchSettings, matchInfo, (int)MajorBetType.Handicap),
            OverUnder = GetMatchSetting(matchSettings, matchInfo, (int)MajorBetType.OverUnder),
            OneXTwo = GetMatchSetting(matchSettings, matchInfo, (int)MajorBetType.OneXTwo),
            Handicap1st = GetMatchSetting(matchSettings, matchInfo, (int)MajorBetType.Handicap1St),
            OverUnder1st = GetMatchSetting(matchSettings, matchInfo, (int)MajorBetType.OverUnder1St),
            OneXTwo1st = GetMatchSetting(matchSettings, matchInfo, (int)MajorBetType.OneXTwo1St),
            Specific15Mins =
                GetMatchSettingForSpecialBettype(matchSettings, matchInfo, (int)MinorBetType.Specific15Mins),
            Specific10Mins =
                GetMatchSettingForSpecialBettype(matchSettings, matchInfo, (int)MinorBetType.Specific10Mins),
            FastMarket = GetMatchSettingForSpecialBettype(matchSettings, matchInfo, (int)MinorBetType.FastMarket)
        };

    private static MatchSetting GetMatchSetting(IEnumerable<MatchSetting> matchSettings, MatchInfo matchInfo, int betTypeId)
        => matchSettings.FirstOrDefault(x => x.OWPMatchId == matchInfo.OWPMatchId && x.BettypeId == betTypeId)
           ?? new MatchSetting();

    private static MatchSetting GetMatchSettingForSpecialBettype(
        IEnumerable<MatchSetting> matchSettings,
        MatchInfo matchInfo,
        int bettypeId)
    {
        var result = !matchInfo.IsMinorMatch
            ? matchSettings.FirstOrDefault(x => x.MinorBettypeId == bettypeId && x.ParentMatchId == matchInfo.OWPMatchId)
            : GetMatchSettingForSpecific15Mins(matchSettings, matchInfo, bettypeId);

        return result ?? new MatchSetting();
    }

    private static MatchSetting GetMatchSettingForSpecific15Mins(IEnumerable<MatchSetting> matchSettings, MatchInfo matchInfo, int bettypeId)
        => (matchInfo.MinorBettypeId, bettypeId) switch
        {
            ((int)MinorBetType.NoOfCorner, (int)MinorBetType.Specific15Mins) =>
                matchSettings.FirstOrDefault(x =>
                    x.MinorBettypeId == (int)MinorBetType.Specific15MinsCorners &&
                    x.BettypeId == (int)MajorBetType.Handicap && x.ParentMatchId == matchInfo.ParentMatchId),
            ((int)MinorBetType.TotalBooking, (int)MinorBetType.Specific15Mins) =>
                matchSettings.FirstOrDefault(x =>
                    x.MinorBettypeId == (int)MinorBetType.Specific15MinsTotalBookings &&
                    x.BettypeId == (int)MajorBetType.Handicap && x.ParentMatchId == matchInfo.ParentMatchId),
            _ => new MatchSetting()
        };
}

