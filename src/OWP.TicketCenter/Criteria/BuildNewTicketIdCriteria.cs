using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria
{
    public class BuildNewTicketIdCriteria : CriteriaBase
    {
        public int TicketType { get; set; }

        public override string GetSettingKey() => "OWP_TicketIssue_Create";

        public override bool IsValid()
            => true;
    }
}