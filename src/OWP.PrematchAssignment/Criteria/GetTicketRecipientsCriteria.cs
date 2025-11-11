using Fanex.Data.Repository;

namespace OWP.PrematchAssignment.Criteria;

public class GetTicketRecipientsCriteria : CriteriaBase
{
    public int TicketTypeId { get; set; }

    public string SubTypeName { get; set; }

    public override string GetSettingKey() => "OWP_TicketCenter_GetDefaultRecipientBySubTypeIdAndName";

    public override bool IsValid() => TicketTypeId > 0;
}