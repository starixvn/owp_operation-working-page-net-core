using Fanex.Data.Repository;

namespace OWP.TeamManagement.Criteria;

public class GetTeamInfoCriteria : CriteriaBase
{
    public string TeamName { get; set; }
    public string LeagueName { get; set; }
    public int LeagueGroupId { get; set; }

    public override string GetSettingKey() => "OWP_TeamManagement_GetInfoTeam";

    public override bool IsValid()
        => true;
}