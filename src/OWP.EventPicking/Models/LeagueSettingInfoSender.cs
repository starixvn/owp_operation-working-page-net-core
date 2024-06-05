using Newtonsoft.Json;

namespace OWP.EventPicking.Models;
public class LeagueSettingInfo
{
    public decimal? Spread_HDP_Value { get; set; } = default!;

    public decimal? Spread_OU_Value { get; set; } = default!;

    public string Spread_Note { get; set; } = default!;

    public decimal? MaxbetWD_HDP_Value { get; set; } = default!;

    public string MaxbetWD_HDP_Note { get; set; } = default!;

    public string WD_NoOfHDP_Value { get; set; } = default!;

    public string WD_NoOfHDP_Note { get; set; } = default!;

    public decimal? MaxbetWK_HDP_Value { get; set; } = default!;

    public string MaxbetWK_HDP_Note { get; set; } = default!;

    public string WK_NoOfHDP_Value { get; set; } = default!;

    public string WK_NoOfHDP_Note { get; set; } = default!;

    public string LeagueGroup { get; set; } = default!;

    public string LeagueGroupNote { get; set; } = default!;

    public string League { get; set; } = default!;

    public string FullConditionDescription { get; set; } = default!;

    public string Comment { get; set; } = default!;

    public string LstNote { get; set; } = default!;

    public string Remark2 { get; set; } = default!;

}

public class LeagueSettingInfoSender : LeagueSettingInfo
{
    public int LeagueSettingId { get; set; } = default!;

    public int OWPLeagueId { get; set; } = default!;

    public int LeagueGroupId { get; set; } = default!;

    public string MaxbetWD_HDP_Show => FormatMaxbet(MaxbetWD_HDP_Value, MaxbetWD_HDP_Note);

    public string MaxbetWK_HDP_Show => FormatMaxbet(MaxbetWK_HDP_Value, MaxbetWK_HDP_Note);

    public string MaxbetWD_NoOfHDP_Show => FormatNoOfHdpMaxbet(WD_NoOfHDP_Value, WD_NoOfHDP_Note);

    public string MaxbetWK_NoOfHDP_Show => FormatNoOfHdpMaxbet(WK_NoOfHDP_Value, WK_NoOfHDP_Note);

    public NewLeagueSettingCondition Condition => JsonConvert.DeserializeObject<NewLeagueSettingCondition>(FullConditionDescription);

    public static string FormatMaxbet(decimal? maxbet, string maxbetNote) => maxbet.HasValue ? $"{maxbet / 1000}".TrimEnd('0').TrimEnd('.') + "K" : maxbetNote;

    public static string FormatNoOfHdpMaxbet(string maxbetNoOfHdp, string maxbetNote)
        => !string.IsNullOrWhiteSpace(maxbetNoOfHdp) ? maxbetNoOfHdp : maxbetNote;
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

public class NewLeagueSettingCondition
{
    public bool HasTv { get; set; }

    public bool HasIpTv { get; set; }

    public IList<Condition> IpTv { get; set; }

    public IList<Condition> Round { get; set; }

    public IList<Condition> Division { get; set; }

    public string Remark { get; set; }
}
