using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria
{
    public class AddNewTicketIssueMessageCriteria : CriteriaBase
    {
        public int TicketId { get; set; }

        public string Message { get; set; }

        public int UserId { get; set; }

        public int? FileId { get; set; }

        public bool IsSaved { get; set; }

        public override string GetSettingKey()
            => "OWP_TicketIssueMessage_Create";

        public override bool IsValid()
            => TicketId > 0 &&
               Message.Length > 0 &&
               UserId > 0;
    }
}