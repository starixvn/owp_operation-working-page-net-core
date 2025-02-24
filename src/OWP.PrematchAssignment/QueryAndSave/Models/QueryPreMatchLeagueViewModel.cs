using Newtonsoft.Json;

namespace OWP.PrematchAssignment.QueryAndSave.Models;

public class QueryPreMatchLeagueViewModel
{
    [JsonProperty("PreMatchLeagues")]
    public IEnumerable<LeagueAssignment> PreMatchLeagues { get; set; }

    [JsonProperty("InfoSiteMatches")]
    public IEnumerable<InfoSiteMatch> InfoSiteMatches { get; set; }
}