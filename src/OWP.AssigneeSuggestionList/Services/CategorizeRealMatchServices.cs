using OWP.AssigneeSuggestionList.Constant;
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
            var assigneeType = GetAssigneeType(schedule.UserInfo, assignments, matchInfo);
            var (categoryType, explanation) = GetCategoryTypeAndExplanation(assigneeType, matchInfo);

            schedule.CategoryType = categoryType;
            schedule.UserInfo.Explanation = explanation;

        }
    }

    public static AssigneeCategory GetAssigneeType(UserInfo userInfo, List<AssignmentModel> assignments, MatchInfo matchInfo)
    {
        var kOTime = matchInfo.KOTime;
        var endTime = matchInfo.EndTime;

        if (!HasWorkingShiftInRangeTime(userInfo, kOTime, endTime))
        {
            return IsPartiallyFreeInRangeTime(userInfo, assignments, matchInfo)
                ? AssigneeCategory.PartiallyFree
                : AssigneeCategory.Busy;
        }

        if (IsFreeInRangeTime(assignments, kOTime, endTime))
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

    public static bool HasWorkingShiftInRangeTime(UserInfo userInfo, DateTime kOTime, DateTime endTime)
        => userInfo.ShiftStartTime <= kOTime && userInfo.ShiftEndTime >= endTime;

    public static bool IsFreeInRangeTime(List<AssignmentModel> assignments, DateTime kOTime, DateTime endTime)
    {
        var isFreeInRangeTime = !assignments.Any(x => x.EventDateTime < endTime && kOTime < x.EndTime);

        return isFreeInRangeTime;
    }

    public static bool IsPartiallyFreeInRangeTime(UserInfo userInfo, List<AssignmentModel> assignments, MatchInfo matchInfo)
    {
        var kOTime = matchInfo.KOTime;
        var minToTime = kOTime.AddMinutes(60);

        var hasWsInPartiallyRangeTime = userInfo.ShiftStartTime <= kOTime && userInfo.ShiftEndTime >= minToTime;
        if (hasWsInPartiallyRangeTime && matchInfo.BettypeId is Bettype.Handicap or Bettype.OverUnder)
        {
            return !assignments.Any(x => x.EventDateTime < minToTime && kOTime < x.EndTime);
        }

        return false;
    }

    public static AssigneeCategory HasAssignmentLinkNGame(UserInfo userInfo, List<AssignmentModel> assignments, MatchInfo matchInfo)
    {
        var sameKoTimeAssignments = GetSameKoTimeAssignments(assignments, matchInfo);
       
        if (!sameKoTimeAssignments.Any())
        {
            return AssigneeCategory.Busy;
        }

        var hasLink1G = sameKoTimeAssignments.Any(x => x.MatchType is MatchType.RealMatchLink1G) 
                        || HasAssignmentWithin10MinutesDiff(assignments,matchInfo);
        var hasLink2G = matchInfo.MaxbetHDP <= CategoryConstants.MAX_BET_5K &&
            sameKoTimeAssignments.Any(x =>
                x.MatchType is MatchType.RealMatchLink2G &&
                x.GroupAssignmentModels.All(y =>
                    y.MaxbetHDP is null or <= CategoryConstants.MAX_BET_5K &&
                    !y.IsSingleMatch)
                );

        return (userInfo.MixMatchCapacity, hasLink1G, hasLink2G) switch
        {
            (MixMatchCapacity.MaxGame2G, true, _) => AssigneeCategory.Capacity2GDoing1G,
            (MixMatchCapacity.MaxGame3G, true, _) => AssigneeCategory.Capacity3GDoing1G,
            (MixMatchCapacity.MaxGame3G, false, true) => AssigneeCategory.Capacity3GDoing2G,
            _ => AssigneeCategory.Busy
        };
    }

    private static bool HasAssignmentWithin10MinutesDiff(IEnumerable<AssignmentModel> assignments,
        MatchInfo matchInfo)
        => assignments
            .Where(x =>
                !x.IsSaba &&
                (x.EventDateTime - matchInfo.KOTime).Duration() == TimeSpan.FromMinutes(10) &&
                !x.IsSingleMatch).Any(x => x.MatchType is MatchType.RealMatchLink1G);
    
    private static List<AssignmentModel> GetSameKoTimeAssignments(IEnumerable<AssignmentModel> assignments, MatchInfo matchInfo)
        => assignments
            .Where(x =>
                !x.IsSaba &&
                x.EventDateTime.CompareTo(matchInfo.KOTime) == 0 &&
                !x.IsSingleMatch)
            .ToList();

    public static (CategoryType, string) GetCategoryTypeAndExplanation(AssigneeCategory category, MatchInfo matchInfo)
        => category switch
        {
            AssigneeCategory.Free => (CategoryType.FullyAvailable, FormatExplanation(ExplanationConstants.FREE, matchInfo)),
            AssigneeCategory.Capacity2GDoing1G => (CategoryType.FullyAvailable, ExplanationConstants.CAPACITY_2G_DOING_1G),
            AssigneeCategory.Capacity3GDoing2G => (CategoryType.FullyAvailable, ExplanationConstants.CAPACITY_3G_DOING_2G),
            AssigneeCategory.Capacity3GDoing1G => (CategoryType.FullyAvailable, ExplanationConstants.CAPACITY_3G_DOING_1G),
            AssigneeCategory.PartiallyFree => (CategoryType.PartiallyAvailable, FormatExplanation(ExplanationConstants.PARTIALLY_AVAILABLE, matchInfo, true)),
            _ => (CategoryType.Busy, null)
        };

    public static string FormatExplanation(string text, MatchInfo matchInfo, bool isPartially = false)
    {
        const string timeFormat = "hh:mm tt";
        var toTime = isPartially ? matchInfo.KOTime.AddHours(1) : matchInfo.EndTime;
        return string.Format(text, matchInfo.KOTime.ToString(timeFormat), toTime.ToString(timeFormat));
    }
}
