namespace OWP.MatchCountLiveReport.Criteria;

using Fanex.Data.Repository;

public class QueryReportInfoCriteria : CriteriaBase
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public override string GetSettingKey() => "OWP_MatchLiveReport_GetByTimeRange";

    public override bool IsValid()
        => true;
}