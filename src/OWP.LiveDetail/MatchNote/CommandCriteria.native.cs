using System.Globalization;

namespace OWP.LiveDetail.MatchNote;

public abstract class CommandCriteria : Criteria.CommandCriteria
{
    public int UserId { get; set; }

    public string SessionId { get; } = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

    public override bool IsValid() => UserId > 0;
}