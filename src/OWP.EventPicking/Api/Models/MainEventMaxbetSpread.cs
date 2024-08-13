namespace OWP.EventPicking.Api.Models;
public class MainEventMaxbetSpread
{
    public int InfoSiteMatchId { get; set; }

    public decimal? SpreadHdp { get; set; }

    public decimal? SpreadOu { get; set; }

    public decimal? MaxbetHdp { get; set; }

    public bool IsReferenceLeagueManagement { get; set; }

    public string NoOfHDP { get; set; }

    [Cell("J2")]
    public string LstNote { get; set; }
}