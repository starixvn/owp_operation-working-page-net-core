using System.Collections.Generic;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.ViewModels
{
    public class EventTicketViewModel
    {
        public IList<EventTicketLeague> EventTicketLeagues
        {
            get;
            set;
        }

        public IList<EventTicketMatch> EventTicketMatch
        {
            get;
            set;
        }

        public IList<EventTicketOther> EventTicketOthers
        {
            get;
            set;
        }
        ///
    }
}
