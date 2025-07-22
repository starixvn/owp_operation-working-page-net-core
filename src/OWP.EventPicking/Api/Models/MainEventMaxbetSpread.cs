namespace OWP.EventPicking.Api.Models;
public class MainEventMaxbetSpread
{
    public int InfoSiteMatchId { get; set; }

    
    public decimal? SpreadHdp { get; set; }

    [Cell("O2")]
    public string SpreadHdpExcel => SpreadHdp.HasValue ? $"{SpreadHdp.Value*100}" : string.Empty;

    public decimal? SpreadOu { get; set; }

    [Cell("P2")]
    public string SpreadOuExcel => SpreadOu.HasValue ? $"{SpreadOu.Value * 100}" : string.Empty;

    public decimal? MaxbetHdp { get; set; }

    [Cell("N2")]
    public string MaxbetHdpExcel => MaxbetHdp.HasValue ? $"{MaxbetHdp.Value/100}K" : string.Empty;

    public bool IsReferenceLeagueManagement { get; set; }

    [Cell("Q2")]
    public string NoOfHDP { get; set; }

    [Cell("M2")]
    public string LstNote { get; set; }

    public string Remark2LeagueManagement { get; set; }
}