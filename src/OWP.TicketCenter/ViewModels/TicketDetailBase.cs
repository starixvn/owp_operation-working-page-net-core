using System.Collections.Generic;

namespace OWP.TicketCenter.ViewModels
{
    public class TicketDetailBase
    {
        public int TicketId { get; set; }

        public int TicketTypeId { get; set; }

        public string TicketTypeName { get; set; }

        public IList<MessageDetail> Messages { get; set; }
    }
}
