namespace OWP.PreMatchAssignment.Models
{
    using System.Collections.Generic;

    public class QueryPreMatchLeagueViewModel
    {
        public IEnumerable<LeagueAssignment> PreMatchLeagues { get; set; }

        public IEnumerable<InfoSiteMatch> InfoSiteMatches { get; set; }
    }
}