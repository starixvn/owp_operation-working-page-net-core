using System.Runtime.CompilerServices;

namespace OWP.NewLeagueSetting.Models;

[Serializable]
public class LeagueSettingInfo
{
    public string LeagueGroup { get; set; } = default!;

    public string LeagueGroupNote { get; set; } = default!;

    public string League { get; set; } = default!;

    public string FullConditionDescription { get; set; } = default!;

    public decimal? Spread_HDP_value { get; set; } = default!;

    public decimal? Spread_OU_value { get; set; } = default!;

    public string Spread_Note { get; set; } = default!;

    public decimal? MaxbetWD_HDP_value { get; set; } = default!;

    public string MaxbetWD_HDP_Note { get; set; } = default!;

    public string MaxbetWD_NoOfHDP_value { get; set; } = default!;

    public string MaxbetWD_NoOfHDP_Note { get; set; } = default!;

    public decimal? MaxbetWK_HDP_value { get; set; } = default!;

    public string MaxbetWK_HDP_Note { get; set; } = default!;

    public string MaxbetWK_NoOfHDP_value { get; set; } = default!;

    public string MaxbetWK_NoOfHDP_Note { get; set; } = default!;

    public string Comment { get; set; } = default!;

    public string LstNote { get; set; } = default!;

    public string Remark2 { get; set; } = default!;

}

public class LeagueSettingInfoSender :LeagueSettingInfo
{
    public int LeagueSettingId { get; set; } = default!;

    public int OWPLeagueId { get; set; } = default!;

    public int LeagueGroupId { get; set; } = default!;

    public ConditionInfo Condition => JsonConvert.DeserializeObject<ConditionInfo>(this.FullConditionDescription);

}

public class ConditionInfo
{
    public bool HasTv { get; set; }

    public bool HasIpTv { get; set; }

    public List<Condition> IpTv { get; set; }

    public List<Condition> Round { get; set;  }

    public List<Condition> Division { get; set;}

    public string Remark { get; set; }

}
