namespace OWP.NewLeagueSetting.Models;

public class LeagueParamRequest 
{
    public string LeagueName { get; set; }

    public bool IsEqualLeague { get; set; }

    public int? LeagueGroupId { get; set; }

    public List<int> Blacklist { get; set; }

    public List<int> OfferLive { get; set; }
}
