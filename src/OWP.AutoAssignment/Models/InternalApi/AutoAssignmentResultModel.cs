namespace OWP.AutoAssignment.Models.InternalApi
{
    public class AutoAssignmentResultModel
    {
        public string RequestId { get; set; }

        public int InfoSiteMatchId { get; set; }

        public int Bettype { get; set; }

        public bool IsSaba { get; set; }

        public bool IsFutsal { get; set; }

        public string PcCode { get; set; }

        public int? UserId_FTHDP { get; set; }

        public int? UserId_1HHDP { get; set; }

        public int? UserId_FTOU { get; set; }

        public int? UserId_1HOU { get; set; }

        public int UserId_LA { get; set; }

        public string Group { get; set; }

        public string ExtraTime { get; set; }

        public int StatusCode { get; set; }
    }
}