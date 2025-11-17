namespace OWP.EventPicking.Api.Models;

public class LeagueTurnOver
{
    public string LeagueName { get; set; }

    public List<TurnOverInfo> TurnoverPeriod { get; set; }

    public List<MarginInfo> MarginPeriod { get; set; }


}

