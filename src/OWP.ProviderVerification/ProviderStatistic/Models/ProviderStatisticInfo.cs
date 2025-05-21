using Epplus.Mapper.Annotations;

namespace OWP.ProviderVerification.ProviderStatistic.Models;
public class ProviderStatisticInfo
{
    [Cell("A4")]
    public string LeagueOrDay { get; set; }

    [Cell("B4")]
    public int? Rb2 { get; set; }

    [Cell("C4")]
    public int? Bg { get; set; }

    [Cell("D4")]
    public int? Betradar { get; set; }

    [Cell("E4")]
    public int? Img { get; set; }

    [Cell("F4")]
    public int? Rts { get; set; }

    [Cell("G4")]
    public int? Fc { get; set; }

    [Cell("H4")]
    public int? Slr { get; set; }

    [Cell("I4")]
    public int? Woo { get; set; }

    [Cell("J4")]
    public int? Streaming { get; set; }

    [Cell("K4")]
    public int? _365Gv { get; set; }

    [Cell("L4")]
    public int? TotalMatches { get; set; }

    [Cell("M4")]
    public int? CancelMatches { get; set; }

    [Cell("N4")]
    public int? Total { get; set; }
}