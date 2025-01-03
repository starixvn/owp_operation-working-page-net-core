namespace OWP.EventPicking.Api.Models;
public class MainEventMaxbetSpread
{
    public int InfoSiteMatchId { get; set; }

    public decimal? SpreadHdp { get; set; }

    public decimal? SpreadOu { get; set; }

    public decimal? MaxbetHdp { get; set; }

    public bool IsReferenceLeagueManagement { get; set; }

    public string NoOfHDP { get; set; }

    [Cell("M2")]
    public string LstNote { get; set; }

    public string Remark2LeagueManagement { get; set; }
}