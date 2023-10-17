namespace OWP.Kpi.LA.Models;

public class TotalGameInfo
{
    public double VirtualMatches { get; set; }

    public double MultipleMatches { get; set; }

    public double SpecialProducts { get; set; }

    public double FiveMajorLeagues { get; set; }

    public double MultipleProducts { get; set; }

    public bool IsWinMinTarget => new[]
    {
        VirtualMatches,
        MultipleMatches,
        SpecialProducts,
        FiveMajorLeagues,
        MultipleProducts
    }.All(value => value >= 0);
}