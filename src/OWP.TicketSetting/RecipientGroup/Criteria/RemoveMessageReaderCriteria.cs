using Fanex.Data.Repository;

namespace OWP.TicketSetting.RecipientGroup.Criteria;

public class RemoveMessageReaderCriteria : NonQueryCommand
{
    public int GroupId { get; set; }

    public string UserIds { get; set; }

    public override string GetSettingKey() => "OWP_RecipientGroup_RemoveMessageReader";

    public override bool IsValid() => true;
}