using System.Collections.Generic;

namespace OWP.TicketCenter.Models
{
    public class RecipientModel
    {
        public IList<int> RecipientIds { get; set; } = new List<int>();

        public int RecipientType { get; set; }
    }
}
