namespace OWP.AutoAssignment.Criteria.PublicApi;

using Fanex.Data.Repository;

public class GetUserInfoFromNAPUserControlCriteria : CriteriaBase
{
    public int SiteId { get; set; }

    public override string GetSettingKey() => "OWP_AutoAssignment_GetUserInfoFromNAPUserControl";

    public override bool IsValid() => SiteId > 0;
}