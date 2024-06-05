namespace OWP.NewLeagueSetting.Models;

public class LeagueSettingCondition
{
    public bool HasTv { get; set; }

    public bool HasIpTv { get; set; }

    public IList<Condition> IpTv { get; set; }

    public IList<Condition> Round { get; set; }

    public IList<Condition> Division { get; set; }

    public string Remark { get; set; }
}