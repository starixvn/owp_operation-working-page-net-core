using Fanex.Data.Repository;
namespace OWP.PreMatchReport.Criteria;


public class GetReportForMatchesCriteria : CriteriaBase
{
    public string KeyWord { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public override bool IsValid() => true;
    public override string GetSettingKey() => "OWP_PreMatchReport_GetReportForMatchByDateRange";
}
