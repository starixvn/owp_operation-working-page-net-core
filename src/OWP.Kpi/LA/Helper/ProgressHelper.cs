using System.Reflection;
using OWP.Kpi.LA.Models;

namespace OWP.Kpi.LA.Helper;
public static class ProgressHelper
{
    public static string Evaluate(List<ConditionInfo> conditionInfos, TotalGameInfo totalGame)
    {
        var progress = string.Empty;
        var firstCondition = conditionInfos.FirstOrDefault();
        var matchingCondition = conditionInfos.Skip(1)
            .LastOrDefault(condition => IsGreaterThanOrEqualTo(totalGame, condition.ConditionsFirst) || IsGreaterThanOrEqualTo(totalGame, condition.ConditionSecond));

        if (totalGame.MultipleMatches + totalGame.VirtualMatches > firstCondition.ConditionsFirst.TotalVirtualMultipleMatches)
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
        PropertyInfo[] properties = typeof(TotalGameInfo).GetProperties();
        foreach (var property in properties)
        {
            int totalGameValue = (int)property.GetValue(totalGame);
            int conditionValue = (int)property.GetValue(condition);

            if (totalGameValue < conditionValue)
            {
                return false;
            }
        }
        return true;
    }
}
