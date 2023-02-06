namespace OWP.PreMatchAssignment.Models;

using Newtonsoft.Json;
using OWP.PrematchAssignment.Models;

public class QueryNightShiftPreMatchLeagueViewModel
{
    [JsonProperty("PreMatchLeagues")]
    public IEnumerable<NightShiftLeagueAssignment> PreMatchLeagues { get; set; }

    [JsonProperty("InfoSiteMatches")]
    public IEnumerable<InfoSiteMatch> InfoSiteMatches { get; set; }
}