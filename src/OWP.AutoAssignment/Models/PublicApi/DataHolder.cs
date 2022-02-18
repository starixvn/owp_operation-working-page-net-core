namespace OWP.AutoAssignment.Models.PublicApi
{
    public class DataHolder
    {
        public SchedulingMatch Matches { get; set; }

        public IEnumerable<SchedulingLAUser> Suspenders { get; set; }

        public IEnumerable<SchedulingTraderInfo> Traders { get; set; }
    }
}