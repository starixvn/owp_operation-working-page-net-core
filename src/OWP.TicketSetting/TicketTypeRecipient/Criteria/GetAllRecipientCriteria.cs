using Fanex.Data.Repository;

namespace OWP.TicketSetting.TicketTypeRecipient.Criteria;

public class GetAllRecipientCriteria : CriteriaBase
{
    public bool IncludeGroup { get; set; }

    public bool IncludeSection { get; set; }

    // TODO
    public string AllUserJson { get; set; }

    public override string GetSettingKey() => "OWP_TicketSetting_GetAllRecipients";

    public override bool IsValid() => true;
}
