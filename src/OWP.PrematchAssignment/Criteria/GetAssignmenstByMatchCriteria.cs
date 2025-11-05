using Fanex.Data.Repository;

namespace OWP.PrematchAssignment.Criteria;

public class GetMatchDiscussionInfoCriteria : CriteriaBase
{
    public int AdminSiteId { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchDiscussion_GetInfo";

    public override bool IsValid() => AdminSiteId > 0;
}