namespace OWP.TicketCenter.ViewModels;

public class TicketLiveIncidentDetailViewModel : TicketDetailBaseViewModel
{
    public int OwpLeagueId { get; set; }

    public string OwpLeagueName { get; set; }

    public int OwpHomeId { get; set; }

    public int OwpAwayId { get; set; }

    public string HomeName { get; set; }

    public string AwayName { get; set; }

    public DateTime? KoTime { get; set; }

    public string EventName => OwpLeagueName;

    public int EventType => (int)Enum.EventType.LiveIncident;
}