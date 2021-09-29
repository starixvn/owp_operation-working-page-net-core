using System.Collections.Generic;

namespace OWP.TicketCenter.ViewModels
{
    public class TicketDetailBase
    {
        public int TicketId { get; set; }

        public int TicketType { get; set; }

        public string TypeName { get; set; }

        public IList<MessageDetail> Messages { get; set; }
    }
}
