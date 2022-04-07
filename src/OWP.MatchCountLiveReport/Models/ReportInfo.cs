using OfficeOpenXml.Attributes;

namespace OWP.MatchCountLiveReport.Models
{
  using System.Text.Json.Serialization;
  using Epplus.Mapper.Annotations;
  using OWP.MatchCountLiveReport.Helper;

  public class ReportInfo
  {
    [Cell("A3")]
    public int? No { get; set; }

    [Cell("B3")]
    public string BetType { get; set; }

    [EpplusIgnore]
    public bool IsSaba { get; set; }

    [EpplusIgnore]
    public int? Total { get; set; }

    [Cell("C3")]
    [JsonIgnore]
    public string ExcelTotalColumn => LiveReportHelper.BuildCellValue(Total);
    [EpplusIgnore]
    public int? Mj { get; set; }

    [Cell("D3")]
    [JsonIgnore]
    public string ExcelMjColumn => LiveReportHelper.BuildCellValue(Mj);

    [EpplusIgnore]
    public int? M1 { get; set; }

    [Cell("E3")]
    [JsonIgnore]
    public string ExcelM1Column => LiveReportHelper.BuildCellValue(M1);

    [EpplusIgnore]
    public int? M2 { get; set; }

    [Cell("F3")]
    [JsonIgnore]
    public string ExcelM2Column => LiveReportHelper.BuildCellValue(M2);

    [EpplusIgnore]
    public int? M3 { get; set; }

    [Cell("G3")]
    [JsonIgnore]
    public string ExcelM3Column => LiveReportHelper.BuildCellValue(M3);

    [EpplusIgnore]
    public int? M4 { get; set; }

    [Cell("H3")]
    [JsonIgnore]
    public string ExcelM4Column => LiveReportHelper.BuildCellValue(M4);

    [EpplusIgnore]
    public int? Others { get; set; }

    [Cell("I3")]
    [JsonIgnore]
    public string ExcelOthersColumn => LiveReportHelper.BuildCellValue(Others);

    [EpplusIgnore]
    public int? Others1 { get; set; }


    [Cell("J3")]
    [JsonIgnore]
    public string ExcelOthers1Column => LiveReportHelper.BuildCellValue(Others1);

    [EpplusIgnore]
    public int? Others2 { get; set; }

    [Cell("K3")]
    [JsonIgnore]
    public string ExcelOthers2Column => LiveReportHelper.BuildCellValue(Others2);

    [EpplusIgnore]
    public int? Others3 { get; set; }

    [Cell("L3")]
    [JsonIgnore]
    public string ExcelOthers3Column => LiveReportHelper.BuildCellValue(Others3);

    [EpplusIgnore]
    public int? Others4 { get; set; }

    [Cell("M3")]
    [JsonIgnore]
    public string ExcelOthers4Column => LiveReportHelper.BuildCellValue(Others4);

    [EpplusIgnore]
    public int? Others5 { get; set; }

    [Cell("N3")]
    [JsonIgnore]
    public string ExcelOthers5Column => LiveReportHelper.BuildCellValue(Others5);

    [EpplusIgnore]
    public int? Others6 { get; set; }

    [Cell("O3")]
    [JsonIgnore]
    public string ExcelOthers6Column => LiveReportHelper.BuildCellValue(Others6);

    [EpplusIgnore]
    public int? Unknown { get; set; }

    [Cell("P3")]
    [JsonIgnore]
    public string ExcelUnknownColumn => LiveReportHelper.BuildCellValue(Unknown);

  }
}