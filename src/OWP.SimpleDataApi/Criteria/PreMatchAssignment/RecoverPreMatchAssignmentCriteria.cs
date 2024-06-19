namespace OWP.SimpleDataApi.Criteria.PreMatchAssignment;

public class RecoverPreMatchAssignmentCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public int PreMatchLeagueId { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_Recover";

    public override bool IsValid() => PreMatchLeagueId > 0;
}