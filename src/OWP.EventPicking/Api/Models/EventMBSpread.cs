namespace OWP.EventPicking.Api.Models;
public class EventMBSpread
{
    public int InfoSiteMatchId { get; set; }

    public decimal? SpreadHdp { get; set; }

    public decimal? SpreadOu { get; set; }

    public decimal? MaxbetHdp { get; set; }

    public decimal? MaxbetOU { get; set; }

    public string? NoOfHDP { get; set; }

    public bool IsReferenceLeagueManagement { get; set; }

    public bool IsChildMatch { get; set; }

    public int MinorBettypeId { get; set; }
}