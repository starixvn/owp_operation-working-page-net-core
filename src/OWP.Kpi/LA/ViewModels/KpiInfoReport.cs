
using Epplus.Mapper.Annotations;

namespace OWP.Kpi.LA.ViewModels;
public class KpiInfoReport
{
    [Cell("A3")]
    public int No { get; set; }

    [Cell("B3")]
    public string UserName { get; set; }

    [Cell("C3")]
    public string SectionName { get; set; }

    [Cell("D3")]
    public string Progress { get; set; }

    [Cell("E3")]
    public string Target { get; set; }

    [Cell("F3")]
    public double GapVirtualMatches { get; set; }

    [Cell("G3")]
    public double GapMultipleMatches { get; set; }

    [Cell("H3")]
    public double GapSpecialProducts { get; set; }

    [Cell("I3")]
    public double GapFiveMajorLeagues { get; set; }

    [Cell("J3")]
    public double GapMultipleProducts { get; set; }

    [Cell("K3")]
    public double AchievedVirtualMatches { get; set; }

    [Cell("L3")]
    public double AchievedMultipleMatches { get; set; }

    [Cell("M3")]
    public double AchievedSpecialProducts { get; set; }

    [Cell("N3")]
    public double AchievedFiveMajorLeagues { get; set; }

    [Cell("O3")]
    public double AchievedMultipleProducts { get; set; }

    [Cell("P3")]
    public string Reward { get; set; }

}
