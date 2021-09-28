namespace OWP.TicketCenter.Criteria
{
    using Fanex.Data.Repository;

    public class GetEventTicketMatchCriteria : GetEventTicketCriteriaBase
    {
        public override string GetSettingKey()
            => "OWP_TicketCenterEvent_Get";
    }
}