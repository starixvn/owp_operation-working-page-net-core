namespace OWP.NewLeagueSetting.Models;
public class LeagueSettingInfoSender : LeagueSettingInfo
{
    public int LeagueSettingId { get; set; } = default!;

    public int OWPLeagueId { get; set; } = default!;

    public int LeagueGroupId { get; set; } = default!;

    public string MaxbetWD_HDP_Show => FormatMaxbet(MaxbetWD_HDP_Value, MaxbetWD_HDP_Note);

    public string MaxbetWK_HDP_Show => FormatMaxbet(MaxbetWK_HDP_Value, MaxbetWK_HDP_Note);

    public string WD_NoOfHDP_Show => FormatNoOfHdpMaxbet(WD_NoOfHDP_Value, WD_NoOfHDP_Note);

    public string WK_NoOfHDP_Show => FormatNoOfHdpMaxbet(WK_NoOfHDP_Value, WK_NoOfHDP_Note);

    public LeagueSettingCondition Condition => JsonConvert.DeserializeObject<LeagueSettingCondition>(FullConditionDescription);

    public static string FormatMaxbet(decimal? maxbet, string maxbetNote) => maxbet.HasValue ? $"{maxbet / 1000}".TrimEnd('0').TrimEnd('.') + "K" : maxbetNote;

    public static string FormatNoOfHdpMaxbet(string maxbetNoOfHdp, string maxbetNote)
        => !string.IsNullOrWhiteSpace(maxbetNoOfHdp) ? maxbetNoOfHdp : maxbetNote;
}

