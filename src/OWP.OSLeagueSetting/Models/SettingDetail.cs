namespace OWP.OSLeagueSetting.Models;

public class SettingDetail
{
    public int LeagueSettingId { get; set; }

    public string LeagueName { get; set; }

    public bool IsActive { get; set; }

    public string Region { get; set; }

    public string Category { get; set; }

    public string WeekMatch { get; set; }

    public string BetDelay { get; set; }

    public TimeSpan KOFromTime { get; set; }

    public TimeSpan KOToTime { get; set; }

    public string Remark { get; set; }

    public bool IsManualUpdateScore { get; set; }

    public bool IsIptvOnly { get; set; }

    public double SpreadHDP { get; set; }

    public double SpreadOU { get; set; }

    public double MaxbetHDP { get; set; }

    public double MaxbetOU { get; set; }

    public string Rounds { get; set; }

    public string OfferLines { get; set; }

    public string Providers { get; set; }

    public string BetTypes { get; set; }
}
