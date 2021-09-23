using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria.NewTicket
{
    public class AddNewTicketIssueOther : NonQueryCommand
    {
        public int TicketId { get; set; }

        public string Subject { get; set; }

        public override string GetSettingKey()
            => "OWP_TicketIssueOther_Create";

        public override bool IsValid()
            => TicketId > 0 && Subject.Length > 0;
    }
}
