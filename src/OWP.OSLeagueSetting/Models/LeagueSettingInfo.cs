namespace OWP.OSLeagueSetting.Models;

public class LeagueSettingInfo : LeagueSettingBase
{
    public int? CategoryId { get; set; }

    public string CategoryName { get; set; }

    public string OSLeagueName { get; set; }

    public bool IsActive { get; set; }

    public double? MaxBetHDP { get; set; }

    public double? MaxBetOU { get; set; }

    public double? SpreadHDP { get; set; }

    public double? SpreadOU { get; set; }

    public string RoundName { get; set; }

    public string BetType { get; set; }

    public string OfferLines { get; set; }

    public string Provider { get; set; }

    public string BetDelay { get; set; }

    public string WeekMatch { get; set; }

    public string Region { get; set; }

    public bool IsManualUpdateScore { get; set; }
}