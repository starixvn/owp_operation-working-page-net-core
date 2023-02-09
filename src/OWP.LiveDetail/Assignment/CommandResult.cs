using OWP.Model;

namespace OWP.LiveDetail.Assignment;

public class CommandResult : OWPCommandResult
{
    public string SessionId { get; set; }

    public int AssignmentId { get; set; }
}