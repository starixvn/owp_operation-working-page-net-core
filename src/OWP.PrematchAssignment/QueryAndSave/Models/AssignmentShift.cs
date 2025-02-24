namespace OWP.PrematchAssignment.QueryAndSave.Models;

public class AssignmentShift
{
    public Dictionary<int, PreMatchLeague> Leagues { get; set; }

    public Dictionary<int, List<InfoSiteMatch>> Matches { get; set; }
}

public class NightShiftInfo
{
    public AssignmentShift DayShifts { get; set; }

    public AssignmentShift NightShifts { get; set; }

    public TimeSpan? DBStartTime { get; set; }
}