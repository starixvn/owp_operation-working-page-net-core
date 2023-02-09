namespace OWP.TicketCenter.Criteria;

using Fanex.Data.Repository;

public class GetTwoLastCommentCriteria : NonQueryCommand
{
    public string TicketId { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_GetTwoLastComment";

    public override bool IsValid()
        => true;
}