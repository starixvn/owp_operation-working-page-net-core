namespace OWP.LeagueManagement.Criteria;

using Fanex.Data.Repository;

public class GetAllLeagueInfoCriteria : CriteriaBase
{
    public string League { get; set; }

    public int LeagueGroupId { get; set; }

    public string Category { get; set; }

    public bool IsActive { get; set; }

    public override string GetSettingKey() => "OWP_LeagueManagement_Get";

    public override bool IsValid() => true;
}