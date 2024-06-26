using Fanex.Data.Repository;

namespace OWP.SimpleDataApi.Criteria.PreMatchAssignment;

public class DeleteNoteMessageCriteria : NonQueryCommand
{
    public int PreMatchNoteMessageId { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_DeleteNoteMessage";

    public override bool IsValid() => PreMatchNoteMessageId > 0;
}