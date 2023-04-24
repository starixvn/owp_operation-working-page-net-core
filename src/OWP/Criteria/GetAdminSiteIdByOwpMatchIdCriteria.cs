using Fanex.Data.Repository;

namespace OWP.Criteria;
public class GetAdminSiteIdByOwpMatchIdCriteria : CriteriaBase
{
    public string ParentMatchIds { get; set; }

    public override string GetSettingKey() => "OWP_LiveGame_GetAdminSiteIdByOwpMatchId";

    public override bool IsValid() => true;
}
