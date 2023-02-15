namespace OWP.OSLeagueSetting.Models;

public class DuplicateSettingModel
{
    public int LeagueSettingId { get; set; }

    public string OsLeagueName { get; set; }

    public List<int?> RoundIds { get; set; }

    public List<int?> OfferLinesIds { get; set; }

    public List<int?> BetDeLayIds { get; set; }

    public List<int?> ProviderIds { get; set; }

    public string Remark { get; set; }

    public TimeSpan? KOFromTime { get; set; }

    public TimeSpan? KOToTime { get; set; }

    public bool IsIptvOnly { get; set; }
}