namespace OWP.TicketSetting.TicketEvent.Models;

public class TicketListModel
{
    public int TicketId { get; set; }

    public string LatestUsername { get; set; }

    public DateTime LatestCreatedDate { get; set; }

    public string LatestMessage { get; set; }

    public string SubtypeName { get; set; }

    public bool IsRead { get; set; }
}
