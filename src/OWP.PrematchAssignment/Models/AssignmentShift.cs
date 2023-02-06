using OWP.PreMatchAssignment.Models;

namespace OWP.PrematchAssignment.Models
{
    public class AssignmentShift
    {
        public IEnumerable<PreMatchLeague> Leagues { get; set; }

        public IEnumerable<InfoSiteMatch> Matches { get; set; }
    }

    public class AssignmentNightShift
    {
        public IEnumerable<NightShiftPreMatchLeague> Leagues { get; set; }

        public IEnumerable<InfoSiteMatch> Matches { get; set; }
    }

    public class DefaultDayAndNightShift
    {
        public AssignmentShift DayShift { get; set; }

        public AssignmentNightShift NightShift { get; set; }
    }
}