using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace OWP.OtherSport.Notification
{
    public class NotifyRequest : IRequest<bool>
    {
        public string SessionId { get; set; }

        public DateTime EventDate { get; set; }
    }

    class Handler
    {
    }
}
