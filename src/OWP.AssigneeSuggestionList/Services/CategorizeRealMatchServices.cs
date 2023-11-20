using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using OWP.Constants;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Services;

public interface CategorizeRealMatchServices
{
    void Categorize(List<WorkingSchedule> schedules, MatchInfo matchInfo);
}

public class CategorizeRealMatchServicesImp : CategorizeRealMatchServices
{
    public void Categorize(List<WorkingSchedule> schedules, MatchInfo matchInfo)
    {
        foreach (var schedule in schedules)
        {
            var assignments = schedule.CategorizedAssignments.ToList();
            var (oTPreShift, oTPostShift) = Helper.Utils.HasOT(assignments, schedule);
            var assigneeType = GetAssigneeType(schedule.UserInfo, assignments, matchInfo);
            var (categoryType, explanation) = GetCategoryTypeAndExplanation(assigneeType, matchInfo);

            schedule.UserInfo.OTPostShift = oTPreShift;
            schedule.UserInfo.OTPostShift = oTPostShift;
            schedule.CategoryType = categoryType;
            schedule.UserInfo.Explanation = explanation;
        }
    }

    private AssigneeCategory GetAssigneeType(UserInfo userInfo, List<AssignmentModel> assignments, MatchInfo matchInfo)
    {
        var kOTime = matchInfo.KOTime;
        var endTime = matchInfo.EndTime;

        if (!userInfo.HasWorkingShiftInRangeTime(kOTime, endTime))
        {
            return IsPartiallyFreeInRangeTime(userInfo, assignments, matchInfo)
                ? AssigneeCategory.PartiallyFree
                : AssigneeCategory.Busy;
        }

        if (assignments.IsFreeInRangeTime(kOTime, endTime))
        {
            return AssigneeCategory.Free;
        }

        if (IsPartiallyFreeInRangeTime(userInfo, assignments, matchInfo))
        {
            return AssigneeCategory.PartiallyFree;
        }

        return matchInfo.IsSingleMatch
            ? AssigneeCategory.Busy
            : HasAssignmentLinkNGame(userInfo, assignments, matchInfo);
    }

    private static bool IsPartiallyFreeInRangeTime(UserInfo userInfo, List<AssignmentModel> assignments, MatchInfo matchInfo)
    {
        var kOTime = matchInfo.KOTime;
        var minToTime = kOTime.AddMinutes(60);

        var hasWsInPartiallyRangeTime = userInfo.ShiftStartTime <= kOTime && userInfo.ShiftEndTime >= minToTime;
        if (hasWsInPartiallyRangeTime && matchInfo.BettypeId is Bettype.Handicap or Bettype.OverUnder)
        {
            return !assignments.Exists(x => x.EventDateTime < minToTime && kOTime < x.EndTime);
        }

        return false;
    }

    private AssigneeCategory HasAssignmentLinkNGame(UserInfo userInfo, List<AssignmentModel> assignments, MatchInfo matchInfo)
    {
        var sameKoTimeAssignments = assignments.GetSameKOTimeAssignments(matchInfo);
        var isAssignmentWithin10Minutes = HasAssignmentWithin10MinutesDiff(assignments, matchInfo);
        if (!sameKoTimeAssignments.Any() && !isAssignmentWithin10Minutes)
        {
            return AssigneeCategory.Busy;
        }

        var hasLink1G = sameKoTimeAssignments.Exists(x => x.MatchType is MatchType.RealMatchLink1G)
                        || isAssignmentWithin10Minutes;

        var hasLink2G = HasLink2G(matchInfo, sameKoTimeAssignments);

        return (userInfo.MixMatchCapacity, hasLink1G, hasLink2G) switch
        {
            (MixMatchCapacity.MaxGame2G, true, _) => AssigneeCategory.Capacity2GDoing1G,
            (MixMatchCapacity.MaxGame3G, true, _) => AssigneeCategory.Capacity3GDoing1G,
            (MixMatchCapacity.MaxGame3G, false, true) => AssigneeCategory.Capacity3GDoing2G,
            _ => AssigneeCategory.Busy
        };
    }

    private static bool HasLink2G(MatchInfo matchInfo, List<AssignmentModel> sameKoTimeAssignments)
    => matchInfo.MaxbetHDP <= CategoryConstants.MAX_BET_5K &&
       sameKoTimeAssignments.Exists(x =>
           x.MatchType is MatchType.RealMatchLink2G &&
           x.GroupAssignmentModels.TrueForAll(y =>
               y.MaxbetHDP is null or <= CategoryConstants.MAX_BET_5K &&
               !y.IsSingleMatch)
       );

    private static bool HasAssignmentWithin10MinutesDiff(IEnumerable<AssignmentModel> assignments,
        MatchInfo matchInfo)
        => assignments
            .Where(x =>
                !x.IsSaba &&
                (x.EventDateTime - matchInfo.KOTime).Duration() <= TimeSpan.FromMinutes(10) &&
                !x.IsSingleMatch).Any(x => x.MatchType is MatchType.RealMatchLink1G);

    private static (CategoryType, string) GetCategoryTypeAndExplanation(AssigneeCategory category, MatchInfo matchInfo)
        => category switch
        {
            AssigneeCategory.Free => (CategoryType.FullyAvailable, CategorizeHelper.FormatExplanation(ExplanationConstants.FREE, matchInfo)),
            AssigneeCategory.Capacity2GDoing1G => (CategoryType.FullyAvailable, ExplanationConstants.CAPACITY_2G_DOING_1G),
            AssigneeCategory.Capacity3GDoing2G => (CategoryType.FullyAvailable, ExplanationConstants.CAPACITY_3G_DOING_2G),
            AssigneeCategory.Capacity3GDoing1G => (CategoryType.FullyAvailable, ExplanationConstants.CAPACITY_3G_DOING_1G),
            AssigneeCategory.PartiallyFree => (CategoryType.PartiallyAvailable, CategorizeHelper.FormatExplanation(ExplanationConstants.PARTIALLY_AVAILABLE, matchInfo, true)),
            _ => (CategoryType.Busy, null)
        };


}
