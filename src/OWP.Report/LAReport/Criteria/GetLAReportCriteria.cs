using Fanex.Data.Repository;

namespace OWP.Report.LAReport.Criteria;

public class GetLAReportCriteria : CriteriaBase
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public bool ActiveUser { get; set; }

    public bool ResignUser { get; set; }

    public string AllUserJson { get; set; }

    public override string GetSettingKey() => "OWP_LAReports";

    public override bool IsValid() => true;
}