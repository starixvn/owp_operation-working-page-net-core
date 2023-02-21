
using OWP.TaskList.Enumerations;
using OWP.TaskList.Models;

namespace OWP.TaskList.Builders;

public static class MatchSettingBuilder
{
    public static MatchSettingModel Build(MatchInfo matchInfo, IEnumerable<MatchSetting> matchSettings)
    {
        var matchSetting = new MatchSettingModel
        {
            Handicap = GetMatchSetting(matchSettings, matchInfo, (int)MajorBettype.Handicap),
            OverUnder = GetMatchSetting(matchSettings, matchInfo, (int)MajorBettype.OverUnder),
            OneXTwo = GetMatchSetting(matchSettings, matchInfo, (int)MajorBettype.OneXTwo),
            Handicap1st = GetMatchSetting(matchSettings, matchInfo, (int)MajorBettype.Handicap1St),
            OverUnder1st = GetMatchSetting(matchSettings, matchInfo, (int)MajorBettype.OverUnder1St),
            OneXTwo1st = GetMatchSetting(matchSettings, matchInfo, (int)MajorBettype.OneXTwo1St),
            Specific15Mins = GetMatchSettingForSpecialBettype(matchSettings, matchInfo, (int)MinorBettype.Specific15Mins),
            Specific10Mins = GetMatchSettingForSpecialBettype(matchSettings, matchInfo, (int)MinorBettype.Specific10Mins),
            FastMarket = GetMatchSettingForSpecialBettype(matchSettings, matchInfo, (int)MinorBettype.FastMarket)
        };
        return matchSetting;
    }
    private static MatchSetting GetMatchSetting(
        IEnumerable<MatchSetting> matchSettings,
        MatchInfo matchInfo,
        int bettypeId)
    {
        var result = matchSettings.FirstOrDefault(x => x.OWPMatchId == matchInfo.OWPMatchId && x.BettypeId == bettypeId);

        return result ?? new MatchSetting();
    }
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
    private static MatchSetting GetMatchSettingForSpecific15Mins(
        IEnumerable<MatchSetting> matchSettings,
        MatchInfo matchInfo,
        int bettypeId)
    {
        MatchSetting result;

        if (matchInfo.MinorBettypeId == (int)MinorBettype.NoOfCorner && bettypeId == (int)MinorBettype.Specific15Mins)
        {
            result = matchSettings.FirstOrDefault(x => x.MinorBettypeId == (int)MinorBettype.Specific15MinsCorners
                                                       && x.BettypeId == (int)MajorBettype.Handicap
                                                       && x.ParentMatchId == matchInfo.ParentMatchId);
        }
        else if (matchInfo.MinorBettypeId == (int)MinorBettype.TotalBooking && bettypeId == (int)MinorBettype.Specific15Mins)
        {
            result = matchSettings.FirstOrDefault(x => x.MinorBettypeId == (int)MinorBettype.Specific15MinsTotalBookings
                                                       && x.BettypeId == (int)MajorBettype.Handicap
                                                       && x.ParentMatchId == matchInfo.ParentMatchId);
        }
        else
        {
            result = new MatchSetting();
        }

        return result ?? new MatchSetting();
    }
        
}