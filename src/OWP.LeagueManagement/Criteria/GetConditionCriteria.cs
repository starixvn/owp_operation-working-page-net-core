namespace OWP.LeagueManagement.Criteria;

using Fanex.Data.Repository;
public class GetConditionCriteria : CriteriaBase
{
    public int LeagueSettingId { get; set; }

    public override string GetSettingKey() => "OWP_LeagueManagement_GetCondition";

    public override bool IsValid() => LeagueSettingId > 0;
}