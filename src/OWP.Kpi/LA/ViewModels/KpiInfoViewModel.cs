using OWP.Kpi.LA.Constant;
using OWP.Kpi.LA.Helper;

namespace OWP.Kpi.LA.Models;

public class KpiInfoViewModel
{
    public int UserId { get; set; }

    public string UserName { get; set; }

    public string Target { get; set; }

    public TotalGameInfo GapWithTarget { get; set; } = new();

    public TotalGameInfo AchievedGames { get; set; } = new();

    public string Progress => ProgressHelper.Evaluate(ConditionConstant.ConditionInfos.ToList(), AchievedGames);

    public string Reward => ConditionConstant.ConditionInfos.FirstOrDefault(x => x.Progress == Progress)?.Reward;
}
