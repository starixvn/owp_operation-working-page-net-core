namespace OWP.Report.TraderReport.Criteria
{
    using System;

    public class GetPerformanceDetailsCriteria
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string TraderIds { get; set; }

        public string MajorBettypeIds { get; set; }
    }
}