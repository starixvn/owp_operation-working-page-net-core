using OfficeOpenXml.Attributes;

namespace OWP.MatchCountLiveReport.Models
{
  using Epplus.Mapper.Annotations;

  public class ReportInfo
  {
    [Cell("A3")]
    public int? No { get; set; }

    [Cell("B3")]
    public string Bettype { get; set; }

    [EpplusIgnore]
    public bool IsSaba { get; set; }

    [Cell("C3")]
    public int? Total { get; set; }

    [Cell("D3")]
    public int? Mj { get; set; }

    [Cell("E3")]
    public int? M1 { get; set; }

    [Cell("F3")]
    public int? M2 { get; set; }

    [Cell("G3")]
    public int? M3 { get; set; }

    [Cell("H3")]
    public int? M4 { get; set; }

    [Cell("I3")]
    public int? Others { get; set; }

    [Cell("J3")]
    public int? Others1 { get; set; }

    [Cell("K3")]
    public int? Others2 { get; set; }

    [Cell("L3")]
    public int? Others3 { get; set; }

    [Cell("M3")]
    public int? Others4 { get; set; }

    [Cell("N3")]
    public int? Others5 { get; set; }

    [Cell("O3")]
    public int? Others6 { get; set; }

    [Cell("P3")]
    public int? Unknown { get; set; }

  }
}