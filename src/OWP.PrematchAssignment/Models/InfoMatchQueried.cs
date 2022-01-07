using System;

namespace OWP.PreMatchAssignment.Models
{
    public class InfoMatchQueried
    {
        public int InfoSiteMatchId { get; set; }
        
        public int OwpLeagueId { get; set; }

        public int OwpHomeId { get; set; }

        public int OwpAwayId { get; set; }

        public DateTime EventDateTime { get; set; }

    }
}