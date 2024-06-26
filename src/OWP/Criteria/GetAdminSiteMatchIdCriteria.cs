using Fanex.Data.Repository;

namespace OWP.Criteria;

public class GetAdminSiteMatchIdCriteria : CriteriaBase
{
    public int OWPMatchId { get; set; }

    public override string GetSettingKey()
    {
        return "OWP_TaskList_GetAdminSiteMatchId";
    }

    public override bool IsValid()
    {
        return true;
    }
}