namespace OWP.Report.LAReport.Models;

public class LAReportModel
{
    public double UserId { get; set; }

    public string Username { get; set; }

    public string Nickname { get; set; }

    public string Section { get; set; }

    public double TotalMatches { get; set; }

    public double Link2 { get; set; }

    public double Link3 { get; set; }

    public double Link4 { get; set; }

    public double Link5 { get; set; }

    public double Link6 { get; set; }

    public double Link7 { get; set; }

    public double Link8Plus { get; set; }

    public double TotalMultiMatches { get; set; }

    public double OnlyProvider { get; set; }

    public double OnlyTv { get; set; }

    public double OnlyReporter { get; set; }

    public double MixedProviderTVReporter { get; set; }

    public double TotalSingleMatches => OnlyProvider + OnlyTv + OnlyReporter + MixedProviderTVReporter;

    public double CornerPlus { get; set; }

    public double TBookingPlus { get; set; }

    public double Fantasy { get; set; }

    public double GoalKick { get; set; }

    public double OtherProducts { get; set; }

    public double TotalSpecialProducts => CornerPlus + TBookingPlus + Fantasy + OtherProducts + GoalKick;

    public double FTCorner { get; set; }

    public double FTCorner5ML { get; set; }

    public double TotalMixedProduct => FTCorner + FTCorner5ML;

    public double FiveML { get; set; }

    public double ESoccer5Mins { get; set; }

    public double ESoccer6Mins { get; set; }

    public double ESoccer15Mins { get; set; }

    public double ESoccer16Mins { get; set; }

    public double ESoccer20Mins { get; set; }

    public double TotalESoccer => ESoccer5Mins + ESoccer6Mins + ESoccer15Mins + ESoccer16Mins + ESoccer20Mins;

    public double Others { get; set; }

    public double SumTotal
        => TotalMultiMatches +
           TotalSingleMatches +
           TotalSpecialProducts +
           TotalMixedProduct +
          TotalESoccer +
          FiveML +
           Others;
}