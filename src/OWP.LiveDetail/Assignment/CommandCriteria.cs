using System.Globalization;

namespace OWP.LiveDetail.Assignment;

public abstract class CommandCriteria : Criteria.CommandCriteria
{
    public int ActorId { get; set; }

    public string SessionId { get; } = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

    public override bool IsValid() => ActorId > 0;
}