using Fanex.Data.Repository;

namespace OWP.SimpleDataApi.Criteria.TicketCenter
{
    public class GetDefaultRecipientCriteria : CriteriaBase
    {
        public int TicketTypeId { get; set; }

        public override string GetSettingKey() => "OWP_TicketCenter_GetDefaultRecipientByTicketTypeId";

        public override bool IsValid() => true;
    }
}