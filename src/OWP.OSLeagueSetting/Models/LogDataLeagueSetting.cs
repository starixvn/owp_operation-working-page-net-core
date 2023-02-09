namespace OWP.OSLeagueSetting.Models;

using Extensions;

public class LogDataLeagueSetting
{
    private string maxbetHDP;

    private string maxbetOU;

    private string spreadHDP;

    private string spreadOU;

    public string LeagueName { get; set; }

    public string IsActive { get; set; }

    public string Region { get; set; }

    public string Category { get; set; }

    public string Round { get; set; }

    public string WeekMatch { get; set; }

    public string OfferLines { get; set; }

    public string BetDelay { get; set; }

    public string Provider { get; set; }

    public string KOFromTime { get; set; }

    public string KOToTime { get; set; }

    public string Remark { get; set; }

    public string IsManualUpdateScore { get; set; }

    public string IsIpTvOnly { get; set; }

    public string BetType { get; set; }

    public string SpreadHDP
    {
        get => spreadHDP;
        set => spreadHDP = value?.TrimEnd('0')
            .TrimEnd('.');
    }

    public string SpreadOU
    {
        get => spreadOU;
        set => spreadOU = value?.TrimEnd('0')
            .TrimEnd('.');
    }

    public string MaxBetHDP
    {
        get => maxbetHDP;
        set => maxbetHDP = value?.TrimEnd('0')
            .TrimEnd('.')
            .ReplaceWith(@"\B(?=(\d{3})+(?!\d))", ",");
    }

    public string MaxBetOU
    {
        get => maxbetOU;
        set => maxbetOU = value?.TrimEnd('0')
            .TrimEnd('.')
            .ReplaceWith(@"\B(?=(\d{3})+(?!\d))", ",");
    }
}