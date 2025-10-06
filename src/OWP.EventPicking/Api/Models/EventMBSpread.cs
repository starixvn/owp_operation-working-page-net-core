namespace OWP.EventPicking.Api.Models;
public class EventMBSpread
{
    public int InfoSiteMatchId { get; set; }

    public decimal? SpreadHdp { get; set; }

    public decimal? SpreadOu { get; set; }

    public decimal? MaxbetHdp { get; set; }

    public bool IsReferenceLeagueManagement { get; set; }

}