using OWP.Models;

namespace OWP.LiveDetail.MatchInfo;

using Common;

public class CommandResult : OWPCommandResult
{
    public string SessionId { get; set; }

    public ValidateResult ValidateResult { get; set; }
}