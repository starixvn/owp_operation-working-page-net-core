using System.Text.Json.Serialization;
using Epplus.Mapper.Annotations;
using OfficeOpenXml.Attributes;
using OWP.MatchCountLiveReport.Helper;

namespace OWP.MatchCountLiveReport.Models;

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
    public int? MJT1 { get; set; }

    [Cell("O3")]
    [JsonIgnore]
    public string ExcelMJT1Column => LiveReportHelper.BuildCellValue(MJT1);

    [EpplusIgnore]
    public int? MJT2 { get; set; }

    [Cell("P3")]
    [JsonIgnore]
    public string ExcelMJT2Column => LiveReportHelper.BuildCellValue(MJT2);

    [EpplusIgnore]
    public int? CLMJQ { get; set; }

    [Cell("Q3")]
    [JsonIgnore]
    public string ExcelCLMJQColumn => LiveReportHelper.BuildCellValue(CLMJQ);

    [EpplusIgnore]
    public int? CLMNQ { get; set; }

    [Cell("R3")]
    [JsonIgnore]
    public string ExcelCLMNQColumn => LiveReportHelper.BuildCellValue(CLMNQ);

    [EpplusIgnore]
    public int? INTMJ { get; set; }

    [Cell("S3")]
    [JsonIgnore]
    public string ExcelINTMJColumn => LiveReportHelper.BuildCellValue(INTMJ);

    [EpplusIgnore]
    public int? INTMN { get; set; }

    [Cell("T3")]
    [JsonIgnore]
    public string ExcelINTMNColumn => LiveReportHelper.BuildCellValue(INTMN);

    [EpplusIgnore]
    public int? WorldCup { get; set; }

    [Cell("U3")]
    [JsonIgnore]
    public string ExcelWorldCupColumn => LiveReportHelper.BuildCellValue(WorldCup);

    [EpplusIgnore]
    public int? Euro { get; set; }

    [Cell("V3")]
    [JsonIgnore]
    public string ExcelEuroColumn => LiveReportHelper.BuildCellValue(Euro);

    [EpplusIgnore]
    public int? Unknown { get; set; }

    [Cell("W3")]
    [JsonIgnore]
    public string ExcelUnknownColumn => LiveReportHelper.BuildCellValue(Unknown);
}