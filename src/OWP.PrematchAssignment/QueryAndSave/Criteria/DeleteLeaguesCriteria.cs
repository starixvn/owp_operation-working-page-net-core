using Fanex.Data.Repository;

namespace OWP.PrematchAssignment.Criteria;

public class DeleteLeagueCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public int PreMatchLeagueId { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_DeleteLeague";

    public override bool IsValid() => PreMatchLeagueId > 0 && ActorId > 0;
}