namespace OWP.TicketCenter.ViewModels;

public class TicketLogWorkDetailViewModel : TicketDetailBaseViewModel
{
    public string LeagueName { get; set; }

    public string HomeName { get; set; }

    public string AwayName { get; set; }

    public DateTime? KOTime { get; set; }

    public string EventName => LeagueName;

    public int EventType => (int)Enum.EventType.LogWork;
}