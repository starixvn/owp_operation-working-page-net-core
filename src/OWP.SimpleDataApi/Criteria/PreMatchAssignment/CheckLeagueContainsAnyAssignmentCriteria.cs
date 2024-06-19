namespace OWP.SimpleDataApi.Criteria.PreMatchAssignment;

public class CheckLeagueContainsAnyAssignmentCriteria : CriteriaBase
{
    public int PreMatchLeagueId { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_CheckLeagueContainsAnyAssignment";

    public override bool IsValid() => PreMatchLeagueId > 0;
}