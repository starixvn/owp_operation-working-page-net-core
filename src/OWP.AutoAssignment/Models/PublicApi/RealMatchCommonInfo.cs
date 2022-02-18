namespace OWP.AutoAssignment.Models.PublicApi
{
    public class RealMatchCommonInfo
    {
        public int InfoSiteId { get; set; }

        public string LeagueName { get; set; }

        public string Tv { get; set; }

        public DateTime KOTime { get; set; }

        public string Provider { get; set; }

        public bool IsSoloMatch { get; set; }

        public bool IsMajorLeague { get; set; }

        public string Remark { get; set; }

        public string OtherProvider { get; set; }

        public string Location { get; set; }

        public string Channel { get; set; }
    }
}