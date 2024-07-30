using Fanex.Data.Repository;

namespace OWP.TicketSetting.RecipientGroup.Criteria;

public class InsertMessageReaderCriteria : NonQueryCommand
{
    public int GroupId { get; set; }

    public string UserIds { get; set; }

    public override string GetSettingKey() => "OWP_RecipientGroup_InsertMessageReader";

    public override bool IsValid() => true;
}