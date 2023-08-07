using Fanex.Data.Repository;
using OWP.TicketCenter.Const;

namespace OWP.TicketCenter.Criteria;

public class AddNewTicketIssueSupportCriteria : NonQueryCommand
{
    public int TicketId { get; set; }

    public string Subject { get; set; }

    public int EventType => (int)EventTypeEnum.SUPPORT;


    public override string GetSettingKey() => "OWP_TicketIssueSupport_Create";

    public override bool IsValid() => TicketId > 0 && Subject.Length > 0;
}