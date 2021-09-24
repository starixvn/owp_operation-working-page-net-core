namespace OWP.TicketCenter.Criteria
{
    using Fanex.Data.Repository;

    public class GetEventTicketOtherCriteria : NonQueryCommand
    {
        public int ActorId { get; set; }

        public string SubjectKeyword { get; set; }

        public override string GetSettingKey()
            => "OWP_TicketCenterOther_Get";

        public override bool IsValid()
            => true;
    }
}