
namespace OWP.AssigneeSuggestionList.Services;

using Constant;
using Helper;
using Models;
using System.Linq;

public interface CategorizeSabaMatchServices
{
    void Categorize(List<WorkingSchedule> schedules, MatchInfo matchInfo);
}

public class CategorizeSabaMatchServicesImp : CategorizeSabaMatchServices
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

    public static AssigneeSabaCategory GetAssigneeType(UserInfo userInfo, List<AssignmentModel> assignments, MatchInfo matchInfo)
    {
        var kOTime = matchInfo.KOTime;
        var endTime = matchInfo.EndTime;

        if (!userInfo.HasWorkingShiftInRangeTime(kOTime, endTime))
        {
            return AssigneeSabaCategory.Busy;
        }

        return assignments.IsFreeInRangeTime(kOTime, endTime)
            ? AssigneeSabaCategory.Free
            : HasAssignmentLinkNGame(userInfo, assignments, matchInfo);
    }

    public static AssigneeSabaCategory HasAssignmentLinkNGame(UserInfo userInfo, List<AssignmentModel> assignments, MatchInfo matchInfo)
    {
        if (string.IsNullOrEmpty(userInfo.MainBetType) || !matchInfo.IsSaba)
        {
            return AssigneeSabaCategory.Busy;
        }

        var mainBetTypeLst = userInfo.MainBetType.Split(',').ToList();
        return mainBetTypeLst switch
        {
            _ when mainBetTypeLst.ExistsValue("SB") && matchInfo.MatchType.IsMultipleType() => LinkSabaGame(assignments, matchInfo),
            _ when mainBetTypeLst.ExistsValue("5mins") && matchInfo.MatchType is MatchType.Saba5Mins => Link5MinsGame(assignments, matchInfo),
            _ => AssigneeSabaCategory.Busy
        };
    }

    public static AssigneeSabaCategory LinkSabaGame(List<AssignmentModel> assignments, MatchInfo matchInfo)
    {
        var sameKoTimeAssignments = assignments.GetSameKOTimeAssignments(matchInfo);
        if (!sameKoTimeAssignments.Any())
        {
            return AssigneeSabaCategory.Busy;
        }

        return assignments.LastOrDefault()?.MatchType switch
        {
            MatchType.SabaLink1G or MatchType.Saba16Mins => AssigneeSabaCategory.Doing1G,
            MatchType.SabaLink2G => AssigneeSabaCategory.Doing2G,
            MatchType.SabaLink3G => AssigneeSabaCategory.Doing3G,
            _ => AssigneeSabaCategory.Busy
        };
    }

    public static AssigneeSabaCategory Link5MinsGame(List<AssignmentModel> assignments, MatchInfo matchInfo)
    {
        var sameHourAssignments = assignments.Where(x =>
            x.IsSaba &&
            x.EventDateTime.Date == matchInfo.KOTime.Date &&
            x.EventDateTime.Hour == matchInfo.KOTime.Hour).ToList();

        if (!sameHourAssignments.Any())
        {
            return AssigneeSabaCategory.Busy;
        }

        var hasLink5mins = sameHourAssignments.Where(x => x.MatchType is MatchType.Saba5Mins);

        return hasLink5mins.Any() ? AssigneeSabaCategory.Doing5Mins1G : AssigneeSabaCategory.Busy;
    }

    public static (CategoryType, string) GetCategoryTypeAndExplanation(AssigneeSabaCategory category, MatchInfo matchInfo)
        => category switch
        {
            AssigneeSabaCategory.Free => (CategoryType.FullyAvailable, CategorizeHelper.FormatExplanation(ExplanationConstants.FREE, matchInfo)),
            AssigneeSabaCategory.Doing1G => (CategoryType.FullyAvailable, ExplanationConstants.CAPACITY_1G_DOING_4G),
            AssigneeSabaCategory.Doing2G => (CategoryType.FullyAvailable, ExplanationConstants.CAPACITY_2G_DOING_4G),
            AssigneeSabaCategory.Doing3G => (CategoryType.FullyAvailable, ExplanationConstants.CAPACITY_3G_DOING_4G),
            AssigneeSabaCategory.Doing5Mins1G => (CategoryType.FullyAvailable, ExplanationConstants.CAPACITY_1G_DOING_2G),
            _ => (CategoryType.Busy, null)
        };
}