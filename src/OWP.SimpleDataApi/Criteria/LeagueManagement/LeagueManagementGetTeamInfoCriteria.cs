namespace OWP.SimpleDataApi.Criteria.LeagueManagement;

using Fanex.Data.Repository;

public class LeagueManagementGetTeamInfoCriteria : CriteriaBase
{
    public int OwpLeagueId { get; set; }

    public override string GetSettingKey() => "OWP_LeagueManagement_GetTeamInfo";

    public override bool IsValid() => OwpLeagueId > 0;
}