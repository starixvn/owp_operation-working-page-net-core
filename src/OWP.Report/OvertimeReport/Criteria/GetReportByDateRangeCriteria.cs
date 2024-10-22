using Fanex.Data.Repository;

namespace OWP.Report.OvertimeReport.Criteria;

public class GetReportByDateRangeCriteria : CriteriaBase
{
    public string TraderName { get; set; }

    public string Location { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public string AllUserJson { get; internal set; }

    public override string GetSettingKey() => "OWP_OvertimeReport_GetReportByDateRange";

    public override bool IsValid() => true;
}