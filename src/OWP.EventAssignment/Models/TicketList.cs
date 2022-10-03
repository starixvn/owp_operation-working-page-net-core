using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.EventAssignment.Models;

public class TicketListModel
{
    public int TicketId { get; set; }

    public string UserName { get; set; }

    public string NickName { get; set; }

    public DateTime CreatedDate { get; set; }

    public string Message { get; set; }

    public string SubtypeName { get; set; }
}
