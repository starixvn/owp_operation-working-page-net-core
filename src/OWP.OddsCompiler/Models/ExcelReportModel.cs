using Epplus.Mapper.Annotations;

namespace OWP.OddsCompiler.Models;
public class ExcelReportModel
{
    [Cell("A3")]
    public string MatchStatus { get; set; }

    [Cell("B3")]
    public string MatchNo { get; set; }

    [Cell("C3")]
    public string KickOffTime { get; set; }

    [Cell("D3")]
    public string OtherTime { get; set; }

    [Cell("E3")]
    public string League { get; set; }

    [Cell("F3")]
    public string Home { get; set; }

    [Cell("G3")]
    public string Away { get; set; }

    [Cell("H3")]
    public string MaxBetHDP { get; set; }

    [Cell("I3")]
    public string SpreadHDP { get; set; }

    [Cell("J3")]
    public string SpreadOU { get; set; }

    [Cell("K3")]
    public string NoOfHDP { get; set; }

    [Cell("L3")]
    public string Fav { get; set; }

    [Cell("M3")]
    public decimal? HDP { get; set; }

    [Cell("N3")]
    public decimal? HomeOdds { get; set; }

    [Cell("O3")]
    public decimal? AwayOdds { get; set; }

    [Cell("P3")]
    public decimal? OU { get; set; }

    [Cell("Q3")]
    public decimal? Over { get; set; }

    [Cell("R3")]
    public decimal? Under { get; set; }

    [Cell("S3")]
    public decimal? One { get; set; }

    [Cell("T3")]
    public decimal? X { get; set; }

    [Cell("U3")]
    public decimal? Two { get; set; }

    [Cell("V3")]
    public string Market { get; set; }

    [Cell("W3")]
    public string Remark { get; set; }

    [Cell("X3")]
    public string WatchBet { get; set; }

    [Cell("Y3")]
    public string TVCode { get; set; }

    [Cell("Z3")]
    public string Round { get; set; }

    [Cell("AA3")]
    public string Division { get; set; }

    [Cell("AB3")]
    public string Channel { get; set; }

    [Cell("AC3")]
    public string Provider { get; set; }

    [Cell("AD3")]
    public string OtherProvider { get; set; }

    [Cell("AE3")]
    public string MatchOffer { get; set; }

    [Cell("AF3")]
    public int? InfoSiteID { get; set; }

    [Cell("AG3")]
    public string Website1 { get; set; }

    [Cell("AH3")]
    public string Website2 { get; set; }

    [Cell("AI3")]
    public string Website3 { get; set; }

    [Cell("AJ3")]
    public string Website4 { get; set; }
}