using System.Collections.Generic;
using MediatR;

namespace OWP.TicketCenter.Models
{
    public class RecipientRequestModelBase: IRequest<bool>
    {
        public int TicketId { get; set; }

        public IList<RecipientModel> Recipients { get; set; }
    }
}
