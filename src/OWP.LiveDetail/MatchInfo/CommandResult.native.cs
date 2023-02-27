using OWP.LiveDetail.Common;
using OWP.Models;

namespace OWP.LiveDetail.MatchInfo;

public class CommandResult : OWPCommandResult
{
    public string SessionId { get; set; }

    public ValidateResult ValidateResult { get; set; }
}