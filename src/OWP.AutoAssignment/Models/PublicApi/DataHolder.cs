using System.Collections.Generic;

namespace OWP.AutoAssignment.Models.PublicApi
{
    public class DataHolder
    {
        public IEnumerable<SchedulingMatch> Matches { get; set; }

        public IEnumerable<SchedulingLAUser> Suspenders { get; set; }

        public IEnumerable<TraderInfo> Traders { get; set; }
    }
}