namespace OWP.NewLeagueSetting.Models;
public class LeagueSettingInfoSender : LeagueSettingInfo
{
    public int LeagueSettingId { get; set; } = default!;

    public int OWPLeagueId { get; set; } = default!;

    public int LeagueGroupId { get; set; } = default!;

    public string MaxbetWD_HDP_Show => FormatMaxbet(MaxbetWD_HDP_value, MaxbetWD_HDP_Note);

    public string MaxbetWK_HDP_Show => FormatMaxbet(MaxbetWK_HDP_value, MaxbetWK_HDP_Note);

    public string MaxbetWD_NoOfHDP_Show => FormatNoOfHdpMaxbet(MaxbetWD_NoOfHDP_value, MaxbetWD_NoOfHDP_Note);

    public string MaxbetWK_NoOfHDP_Show => FormatNoOfHdpMaxbet(MaxbetWK_NoOfHDP_value, MaxbetWK_NoOfHDP_Note);

    public LeagueSettingCondition Condition => JsonConvert.DeserializeObject<LeagueSettingCondition>(FullConditionDescription);

    public string FormatMaxbet(decimal? maxbet, string maxbetNote) => maxbet.HasValue ? $"{maxbet / 1000}".TrimEnd('0').TrimEnd('.') + "K" : maxbetNote;

    public string FormatNoOfHdpMaxbet(string maxbetNoOfHdp, string maxbetNote)
        => !string.IsNullOrWhiteSpace(maxbetNoOfHdp) ? maxbetNoOfHdp : maxbetNote;
}

