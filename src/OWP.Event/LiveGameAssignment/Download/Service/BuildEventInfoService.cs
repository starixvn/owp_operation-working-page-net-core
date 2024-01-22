namespace OWP.Event.LiveGameAssignment.Download.Service;

using OWP.Enums;
using OWP.Event.EventShared.Enum;
using OWP.Event.EventShared.Helper;
using OWP.Event.LiveGameAssignment.EventPicking.Model;
using System.Collections.Generic;

public interface BuildEventInfoService
{
    List<EventInfo> BuildEventInfo(List<EventInfo> eventInfos, List<MatchAssignment> assignments, List<MatchSetting> matchSettings);
}

public class BuildEventInfoServiceImp : BuildEventInfoService
{
    public List<EventInfo> BuildEventInfo(List<EventInfo> eventInfos, List<MatchAssignment> assignments, List<MatchSetting> matchSettings)
    {
        foreach (var match in eventInfos)
        {
            match.TraderHDP = GetTrader(assignments, match.OWPMatchId, (int)MajorBetType.Handicap);
            match.TraderOU = GetTrader(assignments, match.OWPMatchId, (int)MajorBetType.OverUnder);
            match.Trader1HHDP = GetTrader(assignments, match.OWPMatchId, (int)MajorBetType.Handicap1St);
            match.Trader1HOU = GetTrader(assignments, match.OWPMatchId, (int)MajorBetType.OverUnder1St);
            match.Trader1X2 = GetTrader(assignments, match.OWPMatchId, (int)MajorBetType.OneXTwo);
            match.LA = GetTrader(assignments, match.OWPMatchId, (int)MajorBetType.La2Nd);
            match.Trader1H1X2 = GetTrader(assignments, match.OWPMatchId, (int)MajorBetType.OneXTwo1St);

            match.OtherTradersHDP = GetTrader(assignments, match.OWPMatchId, (int)MajorBetType.Handicap, AssignmentType.Others);
            match.OtherTradersOU = GetTrader(assignments, match.OWPMatchId, (int)MajorBetType.OverUnder, AssignmentType.Others);
            match.OtherTraders1HHDP = GetTrader(assignments, match.OWPMatchId, (int)MajorBetType.Handicap1St, AssignmentType.Others);
            match.OtherTraders1HOU = GetTrader(assignments, match.OWPMatchId, (int)MajorBetType.OverUnder1St, AssignmentType.Others);
            match.OtherTraders1X2 = GetTrader(assignments, match.OWPMatchId, (int)MajorBetType.OneXTwo, AssignmentType.Others);
            match.OtherLA = GetTrader(assignments, match.OWPMatchId, (int)MajorBetType.La2Nd, AssignmentType.Others);
            match.OtherTraders1H1X2 = GetTrader(assignments, match.OWPMatchId, (int)MajorBetType.OneXTwo1St, AssignmentType.Others);

            if (match.MinorBettypeId == null)
            {
                match.FM = GetTraderGeneratedOfMainMatch(assignments, match.OWPMatchId, (int)MinorBetType.FastMarket);
                match.AIO = GetTraderGeneratedOfMainMatch(assignments, match.OWPMatchId, (int)MinorBetType.AllInOne);
                match.X15mins = GetTraderGeneratedOfMainMatch(assignments, match.OWPMatchId, (int)MinorBetType.Specific15Mins);
                match.X10mins = GetTraderGeneratedOfMainMatch(assignments, match.OWPMatchId, (int)MinorBetType.Specific10Mins);

                match.OtherTradersFM = GetTraderGeneratedOfMainMatch(assignments, match.OWPMatchId, (int)MinorBetType.FastMarket, AssignmentType.Others);
                match.OtherTradersAIO = GetTraderGeneratedOfMainMatch(assignments, match.OWPMatchId, (int)MinorBetType.AllInOne, AssignmentType.Others);
                match.OtherTraders15mins = GetTraderGeneratedOfMainMatch(assignments, match.OWPMatchId, (int)MinorBetType.Specific15Mins, AssignmentType.Others);
                match.OtherTraders10mins = GetTraderGeneratedOfMainMatch(assignments, match.OWPMatchId, (int)MinorBetType.Specific10Mins, AssignmentType.Others);

                match.MaxBetFM = GetSettingGeneratedOfMainMatch(matchSettings, match.OWPMatchId, (int)MinorBetType.FastMarket, true);
                match.MaxBetXmins = GetSettingGeneratedOfMainMatch(matchSettings, match.OWPMatchId, (int)MinorBetType.Specific10Mins, true);

                match.SpreadFM = GetSettingGeneratedOfMainMatch(matchSettings, match.OWPMatchId, (int)MinorBetType.FastMarket, false);
                match.SpreadXmins = GetSettingGeneratedOfMainMatch(matchSettings, match.OWPMatchId, (int)MinorBetType.Specific10Mins, false);
            }
            else
            {
                match.X15mins = GetTraderForSpecific15minsOfMinorMatch(assignments, match);
                match.OtherTraders15mins = GetTraderForSpecific15minsOfMinorMatch(assignments, match, AssignmentType.Others);

                match.MaxBetXmins = GetSettingForSpecific15minsOfMinorMatch(matchSettings, match, true);
                match.SpreadXmins = GetSettingForSpecific15minsOfMinorMatch(matchSettings, match, false);
            }
        }

        return eventInfos;
    }

