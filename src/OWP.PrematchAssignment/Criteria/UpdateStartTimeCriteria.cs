using Fanex.Data.Repository;

namespace OWP.PrematchAssignment.Criteria;

public class UpdateStartTimeCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public TimeSpan? StartTime { get; set; }

    public DateTime Date { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_UpdateStartTime";

    public override bool IsValid() => StartTime.HasValue && ActorId > 0;
}