using System.Runtime.CompilerServices;
using OWP.Kpi.LA.Constant;
using OWP.Kpi.LA.Helper;

namespace OWP.Kpi.LA.Models;

public class KpiInfo
{
    public int UserId { get; set; }

    public string UserName { get; set; }

    public string Target { get; set; }

    public TotalGameInfo GapWithTarget { get; set; }

    public TotalGameInfo AchievedGames { get; set; }

    public string Progress => ProgressHelper.Evaluate(ConditionConstant.ConditionInfos, AchievedGames);
}
