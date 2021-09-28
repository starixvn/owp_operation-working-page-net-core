namespace OWP.TicketCenter.Criteria
{
    using Fanex.Data.Repository;

    public class GetEventTicketOtherCriteria : GetEventTicketCriteriaBase
    {
        public override string GetSettingKey()
            => "OWP_TicketCenterOther_Get";

    }
}