using System.Reflection.Metadata.Ecma335;
using OWP.Kpi.LA.Constant;
using OWP.Kpi.LA.Models;

namespace OWP.Kpi.LA.Helper;
public static class CalculateGapHelper
{
    public static bool IsGap(DateTime fromDate, DateTime toDate)
        => toDate.Month == 11 && toDate.Year == DateTime.Now.Year &&
           fromDate.Month == 12 && fromDate.Year == DateTime.Now.Year - 2;

    public static TotalGameInfo CalculateGap(int? targetId, TotalGameInfo achievedGame)
    {
        var targetCondition = ConditionConstant.ConditionInfos
            .FirstOrDefault(x => x.LevelId == targetId)?.TargetCondition ?? new Condition();
        return new()
        {
            MultipleMatches = CalculateValue(achievedGame.MultipleMatches, targetCondition.MultipleMatches),
            VirtualMatches = CalculateValue(achievedGame.VirtualMatches, targetCondition.VirtualMatches),
            MultipleProducts = CalculateValue(achievedGame.MultipleProducts, targetCondition.MultipleProducts),
            FiveMajorLeagues = CalculateValue(achievedGame.FiveMajorLeagues, targetCondition.FiveMajorLeagues),
            SpecialProducts = CalculateValue(achievedGame.SpecialProducts, targetCondition.SpecialProducts)
        };
    }

    public static double CalculateValue(double achieved, double target)
        => target != 0 ? achieved - target : 0;
}
