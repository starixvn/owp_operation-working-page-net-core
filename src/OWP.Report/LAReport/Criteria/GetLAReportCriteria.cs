namespace OWP.Report.LAReport.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetLAReportCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey() => "OWP_LAReports";

        public override bool IsValid() => true;
    }
}