namespace OWP.TicketCenter.Models;

public class EventTicketMatchOfLogWorkModel : EventTicketModelBase
{
    public string Subject { get; set; }

    public string LeagueName { get; set; }

    public string HomeName { get; set; }

    public string AwayName { get; set; }
        
    public DateTime? KoTime { get; set; }   
}