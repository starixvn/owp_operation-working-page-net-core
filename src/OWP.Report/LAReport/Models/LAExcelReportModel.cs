using Epplus.Mapper.Annotations;

namespace OWP.Report.LAReport.Models;

public class LAExcelReportModel
{
    [Cell("A7")]
    public string Username { get; set; }

    [Cell("B7")]
    public string Section { get; set; }

    [Cell("C7")]
    public double TotalMatches { get; set; }

    [Cell("D7")]
    public double Link2 { get; set; }

    [Cell("E7")]
    public double Link3 { get; set; }

    [Cell("F7")]
    public double Link4 { get; set; }

    [Cell("G7")]
    public double Link5 { get; set; }

    [Cell("H7")]
    public double Link6 { get; set; }

    [Cell("I7")]
    public double Link7 { get; set; }

    [Cell("J7")]
    public double Link8Plus { get; set; }

    [Cell("K7")]
    public double TotalMultiMatches { get; set; }

    [Cell("L7")]
    public double OnlyProvider { get; set; }

    [Cell("M7")]
    public double OnlyTv { get; set; }

    [Cell("N7")]
    public double OnlyReporter { get; set; }

    [Cell("O7")]
    public double MixedProviderTVReporter { get; set; }

    [Cell("P7")]
    public double TotalSingleMatches { get; set; }

    [Cell("Q7")]
    public double CornerPlus { get; set; }

    [Cell("R7")]
    public double TBookingPlus { get; set; }

    [Cell("S7")]
    public double Fantasy { get; set; }

    [Cell("T7")]
    public double GoalKick { get; set; }

    [Cell("U7")]
    public double OtherProducts { get; set; }

    [Cell("V7")]
    public double TotalSpecialProducts { get; set; }

    [Cell("W7")]
    public double FTCorner { get; set; }

    [Cell("X7")]
    public double FTCorner5ML { get; set; }

    [Cell("Y7")]
    public double TotalMixedProduct { get; set; }

    [Cell("Z7")]
    public double FiveML { get; set; }

    [Cell("AA7")]
    public double ESoccer5Mins { get; set; }

    [Cell("AB7")]
    public double ESoccer6Mins { get; set; }

    [Cell("AC7")]
    public double ESoccer15Mins { get; set; }

    [Cell("AD7")]
    public double ESoccer16Mins { get; set; }

    [Cell("AE7")]
    public double ESoccer20Mins { get; set; }

    [Cell("AF7")]
    public double TotalESoccer { get; set; }

    [Cell("AG7")]
    public double Others { get; set; }
}