namespace OWP.TicketCenter.ViewModels;

public class TicketOtherDetailViewModel : TicketDetailBaseViewModel
{
    public string Subject { get; set; }

    public string EventName => Subject;

    public int EventType => (int)Enum.EventType.Other;
}