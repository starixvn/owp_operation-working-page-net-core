namespace OWP.LiveDetail.MatchInfo;

using Common;
using Model;

public class CommandResult : OWPCommandResult
{
    public string SessionId { get; set; }

    public ValidateResult ValidateResult { get; set; }
}