using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.Constants;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Services;

public interface CategorizeRealMatchServices
{

}

public class CategorizeRealMatchServicesImp : CategorizeRealMatchServices
{
    public void Categorize(IEnumerable<WorkingSchedule> schedules, MatchInfo matchInfo)
    {
        foreach (var schedule in schedules)
        {
            var (type, explanation) = GetCategoryType(schedule, matchInfo);
            schedule.CategoryType = type;
            schedule.Explanation = explanation;
        }
    }

    public (CategoryType, string) GetCategoryType(WorkingSchedule schedule, MatchInfo matchInfo)
    {
        var userInfo = schedule.UserInfo;
        if (!IsHaveWorkingShiftInRangeTime(userInfo, matchInfo))
        {
            return (CategoryType.Busy, "Busy");
        }

        if (IsFreeInRangeTime(schedule, matchInfo))
        {
            return (CategoryType.FullyAvailable, $"100% free from {matchInfo.KOTime} to {matchInfo.EndTime}");
        }

        if (!matchInfo.IsSingleMatch)
        {
            return (CategoryType.Busy, "Busy");
        }

        var sameKOTimeAssignments = schedule.CategorizedAssignments.Where(x => x.KOTime.CompareTo(matchInfo.KOTime) == 0).ToList();
        if (!sameKOTimeAssignments.Any())
        {
            return (CategoryType.Busy, "Busy");
        }

        var (isHaveLinkNGame, explanation) = IsHaveAssignmentLinkNGame(sameKOTimeAssignments, userInfo);
        if (isHaveLinkNGame)
        {
            return (CategoryType.FullyAvailable, explanation);
        }

        return (CategoryType.Busy, "Busy");
    }

    public bool IsHaveWorkingShiftInRangeTime(UserInfo userInfo, MatchInfo matchInfo)
        => userInfo.ShiftStartTime <= matchInfo.KOTime && userInfo.ShiftEndTime >= matchInfo.EndTime;

    public bool IsFreeInRangeTime(WorkingSchedule schedule, MatchInfo matchInfo)
    {
        var fromTime = matchInfo.KOTime;
        var toTime = matchInfo.EndTime;

        var isFreeInRangeTime = !schedule.CategorizedAssignments.Any(x =>
            (x.EventDateTime < fromTime && x.EndTime < fromTime) ||
            (x.EventDateTime > toTime && x.EndTime > toTime));

        return isFreeInRangeTime;
    }

    public bool IsPartiallyFreeInRangeTime(WorkingSchedule schedule, MatchInfo matchInfo)
    {
        var fromTime = matchInfo.KOTime;
        var toTime = matchInfo.EndTime;
        if (matchInfo.BettypeId is Bettype.FirstHalfHandicap or Bettype.FirstHalfOverUnder)
        {

        }

        var isFreeInRangeTime = !schedule.CategorizedAssignments.Any(x =>
            (x.EventDateTime < fromTime && x.EndTime < fromTime) ||
            (x.EventDateTime > toTime && x.EndTime > toTime));

        return false;
    }

    public static (bool, string) IsHaveAssignmentLinkNGame(List<AssignmentModel> assignments, UserInfo userInfo)
    {
        var busy = (false, "busy");
        switch (userInfo.MixMatchCapacity)
        {
            case MixMatchCapacity.MaxGame2G:
                {
                    var linkNGames = assignments.Any(x => x.MatchType is MatchType.RealMatchLink1G);
                    return linkNGames ? (true, "doing 1 of 2 games in capacity") : busy;
                }
            case MixMatchCapacity.MaxGame3G:
                {
                    var linkNGames = assignments.Any(x => x.MatchType is MatchType.RealMatchLink2G);
                    if (linkNGames)
                    {
                        return (true, "doing 2 of 3 games in capacity");
                    }

                    linkNGames = assignments.Any(x => x.MatchType is MatchType.RealMatchLink1G);
                    return linkNGames ? (true, "doing 1 of 3 games in capacity") : busy;
                }
            case MixMatchCapacity.MaxGame1G:
            case MixMatchCapacity.None:
            default:
                return (false, "busy");
        }
    }

    //public string GetMixMatchExplanation(MixMatchCapacity mixMatchCapacity, MatchType? doingMatchType)
    //    => (capacity: mixMatchCapacity, doingMatchType) switch
    //    {
    //        (MixMatchCapacity.MaxGame2G, MatchType.RealMatchLink1G) => "doing 1 of 2 games in capacity",
    //        (MixMatchCapacity.MaxGame3G, MatchType.RealMatchLink1G) => "doing 1 of 3 games in capacity",
    //        (MixMatchCapacity.MaxGame3G, MatchType.RealMatchLink2G) => "doing 2 of 3 games in capacity",
    //        _ => string.Empty
    //    };
}
