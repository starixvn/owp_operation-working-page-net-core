using System;

namespace OWP.TicketCenter.ViewModels
{
    public class EventInfoViewModel
    {
        public string LeagueName { get; set; }

        public string EventName { get; set; }

        public DateTime? EventDateTimeGmt8 { get; set; }

        public bool IsLeague { get; set; }
    }
}
