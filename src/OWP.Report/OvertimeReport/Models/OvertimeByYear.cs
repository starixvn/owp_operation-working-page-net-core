namespace OWP.Report.OvertimeReport.Models;

public class OvertimeByYear
{
    public string Location { get; set; }

    public string Trader { get; set; }

    public decimal? Total { get; set; }

    public List<decimal?> OvertimeData { get; set; }
}