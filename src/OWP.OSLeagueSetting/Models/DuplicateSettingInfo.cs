namespace OWP.OSLeagueSetting.Models;

public class DuplicateSettingInfo
{
    public int LeagueSettingId { get; set; }

    public int? RoundId { get; set; }

    public int? OfferLineId { get; set; }

    public int? BetDeLayId { get; set; }

    public int? ProviderId { get; set; }

    public string Remark { get; set; }

    public TimeSpan? KOFromTime { get; set; }

    public TimeSpan? KOToTime { get; set; }

    public bool IsIptvOnly { get; set; }
}