    private static string GetTrader(List<MatchAssignment> assignments, int owpMatchId, int bettype, AssignmentType assignmentType = AssignmentType.Main)
    {
        var assignment = assignments.Find(x => x.AssignmentType == (int)assignmentType && x.OWPMatchId == owpMatchId && x.Bettype == bettype);
        return GetTraderValue(assignmentType, assignment);
    }

    private static string GetTraderGeneratedOfMainMatch(List<MatchAssignment> assignments, int owpMatchId, int minorBettype, AssignmentType assignmentType = AssignmentType.Main)
    {
        var assignment = assignments.Find(x => x.AssignmentType == (int)assignmentType &&
                x.MinorBettypeId == minorBettype &&
                x.ParentMatchId == owpMatchId &&
                x.Bettype == (int)MajorBetType.Handicap);
        return GetTraderValue(assignmentType, assignment);
    }

    private static string GetTraderForSpecific15minsOfMinorMatch(List<MatchAssignment> assignments, EventInfo matchInfo, AssignmentType assignmentType = AssignmentType.Main)
    {
        var assignment = new MatchAssignment();
        if (matchInfo.MinorBettypeId == (int)MinorBetType.NoOfCorner)
        {
            assignment = assignments.Find(x => x.AssignmentType == (int)assignmentType &&
                    x.MinorBettypeId == (int)MinorBetType.Specific15MinsCorners &&
                    x.ParentMatchId == matchInfo.ParentMatchId &&
                    x.Bettype == (int)MajorBetType.Handicap);
        }

        if (matchInfo.MinorBettypeId == (int)MinorBetType.TotalBooking)
        {
            assignment = assignments.Find(x => x.AssignmentType == (int)assignmentType &&
                    x.MinorBettypeId == (int)MinorBetType.Specific15MinsTotalBookings &&
                    x.ParentMatchId == matchInfo.ParentMatchId &&
                    x.Bettype == (int)MajorBetType.Handicap);
        }

        return GetTraderValue(assignmentType, assignment);
    }

    private static string GetTraderValue(AssignmentType assignmentType, MatchAssignment assignment)
    {
        return assignmentType == AssignmentType.Main
            ? assignment?.Username
            : GetAssignmentNote(assignment);
    }

    private static string GetAssignmentNote(MatchAssignment assignment)
        => assignment?.AssignmentNote?.Replace("<user>", assignment.Username);

    private static string GetSettingGeneratedOfMainMatch(List<MatchSetting> matchSettings, int owpMatchId, int minorBettype, bool isMaxbet)
    {
        var matchSetting = matchSettings.Find(x => x.MinorBettypeId == minorBettype &&
                x.ParentMatchId == owpMatchId &&
                x.Bettype == (int)MajorBetType.Handicap);
        return GetSettingValue(isMaxbet, matchSetting);
    }

    private static string GetSettingForSpecific15minsOfMinorMatch(List<MatchSetting> matchSettings, EventInfo matchInfo, bool isMaxbet)
    {
        var matchSetting = new MatchSetting();
        if (matchInfo.MinorBettypeId == (int)MinorBetType.NoOfCorner)
        {
            matchSetting = matchSettings.Find(x => x.MinorBettypeId == (int)MinorBetType.Specific15MinsCorners &&
                    x.ParentMatchId == matchInfo.ParentMatchId &&
                    x.Bettype == (int)MajorBetType.Handicap);
        }

        if (matchInfo.MinorBettypeId == (int)MinorBetType.TotalBooking)
        {
            matchSetting = matchSettings.Find(x => x.MinorBettypeId == (int)MinorBetType.Specific15MinsTotalBookings &&
                    x.ParentMatchId == matchInfo.ParentMatchId &&
                    x.Bettype == (int)MajorBetType.Handicap);
        }

        return GetSettingValue(isMaxbet, matchSetting);
    }

    private static string GetSettingValue(bool isMaxbet, MatchSetting matchSetting)
    {
        return isMaxbet
            ? matchSetting?.MaxBet.ToOWPMaxBetFormat()
            : matchSetting?.Spread.ToOWPSpreadFormat();
    }
}
