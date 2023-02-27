using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;

public class SaveMessageCriteria : NonQueryCommand
{
    public int MessageId { get; set; }

    public bool IsSaved { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_UpdateIsSavedMessage";

    public override bool IsValid() => MessageId > 0;
}