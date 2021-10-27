using System.Collections.Generic;

namespace OWP.TicketCenter.Models
{
    public class RecipientRequestMode
    {
        public int TicketId { get; set; }

        public IList<InsertOrRemoveManyRecipientModel> Recipients { get; set; }
    }
}
