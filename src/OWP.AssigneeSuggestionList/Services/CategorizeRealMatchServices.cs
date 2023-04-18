using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.Constants;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.Services;

public interface CategorizeRealMatchServices
{
    void Categorize(IEnumerable<WorkingSchedule> schedules, MatchInfo matchInfo);
}

public class CategorizeRealMatchServicesImp : CategorizeRealMatchServices
{
    public void Categorize(IEnumerable<WorkingSchedule> schedules, MatchInfo matchInfo)
    {
        foreach (var schedule in schedules)
        {
            var realAssignments = schedule.CategorizedAssignments.Where(x => !x.IsSaba).ToList();
            var assigneeType = GetAssigneeType(schedule.UserInfo, realAssignments, matchInfo);
            var (categoryType, explanation) = GetCategoryTypeAndExplanation(assigneeType, matchInfo);

            schedule.CategoryType = categoryType;
            schedule.Explanation = explanation;
        }
    }

    public AssigneeCategory GetAssigneeType(UserInfo userInfo, List<AssignmentModel> assignments, MatchInfo matchInfo)
    {
        var kOTime = matchInfo.KOTime;
        var endTime = matchInfo.EndTime;

        if (!IsHaveWorkingShiftInRangeTime(userInfo, kOTime, endTime))
        {
            return AssigneeCategory.Busy;
        }

        if (IsFreeInRangeTime(assignments, kOTime, endTime))
        {
            return AssigneeCategory.Free;
        }

        if (IsPartiallyFreeInRangeTime(assignments, kOTime, matchInfo.BettypeId))
        {
            return AssigneeCategory.PartiallyFree;
        }

        if (!matchInfo.IsSingleMatch)
        {
            IsHaveAssignmentLinkNGame(userInfo, assignments, matchInfo);
        }

        return AssigneeCategory.Busy;
    }

    public static bool IsHaveWorkingShiftInRangeTime(UserInfo userInfo, DateTime kOTime, DateTime endTime)
        => userInfo.ShiftStartTime <= kOTime && userInfo.ShiftEndTime >= endTime;

    public static bool IsFreeInRangeTime(List<AssignmentModel> assignments, DateTime kOTime, DateTime endTime)
    {
        var isFreeInRangeTime = !assignments.Any(x =>
            (x.EventDateTime < kOTime && x.EndTime < kOTime) ||
            (x.EventDateTime > endTime && x.EndTime > endTime));

        return isFreeInRangeTime;
    }

    public static bool IsPartiallyFreeInRangeTime(List<AssignmentModel> assignments, DateTime kOTime, int bettypeId)
    {
        var minToTime = kOTime.AddMinutes(60);
        if (bettypeId is Bettype.Handicap or Bettype.OverUnder)
        {
            return !assignments.Any(x =>
                (x.EventDateTime < kOTime && x.EndTime < kOTime) ||
                (x.EventDateTime > minToTime && x.EndTime > minToTime));
        }

        return false;
    }

    public static AssigneeCategory IsHaveAssignmentLinkNGame(UserInfo userInfo, List<AssignmentModel> assignments, MatchInfo matchInfo)
    {
        var sameKOTimeAssignments = assignments
            .Where(x => x.EventDateTime.CompareTo(matchInfo.KOTime) == 0).ToList();
        if (!sameKOTimeAssignments.Any())
        {
            return AssigneeCategory.Busy;
        }

        switch (userInfo.MixMatchCapacity)
        {
            case MixMatchCapacity.MaxGame2G:
                {
                    return sameKOTimeAssignments.Any(x => x.MatchType is MatchType.RealMatchLink1G)
                        ? AssigneeCategory.Capacity2GDoing1G
                        : AssigneeCategory.Busy;
                }
            case MixMatchCapacity.MaxGame3G:
                {
                    if (matchInfo.MaxbetHDP <= CategoryConstants.MAX_BET_5K &&
                        sameKOTimeAssignments.Any(x => x.MatchType is MatchType.RealMatchLink2G))
                    {
                        return AssigneeCategory.Capacity3GDoing2G;
                    }

                    return sameKOTimeAssignments.Any(x => x.MatchType is MatchType.RealMatchLink1G)
                        ? AssigneeCategory.Capacity3GDoing1G
                        : AssigneeCategory.Busy;
                }
            case MixMatchCapacity.MaxGame1G:
            case MixMatchCapacity.None:
            default:
                return AssigneeCategory.Busy;
        }
    }

    public static (CategoryType, string) GetCategoryTypeAndExplanation(AssigneeCategory category, MatchInfo matchInfo)
        => category switch
        {
            AssigneeCategory.Free => (CategoryType.FullyAvailable, FormatExplanation(ExplanationConstants.FREE, matchInfo)),
            AssigneeCategory.Capacity2GDoing1G => (CategoryType.FullyAvailable, ExplanationConstants.CAPACITY_2G_DOING_1G),
            AssigneeCategory.Capacity3GDoing2G => (CategoryType.FullyAvailable, ExplanationConstants.CAPACITY_3G_DOING_2G),
            AssigneeCategory.Capacity3GDoing1G => (CategoryType.FullyAvailable, ExplanationConstants.CAPACITY_3G_DOING_1G),
            AssigneeCategory.PartiallyFree => (CategoryType.FullyAvailable, FormatExplanation(ExplanationConstants.PARTIALLY_AVAILABLE, matchInfo)),
            _ => (CategoryType.FullyAvailable, ExplanationConstants.BUSY),
        };

    public static string FormatExplanation(string explanationString, MatchInfo matchInfo)
        => string.Format(explanationString, matchInfo.KOTime.ToString("hh:mm"), matchInfo.EndTime.ToString("hh:mm"));
}
