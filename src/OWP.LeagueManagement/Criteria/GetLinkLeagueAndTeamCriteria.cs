namespace OWP.LeagueManagement.Criteria;

using Fanex.Data.Repository;

public class GetLinkLeagueAndTeamCriteria : CriteriaBase
{
    public string LeagueIdString { get; set; }

    public override string GetSettingKey() => "OWP_LeagueManagement_GetLinkLeagueAndTeam";

    public override bool IsValid() => true;
}