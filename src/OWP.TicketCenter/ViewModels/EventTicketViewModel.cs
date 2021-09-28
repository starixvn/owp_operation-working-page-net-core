using System.Collections.Generic;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.ViewModels
{
    public class EventTicketViewModel
    {
        public IList<EventTicketLeagueRaw> EventTicketLeagues
        {
            get;
            set;
        }

        public IList<EventTicketMatchRaw> EventTicketMatch
        {
            get;
            set;
        }

        public IList<EventTicketOtherRaw> EventTicketOthers
        {
            get;
            set;
        }
        ///
    }
}
