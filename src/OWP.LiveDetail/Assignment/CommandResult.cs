namespace OWP.LiveDetail.Assignment;

using Model;

public class CommandResult : OWPCommandResult
{
    public string SessionId { get; set; }

    public int AssignmentId { get; set; }
}