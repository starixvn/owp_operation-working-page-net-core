
namespace OWP.NewLeagueSetting.Models;

[Serializable]
public class LeagueSettingInfo : LeagueSettingCore
{
    public string LeagueGroup { get; set; } = default!;

    public string LeagueGroupNote { get; set; } = default!;

    public string League { get; set; } = default!;

    public string FullConditionDescription { get; set; } = default!;

    public string Comment { get; set; } = default!;

    public string LstNote { get; set; } = default!;

    public string Remark2 { get; set; } = default!;
}

public class ConditionInfo
{
    public bool HasTv { get; set; }

    public bool HasIpTv { get; set; }

    public List<Condition> IpTv { get; set; }

    public List<Condition> Round { get; set; }

    public List<Condition> Division { get; set; }

    public string Remark { get; set; }

}
