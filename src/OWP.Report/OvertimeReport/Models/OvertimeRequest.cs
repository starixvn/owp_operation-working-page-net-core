namespace OWP.Report.OvertimeReport.Models;

public class OverTimeRequest
{
    public string Location { get; set; }

    public string Trader { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }
}