namespace OWP.EventPicking.Api.Models;
public class MainEventMaxbetSpread
{
    public int InfoSiteMatchId { get; set; }

    
    public decimal? SpreadHdp { get; set; }

    [Cell("O2")]
    public string SpreadHdpExcel => SpreadHdp.ToOWPSpreadFormatPercent();

    public decimal? SpreadOu { get; set; }

    [Cell("P2")]
    public string SpreadOuExcel => SpreadOu.ToOWPSpreadFormatPercent();

    public decimal? MaxbetHdp { get; set; }


    [Cell("N2")]
    public string MaxbetHdpExcel => MaxbetHdp.ToOWPMaxBetFormat();

    public decimal? MaxbetOu { get; set; }
    [Cell("BH2")]
    public string MaxbetOuExcel => MaxbetOu.ToOWPMaxBetFormat();

    public bool IsReferenceLeagueManagement { get; set; }

    [Cell("Q2")]
    public string NoOfHDP { get; set; }

    [Cell("M2")]
    public string LstNote { get; set; }

    public string Remark2LeagueManagement { get; set; }

    public bool IsChildMatch { get; set; }
    public int MinorBettypeId { get; set; }
}   