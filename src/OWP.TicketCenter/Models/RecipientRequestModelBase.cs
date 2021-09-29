using System.Collections.Generic;
using MediatR;

namespace OWP.TicketCenter.Models
{
    public class RecipientRequestModelBase: IRequest<bool>
    {
        public int TicketId { get; set; }

        public IList<AddNewRecipientModel> Recipients { get; set; }
    }
}
