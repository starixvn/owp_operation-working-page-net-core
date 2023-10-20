namespace OWP.Kpi.LA.Models;

public class KpiInfo
{
    public int UserId { get; set; }

    public string UserName { get; set; }

    public string SectionName { get; set; }

    public string Target { get; set; }

    public int? TargetId { get; set; }

    public double VirtualMatches { get; set; }

    public double MultipleMatches { get; set; }

    public double SpecialProducts { get; set; }

    public double FiveMajorLeagues { get; set; }

    public double MultipleProducts { get; set; }
}
