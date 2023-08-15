namespace OWP.OSLeagueSetting.Models;

public class SettingDetail
{
    public int? LeagueSettingId { get; set; }

    public string LeagueName { get; set; }

    public bool IsActive { get; set; }

    public bool IsActiveMaxBet { get; set; }

    public int? Region { get; set; }

    public int? Category { get; set; }

    public int? WeekMatch { get; set; }

    public string BetDelay { get; set; }

    public DateTime? KOFromTime { get; set; }

    public DateTime? KOToTime { get; set; }

    public string Remark { get; set; }

    public bool IsManualUpdateScore { get; set; }

    public bool IsIptvOnly { get; set; }

    public decimal? SpreadHDP { get; set; }


    public decimal? SpreadOU { get; set; }

    public decimal? MaxbetHDP { get; set; }

    public decimal? MaxbetOU { get; set; }

    public string Rounds { get; set; }

    public string OfferLines { get; set; }

    public string Providers { get; set; }

    public string BetTypes { get; set; }
}