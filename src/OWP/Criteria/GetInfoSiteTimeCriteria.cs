using Fanex.Data.Repository;

namespace OWP.Criteria;

public class GetInfoSiteTimeCriteria : CriteriaBase
{
    public string ParentMatchIds { get; set; }

    public override string GetSettingKey()
    {
        return "OWP_LiveGame_GetInfoSiteTime";
    }

    public override bool IsValid()
    {
        return true;
    }
}