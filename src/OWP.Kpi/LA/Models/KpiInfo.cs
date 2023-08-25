namespace OWP.Kpi.LA.Models;

public class KpiInfo
{
    public int UserId { get; set; }

    public string UserName { get; set; }

    public string Progress { get; set; }

    public string Target { get; set; }

    public TotalGameInfo GapWithTarget { get; set; }

    public TotalGameInfo AchievedGames { get; set; }
}
