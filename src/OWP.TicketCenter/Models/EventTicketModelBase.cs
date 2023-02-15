namespace OWP.TicketCenter.Models;

public class EventTicketModelBase
{
    public int TicketId { get; set; }

    public int TicketType { get; set; }

    public string TicketStatus { get; set; }

    public int? TicketSubTypeId { get; set; }

    public string TicketSubTypeName { get; set; }

    public string TypeName { get; set; }

    public bool IsRead { get; set; }

    public bool IsSaved { get; set; }

    public bool IsPinned { get; set; }
}