using Fanex.Data.Repository;

namespace OWP.Report.PreMatchReport.Criteria;

public class GetReportForLeagueCriteria : CriteriaBase
{
    public string KeyWord { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "OWP_PreMatchReport_GetReportForLeagueByDateRange";
}