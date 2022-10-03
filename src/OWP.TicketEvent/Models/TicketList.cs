using System;

namespace OWP.TicketEvent.Models;

public class TicketListModel
{
    public int TicketId { get; set; }

    public string UserName { get; set; }

    public string NickName { get; set; }

    public DateTime CreatedDate { get; set; }

    public string Message { get; set; }

    public string SubtypeName { get; set; }
}
