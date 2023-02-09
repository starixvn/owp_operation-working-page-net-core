namespace OWP.LiveDetail.MatchOdd;

using System;
using System.Globalization;

public abstract class CommandCriteria : OWP.Criteria.CommandCriteria
{
    public int UserId { get; set; }

    public string SessionId { get; private set; } = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

    public override bool IsValid() => UserId > 0;
}