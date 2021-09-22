using System;

namespace OWP.TicketCenter.Models
{
    public class EventInfo
    {
        public string LeagueName { get; set; }

        public string EventName { get; set; }

        public DateTime EventDateTimeGmt8 { get; set; }

        public bool IsLeague { get; set; }

        public int? UserId { get; set; }

        public string Username { get; set; }
    }
}
