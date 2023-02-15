namespace OWP.Criteria;

using Fanex.Data.Repository;

public class GetMinorAdminSiteMatchCriteria : CriteriaBase
{
    public int MatchId { get; set; }

    public int SubType { get; set; }

    public override string GetSettingKey() => "OWP_TaskList_GetMinorMatchByMatchId";

    public override bool IsValid() => true;
}