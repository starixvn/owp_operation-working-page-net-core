namespace OWP.OSLeagueSetting.Models;

public class AddOrEditSettingCommon
{
    public int RegionId { get; set; }

    public int CategoryId { get; set; }

    public string RoundId { get; set; }

    public int WeekMatchId { get; set; }

    public string OfferLineId { get; set; }

    public int BetDelayId { get; set; }

    public string ProviderId { get; set; }

    public TimeSpan FromTime { get; set; }

    public TimeSpan ToTime { get; set; }

    public Decimal? SpreadHDP { get; set; }

    public Decimal? SpreadOU { get; set; }

    public Decimal? MaxbetHDP { get; set; }

    public Decimal? MaxbetOU { get; set; }

    public string Remark { get; set; }

    public bool UpdateScore { get; set; }

    public bool IpTv { get; set; }

    public string BettypeId { get; set; }
}
