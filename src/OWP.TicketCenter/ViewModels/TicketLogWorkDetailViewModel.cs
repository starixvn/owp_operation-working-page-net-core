namespace OWP.TicketCenter.ViewModels;

public class TicketLogWorkDetailViewModel : TicketDetailBaseViewModel
{
    public string Subject { get; set; }

    public int AdminSiteId { get; set; }

    public string LeagueName { get; set; }

    public string HomeName { get; set; }

    public string AwayName { get; set; }

    public DateTime? KoTime { get; set; }

    public string EventName => LeagueName;

    public int EventType => (int)Enum.EventTypeEnum.LogWork;
}