using System.Collections.Generic;

namespace OWP.TicketCenter.Models
{
    public class NewTicketModelBase
    {
        // FE do not care this property
        public int TicketId { get; set; }

        public int TicketType { get; set; }

        public string Message { get; set; }

        public int CreatorId { get; set; }

        public IList<RecipientModel> Recipients { get; set; }
    }
}
