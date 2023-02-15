namespace OWP.LiveDetail.Assignment;

using System.Globalization;

public abstract class CommandCriteria : Criteria.CommandCriteria
{
    public int ActorId { get; set; }

    public string SessionId { get; } = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

    public override bool IsValid() => ActorId > 0;
}