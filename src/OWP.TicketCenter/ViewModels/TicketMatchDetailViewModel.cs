namespace OWP.TicketCenter.ViewModels;

public class TicketMatchDetailViewModel : TicketDetailBaseViewModel
{
    public int OwpLeagueId { get; set; }

    public string OwpLeagueName { get; set; }

    public int OwpHomeId { get; set; }

    public int OwpAwayId { get; set; }

    public string HomeName { get; set; }

    public string AwayName { get; set; }

    public DateTime? KOTime { get; set; }

    public string EventName => OwpLeagueName;

    public int EventType => (int)Enum.EventType.Match;
}