namespace OWP.TicketCenter.Models;

public class EventTicketMatchOfLogWorkModel : EventTicketModelBase
{
    public string LeagueName { get; set; }

    public string HomeName { get; set; }

    public string AwayName { get; set; }

    public DateTime? KOTime { get; set; }
}