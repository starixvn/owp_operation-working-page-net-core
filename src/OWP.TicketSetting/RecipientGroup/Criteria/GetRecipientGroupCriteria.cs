using Fanex.Data.Repository;

namespace OWP.TicketSetting.RecipientGroup.Criteria;

public class GetRecipientGroupCriteria : CriteriaBase
{
    public override string GetSettingKey() => "OWP_RecipientGroup_GetRecipientGroup";

    public override bool IsValid() => true;
}