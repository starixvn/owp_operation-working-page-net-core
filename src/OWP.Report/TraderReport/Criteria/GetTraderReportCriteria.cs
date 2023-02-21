using Fanex.Data.Repository;

namespace OWP.Report.TraderReport.Criteria;

public class GetTraderReportCriteria : CriteriaBase
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_TraderReports";
}