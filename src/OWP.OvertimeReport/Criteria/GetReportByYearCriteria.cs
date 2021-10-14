namespace OWP.OvertimeReport.Criteria
{
    using Fanex.Data.Repository;

    public class GetReportByYearCriteria : CriteriaBase
    {
        public string Trader { get; set; }

        public string Location { get; set; }

        public int Year { get; set; }

        public override string GetSettingKey() => "OWP_OvertimeReport_GetReportByYear";

        public override bool IsValid() => true;
    }
}
