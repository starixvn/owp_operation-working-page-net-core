using OWP.Extensions;
using OWP.LiveGameList.Enum;
using OWP.LiveGameList.Models;

namespace OWP.LiveGameList.Builder;

public static class MatchSettingBuilder
{
    public static MatchSettingModel Build(MatchInfo matchInfo, Dictionary<int, List<MatchSetting>> matchSettingsDict, Dictionary<int, List<MatchSetting>> minorMatchSettingsDict)
    {
        var matchSettings = matchSettingsDict.GetValue(matchInfo.OWPMatchId, new List<MatchSetting>());
        var generatedMatchSettings = minorMatchSettingsDict.GetValue(matchInfo.OWPMatchId, new List<MatchSetting>());

        return new MatchSettingModel
        {
            Handicap = GetMatchSetting(matchSettings, matchInfo, (int)MajorBetType.Handicap),
            OverUnder = GetMatchSetting(matchSettings, matchInfo, (int)MajorBetType.OverUnder),
            OneXTwo = GetMatchSetting(matchSettings, matchInfo, (int)MajorBetType.OneXTwo),
            Handicap1st = GetMatchSetting(matchSettings, matchInfo, (int)MajorBetType.Handicap1St),
            OverUnder1st = GetMatchSetting(matchSettings, matchInfo, (int)MajorBetType.OverUnder1St),
            OneXTwo1st = GetMatchSetting(matchSettings, matchInfo, (int)MajorBetType.OneXTwo1St),
            Specific15Mins = GetMatchSettingForSpecific15Mins(generatedMatchSettings, matchInfo),
            Specific10Mins =
                GetMatchSettingForSpecialBettype(generatedMatchSettings, matchInfo, (int)MinorBetType.Specific10Mins),
            FastMarket =
                GetMatchSettingForSpecialBettype(generatedMatchSettings, matchInfo, (int)MinorBetType.FastMarket)
        };
    }

    private static MatchSetting GetMatchSetting(IEnumerable<MatchSetting> matchSettings, MatchInfo matchInfo, int betTypeId)
        => matchSettings.FirstOrDefault(x => x.OWPMatchId == matchInfo.OWPMatchId && x.BettypeId == betTypeId)
           ?? new MatchSetting();

    private static MatchSetting GetMatchSettingForSpecialBettype(IEnumerable<MatchSetting> generatedMatchSettings, MatchInfo matchInfo, int bettypeId)
        => GetGeneratedMatchSettingsForMainMatch(generatedMatchSettings, matchInfo, bettypeId);

    private static MatchSetting GetGeneratedMatchSettingsForMainMatch(IEnumerable<MatchSetting> generatedMatchSettings, MatchInfo matchInfo, int bettypeId)
        => generatedMatchSettings.FirstOrDefault(x => x.MinorBettypeId == bettypeId && x.ParentMatchId == matchInfo.OWPMatchId)
            ?? new MatchSetting();

    private static MatchSetting GetMatchSettingForSpecific15Mins(IEnumerable<MatchSetting> generatedMatchSettings, MatchInfo matchInfo)
    {
        var result = !matchInfo.IsMinorMatch
            ? GetGeneratedMatchSettingsForMainMatch(generatedMatchSettings, matchInfo, (int)MinorBetType.Specific15Mins)
            : GetMatchSettingForSpecific15MinsOfSpecialProduct(generatedMatchSettings, matchInfo);

        return result ?? new MatchSetting();
    }

    private static MatchSetting GetMatchSettingForSpecific15MinsOfSpecialProduct(IEnumerable<MatchSetting> generatedMatchSettings, MatchInfo matchInfo)
    {
        var refMinorBettypeId = matchInfo.MinorBettypeId switch
        {
            (int)MinorBetType.NoOfCorner => (int)MinorBetType.Specific15MinsCorners,
            (int)MinorBetType.TotalBooking => (int)MinorBetType.Specific15MinsTotalBookings,
            _ => 0
        };

        if (refMinorBettypeId == 0)
        {
            return new MatchSetting();
        }

        return generatedMatchSettings.FirstOrDefault(x =>
            x.MinorBettypeId == refMinorBettypeId &&
            x.BettypeId == (int)MajorBetType.Handicap &&
            x.ParentMatchId == matchInfo.ParentMatchId) ?? new MatchSetting();
    }
}

