
using Epplus.Mapper.Annotations;

namespace OWP.Kpi.LA.ViewModels;
public class KpiInfoReport
{
    [Cell("A3")]
    public int No { get; set; }

    [Cell("B3")]
    public string UserName { get; set; }

    [Cell("C3")]
    public string Progress { get; set; }

    [Cell("D3")]
    public string Target { get; set; }

    [Cell("E3")]
    public int GapVirtualMatches { get; set; }

    [Cell("F3")]
    public int GapMultipleMatches { get; set; }

    [Cell("G3")]
    public int GapSpecialProducts { get; set; }

    [Cell("H3")]
    public int GapFiveMajorLeagues { get; set; }

    [Cell("I3")]
    public int GapMultipleProducts { get; set; }

    [Cell("J3")]
    public int AchievedVirtualMatches { get; set; }
   
    [Cell("K3")]
    public int AchievedMultipleMatches { get; set; }

    [Cell("L3")]
    public int AchievedSpecialProducts { get; set; }

    [Cell("M3")]
    public int AchievedFiveMajorLeagues { get; set; }

    [Cell("N3")]
    public int AchievedMultipleProducts { get; set; }

    [Cell("O3")]
    public string Reward { get; set; }

}
