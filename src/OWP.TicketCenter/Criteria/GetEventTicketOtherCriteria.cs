namespace OWP.TicketCenter.Criteria
{
    using Fanex.Data.Repository;

    public class GetEventTicketOtherCriteria : GetEventTicketBaseCriteria
    {
        public override string GetSettingKey()
            => "OWP_TicketCenterOther_Get";

    }
}