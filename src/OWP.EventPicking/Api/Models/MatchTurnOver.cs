namespace OWP.EventPicking.Api.Models;

public class MatchTurnOver 
{
    public int InfoSiteMatchId { get; set; }

    public string LeagueName { get; set; }

    public IEnumerable<TurnOverInfo> TurnoverPeriod { get; set; }

    public IEnumerable<MarginInfo> MarginPeriod { get; set; }


}

