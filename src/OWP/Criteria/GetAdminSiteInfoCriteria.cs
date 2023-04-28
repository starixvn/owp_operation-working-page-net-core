using Fanex.Data.Repository;

namespace OWP.Criteria;
public class GetAdminSiteInfoCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_LiveGame_GetAdminSiteInfo";

    public override bool IsValid() => true;
}
