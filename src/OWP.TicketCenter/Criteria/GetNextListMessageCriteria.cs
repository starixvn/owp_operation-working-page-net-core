namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public class GetNextListMessageCriteria : CriteriaBase
{
    public bool CanShowMoreMessage { get; set; }

    public int MsgOfPage { get; set; }

    public int TicketId { get; set; }

    public int? FirstMsgId { get; set; }


    public override string GetSettingKey()
        => "OWP_TicketCenter_GetNextListMessage";

    public override bool IsValid()
        => TicketId > 0;
}