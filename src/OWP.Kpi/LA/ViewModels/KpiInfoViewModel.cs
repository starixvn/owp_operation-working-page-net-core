using OWP.Kpi.LA.Constant;
using OWP.Kpi.LA.Helper;

namespace OWP.Kpi.LA.Models;

public class KpiInfoViewModel
{
    public int No { get; set; }

    public int UserId { get; set; }

    public string UserName { get; set; }

    public string Target { get; set; }

    public int? TargetId { get; set; }

    public TotalGameInfo AchievedGames { get; set; } = new();

    public TotalGameInfo GapWithTarget { get; set; } = new();

    public string Progress { get; set; }

    public string Reward { get; set; }
}
