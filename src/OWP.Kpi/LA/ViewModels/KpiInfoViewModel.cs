using OWP.Kpi.LA.Constant;
using OWP.Kpi.LA.Helper;

namespace OWP.Kpi.LA.Models;

public class KpiInfoViewModel
{
    public int UserId { get; set; }

    public string UserName { get; set; }

    public string Target { get; set; }

    public int? TargetId { get; set; }

    public TotalGameInfo AchievedGames { get; set; } = new();

    public TotalGameInfo GapWithTarget => CalculateGapHelper.CalculateGap(TargetId, AchievedGames);

    public string Progress => ProgressHelper.Evaluate(ConditionConstant.ConditionInfos.ToList(), AchievedGames);

    public string Reward => ConditionConstant.ConditionInfos.FirstOrDefault(x => x.Progress == Progress)?.Reward;
}
