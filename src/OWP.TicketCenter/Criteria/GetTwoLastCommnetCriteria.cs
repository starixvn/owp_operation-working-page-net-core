using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria
{
    public class GetTwoLastCommnetCriteria : NonQueryCommand
    {
        public string TicketId { get; set; }

        public override string GetSettingKey() => "OWP_TicketCenter_GetTwoLastComment";

        public override bool IsValid()
            => true;
    }
}