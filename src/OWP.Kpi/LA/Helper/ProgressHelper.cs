using OWP.Kpi.LA.Constant;
using OWP.Kpi.LA.Models;

namespace OWP.Kpi.LA.Helper;
public static class ProgressHelper
{
    private const string NA = "N/A";
    private const string Condition1 = "Condition 1";
    private const string Condition2 = "Condition 2";
    private const string Condition3 = "Condition 3";


    public static string Evaluate(List<ConditionInfo> conditionInfos, TotalGameInfo totalGame)
    {
        var progress = "Fail";
        var firstCondition = conditionInfos.FirstOrDefault();
        var matchingCondition = conditionInfos.Skip(1)
            .LastOrDefault(condition => IsGreaterThanOrEqualTo(totalGame, condition.ConditionFirst)
                                        || IsGreaterThanOrEqualTo(totalGame, condition.ConditionSecond)
                                        || IsGreaterThanOrEqualTo(totalGame, condition.ConditionThird));

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

    public static string CalculateApplyCondition(ConditionInfo conditionInfos, TotalGameInfo totalGame)
    {
        if (conditionInfos is null)
        {
            return NA;
        }

        var result = new List<string>();

        if (conditionInfos.LevelId == ConditionConstant.ConditionInfos[0].LevelId)
        {
            AddIfConditionMet(
                result,
                totalGame.MultipleMatches + totalGame.VirtualMatches >= conditionInfos.ConditionFirst.TotalVirtualMultipleMatches,
                Condition1,
                Condition2
            );

            AddIfConditionMet(
                result,
                totalGame.MultipleMatches >= conditionInfos.ConditionThird.MultipleMatches,
                Condition3
            );
        }
        else
        {
            AddIfConditionMet(result, IsGreaterThanOrEqualTo(totalGame, conditionInfos.ConditionFirst), Condition1);
            AddIfConditionMet(result, IsGreaterThanOrEqualTo(totalGame, conditionInfos.ConditionSecond), Condition2);
            AddIfConditionMet(result, IsGreaterThanOrEqualTo(totalGame, conditionInfos.ConditionThird), Condition3);
        }

        return result.Any() ? string.Join(", ", result) : NA;
    }

    public static bool IsGreaterThanOrEqualTo(TotalGameInfo totalGame, TotalGameInfo condition)
    {
        var properties = typeof(TotalGameInfo).GetProperties();
        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(bool))
            {
                continue;
            }

            var totalGameValue = (double)property.GetValue(totalGame);
            var conditionValue = (double)property.GetValue(condition);

            if (totalGameValue < conditionValue)
            {
                return false;
            }
        }
        return true;
    }

    private static void AddIfConditionMet(List<string> result, bool condition, params string[] conditionsToAdd)
    {
        if (condition)
        {
            result.AddRange(conditionsToAdd);
        }
    }
}
