namespace OWP.PreMatchAssignment.Criteria;

using Fanex.Data.Repository;

public class InsertNewNoteMessageCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public string Message { get; set; }

    public int PreMatchNoteId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string Username { get; set; }

    public int PreMatchNoteMessageId { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_InsertNewNoteMessage";

    public override bool IsValid() => ActorId > 0 && PreMatchNoteId > 0 && !string.IsNullOrWhiteSpace(Message);
}