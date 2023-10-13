using OWP.Kpi.LA.Models;

namespace OWP.Kpi.LA.Helper;
public static class ProgressHelper
{
    public static string Evaluate(List<ConditionInfo> conditionInfos, TotalGameInfo totalGame)
    {
        var progress = "Fail";
        var firstCondition = conditionInfos.FirstOrDefault();
        var matchingCondition = conditionInfos.Skip(1)
            .LastOrDefault(condition => IsGreaterThanOrEqualTo(totalGame, condition.ConditionFirst)
                                        || IsGreaterThanOrEqualTo(totalGame, condition.ConditionSecond));

        if (totalGame.MultipleMatches + totalGame.VirtualMatches
            > firstCondition?.ConditionFirst.TotalVirtualMultipleMatches)
        {
            progress = firstCondition.Progress;
        }

        if (matchingCondition is not null)
        {
            progress = matchingCondition.Progress;
        }

        return progress;
    }

    private static bool IsGreaterThanOrEqualTo(TotalGameInfo totalGame, TotalGameInfo condition)
    {
        var properties = typeof(TotalGameInfo).GetProperties();
        foreach (var property in properties)
        {
            var totalGameValue = (double)property.GetValue(totalGame);
            var conditionValue = (double)property.GetValue(condition);

            if (totalGameValue < conditionValue)
            {
                return false;
            }
        }
        return true;
    }
}
