
namespace OWP.PreMatchAssignment.Models
{
    using System;

    public class PreMatchIssueInfo
    {
        public int PreMatchIssueId { get; set; }

        public int InfoSiteMatchId { get; set; }

        public int LeagueId { get; set; }

        public string LeagueName { get; set; }

        public string HomeName { get; set; }

        public string AwayName { get; set; }

        public DateTime? EventDateTime { get; set; }

        public string Subject { get; set; }
    }
}