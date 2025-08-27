namespace OWP.NewLeagueSetting.Models;
public class LeagueSettingInfoSender : LeagueSettingInfo
{
    public int LeagueSettingId { get; set; } = default!;

    public int OWPLeagueId { get; set; } = default!;

    public int LeagueGroupId { get; set; } = default!;

    public bool IsBlackList { get; set; } = default!;

    public bool NoOfferLive { get; set; } = default!;

    public string MaxbetWD_HDP_Show => FormatMaxbet(MaxbetWD_HDP_Value);

    public string MaxbetWK_HDP_Show => FormatMaxbet(MaxbetWK_HDP_Value);

    public string WD_NoOfHDP_Show => FormatNoOfHdpMaxbet(WD_NoOfHDP_Value);

    public string WK_NoOfHDP_Show => FormatNoOfHdpMaxbet(WK_NoOfHDP_Value);

    public string SpreadShow => AnalyzeSpreadSourceFT(Spread_HDP_Value, Spread_OU_Value);

    public LeagueSettingCondition Condition => string.IsNullOrWhiteSpace(FullConditionDescription)?
        new LeagueSettingCondition():
        JsonConvert.DeserializeObject<LeagueSettingCondition>(FullConditionDescription);

    public static string FormatMaxbet(decimal? maxbet) => maxbet.HasValue ? $"{maxbet / 1000}" + "K" : string.Empty;

    public static string FormatNoOfHdpMaxbet(string maxbetNoOfHdp)
        => !string.IsNullOrWhiteSpace(maxbetNoOfHdp) ? maxbetNoOfHdp : string.Empty;

    private static string AnalyzeSpreadSourceFT(decimal? spreadHdp, decimal? spreadOu)
    {
        if (spreadHdp.HasValue && spreadOu.HasValue)
        {
            var spreadHdpString = (spreadHdp * 100).ToString().TrimEnd('0').TrimEnd('.');
            var spreadOuString = (spreadOu * 100).ToString().TrimEnd('0').TrimEnd('.');

            return $"{spreadHdpString}%-{spreadOuString}%";
        }

        return null;
    }
}

