namespace OWP.PreMatchAssignment.Models
{
    using Newtonsoft.Json;

    public class QueryPreMatchLeagueViewModel
    {
        [JsonProperty("PreMatchLeagues")]
        public IEnumerable<LeagueAssignment> PreMatchLeagues { get; set; }

        [JsonProperty("InfoSiteMatches")]
        public IEnumerable<InfoSiteMatch> InfoSiteMatches { get; set; }
    }
}