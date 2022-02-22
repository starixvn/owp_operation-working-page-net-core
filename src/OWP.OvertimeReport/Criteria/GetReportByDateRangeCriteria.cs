namespace OWP.OvertimeReport.Criteria
{
    using Fanex.Data.Repository;

    public class GetReportByDateRangeCriteria : CriteriaBase
    {
        public string TraderName { get; set; }

        public string Location { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int SiteId { get; set; }

        public override string GetSettingKey() => "OWP_OvertimeReport_GetReportByDateRange";

        public override bool IsValid() => true;
    }
}