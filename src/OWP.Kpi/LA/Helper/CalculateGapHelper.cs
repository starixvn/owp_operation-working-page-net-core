using OWP.Kpi.LA.Constant;
using OWP.Kpi.LA.Models;

namespace OWP.Kpi.LA.Helper;
public static class CalculateGapHelper
{
    public static bool IsGap(DateTime fromDate, DateTime toDate)
        => toDate.Month == 12&& toDate.Year == DateTimeOffset.Now.Year &&
           fromDate.Month == 1 && fromDate.Year == DateTimeOffset.Now.Year - 1;

    public static TotalGameInfo CalculateGap(int? targetId, TotalGameInfo achievedGame)
    {
        var targetCondition = ConditionConstant.ConditionInfos.ToList()
            .Find(x => x.LevelId == targetId)?.TargetCondition ?? new Condition();
        return new TotalGameInfo
        {
            MultipleMatches = CalculateValue(achievedGame.MultipleMatches, targetCondition.MultipleMatches),
            VirtualMatches = CalculateValue(achievedGame.VirtualMatches, targetCondition.VirtualMatches),
            MultipleProducts = CalculateValue(achievedGame.MultipleProducts, targetCondition.MultipleProducts),
            FiveMajorLeagues = CalculateValue(achievedGame.FiveMajorLeagues, targetCondition.FiveMajorLeagues),
            SpecialProducts = CalculateValue(achievedGame.SpecialProducts, targetCondition.SpecialProducts)
        };
    }

    public static double CalculateValue(double achieved, double target)
        => (target is < 0 or > 0) ? Math.Round(achieved - target, 1) : 0;
}
