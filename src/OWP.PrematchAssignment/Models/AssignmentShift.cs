using OWP.PreMatchAssignment.Models;

namespace OWP.PrematchAssignment.Models;

public class AssignmentShift
{
    public IEnumerable<PreMatchLeague> Leagues { get; set; }

    public IEnumerable<InfoSiteMatch> Matches { get; set; }
}

public class DefaultDayAndNightShift
{
    public AssignmentShift DayShift { get; set; }

    public AssignmentShift NightShift { get; set; }
}