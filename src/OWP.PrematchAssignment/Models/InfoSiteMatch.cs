using OWP.PrematchAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OWP.PreMatchAssignment.Models
{
    public class InfoSiteMatch
    {
        public int InfoSiteMatchId { get; set; }

        public string LeagueName { get; set; }

        public string HomeName { get; set; }

        public string AwayName { get; set; }

        public DateTime EventDateTime { get; set; }

        public int PreMatchLeagueId { get; set; }

        public bool HasNotes { get; set; }

        public int? PreMatchNoteId { get; set; }

        public bool IsLive { get; set; }

        public string MatchStatus { get; set; } = string.Empty;

        public List<TicketMatchBasicInfo> TicketList { get; set; }

        public bool HasMessages => TicketList.Count > 0;

        public bool IsRead => TicketList.All(x => x.IsRead);
    }
}