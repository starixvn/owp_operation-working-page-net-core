using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria.NewTicket
{
    public class GetEventCriteria : CriteriaBase
    {
        public string Keyword { get; set; }

        public override string GetSettingKey()
            => "OWP_TicketCenter_NewTicket_GetEvent";

        public override bool IsValid()
            => true;
    }
}
