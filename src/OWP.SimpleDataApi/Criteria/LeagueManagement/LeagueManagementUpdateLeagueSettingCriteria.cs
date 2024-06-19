using System.Diagnostics.CodeAnalysis;

namespace OWP.SimpleDataApi.Criteria.LeagueManagement;

public class LeagueManagementUpdateLeagueSettingCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public int LeagueSettingId { get; set; }

    public string Category { get; set; }

    public string TimeDesc { get; set; }

    public string Remark { get; set; }

    public string LSTNote { get; set; }

    public decimal? Spread_HDP { get; set; }

    public decimal? Spread_OU { get; set; }

    public decimal? Spread_1HHDP { get; set; }

    public decimal? Spread_1HOU { get; set; }

    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public decimal? Speard_1X2 { get; set; }

    public decimal? MaxbetWD_HDP { get; set; }

    public decimal? MaxbetWD_OU { get; set; }

    public decimal? MaxbetWD_1H { get; set; }

    public string MaxbetWD_NoOfHDP { get; set; }

    public string MaxbetWD_MatchOffer { get; set; }

    public decimal? MaxbetWK_HDP { get; set; }

    public decimal? MaxbetWK_OU { get; set; }

    public decimal? MaxbetWK_1H { get; set; }

    public string MaxbetWK_NoOfHDP { get; set; }

    public string MaxbetWK_MatchOffer { get; set; }

    public decimal? MaxBet_1X2Spread { get; set; }

    public decimal? MaxBet_FT1X2 { get; set; }

    public decimal? MaxBet_1H1X2 { get; set; }

    public string MaxBet_1X2_MatchOffer { get; set; }

    public bool IsApplied { get; set; }

    public override string GetSettingKey() => "OWP_LeagueManagement_Update";

    public override bool IsValid() => LeagueSettingId > 0 && ActorId > 0;
}