using System.Collections.Generic;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.ViewModels
{
    public class EventTicketViewModel
    {
        public IList<TicketLeagueDetailViewModel> EventTicketLeagues
        {
            get;
            set;
        }

        public IList<TicketMatchDetailViewModel> EventTicketMatch
        {
            get;
            set;
        }

        public IList<TicketOtherDetailViewModel> EventTicketOthers
        {
            get;
            set;
        }
    }
}
