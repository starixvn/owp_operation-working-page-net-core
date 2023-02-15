namespace OWP.AutoAssignment.Models.InternalApi;

public class LeagueSettingInfo
{
    public int MatchId { get; set; }

    public string MatchOffer { get; set; }

    public string LSTNote { get; set; }

    public decimal? MaxBetHDP { get; set; }

    public decimal? MaxBetNextCorner { get; set; }

    public decimal? MaxbetNoOfCorner { get; set; }

    public decimal? SpreadHDP { get; set; }

    public decimal? SpreadOU { get; set; }

    public decimal? MaxBetOU { get; set; }

    public decimal? MaxBet1HHDP { get; set; }

    public decimal? MaxBet1HOU { get; set; }

    public decimal? Spread1HHDP { get; set; }

    public decimal? Spread1HOU { get; set; }

    public string NoHDP { get; set; }

    public decimal? MaxBet1X2 { get; set; }

    public decimal? Spread1X2 { get; set; }

    public decimal? MaxBet1H1X2 { get; set; }
}