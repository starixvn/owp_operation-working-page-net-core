using Fanex.Data.Repository;

namespace OWP.AutoAssignment.Criteria.PublicApi;

public class GetUserInfoFromNAPUserControlCriteria : CriteriaBase
{
    public int SiteId { get; set; }

    public override string GetSettingKey() => "OWP_AutoAssignment_GetUserInfoFromNAPUserControl";

    public override bool IsValid() => SiteId > 0;
}