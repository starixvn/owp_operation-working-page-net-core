namespace OWP.Kpi.LA.Models;

public class Condition : TotalGameInfo
{
    public int TotalVirtualMultipleMatches { get; set; }
}

public class ConditionInfo
{
    public string Progress { get; set; }

    public string Level { get; set; }

    public int LevelId { get; set; }

    public Condition ConditionFirst { get; set; }

    public Condition ConditionSecond { get; set; }

    public Condition TargetCondition { get; set; }

    public string TimeRange { get; set; }

    public string Reward { get; set; }
}

