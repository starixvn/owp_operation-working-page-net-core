namespace OWP.LiveDetail.Assignment;

using System;
using System.Globalization;

public abstract class CommandCriteria : OWP.Criteria.CommandCriteria
{
    public int ActorId { get; set; }

    public string SessionId { get; private set; } = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

    public override bool IsValid() => ActorId > 0;
}