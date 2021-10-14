namespace OWP.OvertimeReport.Criteria
{
    using Fanex.Data.Repository;

    public class GetReportByMonthCriteria : CriteriaBase
    {
        public string Trader { get; set; }

        public string Location { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public override string GetSettingKey() => "OWP_OvertimeReport_GetReportByMonth";

        public override bool IsValid() => true;
    }
}
