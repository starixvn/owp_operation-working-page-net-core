using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria;
public class AddMessageIconCriteria: NonQueryCommand
{
    public int MessageId { get; set; }

    public string IconKey { get; set; }

    public int CreatorId  { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_AddMessageIcon";

    public override bool IsValid()
        => MessageId > 0 && CreatorId > 0;
}
