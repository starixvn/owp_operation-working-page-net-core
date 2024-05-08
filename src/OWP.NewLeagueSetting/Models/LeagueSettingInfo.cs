
namespace OWP.NewLeagueSetting.Models;

[Serializable]
public class LeagueSettingInfo : LeagueSettingCore
{
    public string LeagueGroup { get; set; } = default!;

    public string LeagueGroupNote { get; set; } = default!;

    public string League { get; set; } = default!;

    public string FullConditionDescription { get; set; } = default!;

    public string WD_NoOfHDP_value { get; set; } = default!;
    public string WD_NoOfHDP_Note { get; set; } = default!;
    public string WK_NoOfHDP_value { get; set; } = default!;
    public string WK_NoOfHDP_Note { get; set; } = default!;
    public string Comment { get; set; } = default!;

    public string LstNote { get; set; } = default!;

    public string Remark2 { get; set; } = default!;

}

public class LeagueSettingInfoSender : LeagueSettingInfo

    public string MaxbetWD_NoOfHDP_Show => FormatNoOfHdpMaxbet(WD_NoOfHDP_value, WD_NoOfHDP_Note);
    public string MaxbetWK_NoOfHDP_Show => FormatNoOfHdpMaxbet(WK_NoOfHDP_value, WK_NoOfHDP_Note);
    public string FormatMaxbet(decimal? maxbet, string maxbetNote) => maxbet.HasValue ? $"{maxbet / 1000}".TrimEnd('0').TrimEnd('.') + "K" : maxbetNote;
        => !string.IsNullOrWhiteSpace(maxbetNoOfHdp) ? maxbetNoOfHdp : maxbetNote;
    public List<Condition> Round { get; set; }
    public List<Condition> Division { get; set; }
