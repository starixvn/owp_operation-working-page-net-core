using System.Collections.Generic;
using MediatR;

namespace OWP.TicketCenter.Models
{
    public class RecipientRequestBase: IRequest<bool>
    {
        public int TicketId { get; set; }

        public IList<Recipient> Recipients { get; set; }
    }
}
