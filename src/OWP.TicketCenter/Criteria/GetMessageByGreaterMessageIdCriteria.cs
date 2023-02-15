namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public class GetMessageByGreaterMessageIdCriteria : CriteriaBase
{
    public int TicketId { get; set; }

    public bool CanShowMoreMessage { get; set; }

    public int MessageId { get; set; }

    public override bool IsValid() => TicketId > 0;

    public override string GetSettingKey() => "OWP_TicketCenter_GetMessageByGreaterMessageId";
}