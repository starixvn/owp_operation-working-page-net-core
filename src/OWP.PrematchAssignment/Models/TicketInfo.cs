using System;

namespace OWP.PrematchAssignment.Models
{
    public class TicketLeagueBasicInfo
    {
        public int OWPLeagueId { get; set; }

        public int TicketId { get; set; }

        public string LatestUsername { get; set; }

        public DateTime LatestCreatedDate { get; set; }

        public string LatestMessage { get; set; }

        public bool IsRead { get; set; }

        public int EventType => (int)Enum.EventType.League;
    }

    public class TicketMatchBasicInfo
    {
        public int InfoSiteMatchId { get; set; }

        public int TicketId { get; set; }

        public string LatestUsername { get; set; }

        public DateTime LatestCreatedDate { get; set; }

        public string LatestMessage { get; set; }

        public bool IsRead { get; set; }

        public int EventType => (int)Enum.EventType.Match;
    }
}
