namespace OWP.AssignmentUpload.SyncInfoSite.Criteria;

using Fanex.Data.Repository;

public class QueryInfositeMatchCriteria : CriteriaBase
{
    public DateTime EventDate { get; set; }

    public override string GetSettingKey() => "OWP_InfoSiteMatch_GetInfoSiteMatchByDate";

    public override bool IsValid() => true;
}