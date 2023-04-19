using Fanex.Data.Repository;

namespace OWP.Criteria;
public class GetAdminSiteInfoCriteria : CriteriaBase
{
    public string ParentMatchIds { get; set; }

    public override string GetSettingKey() => "OWP_LiveGame_GetAdminSiteInfo";

    public override bool IsValid() => true;
}
