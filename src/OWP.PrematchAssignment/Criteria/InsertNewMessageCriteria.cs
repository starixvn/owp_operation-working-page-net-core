
using Fanex.Data.Repository;

namespace OWP.PreMatchAssignment.Criteria
{
    public class InsertNewMessageCriteria : NonQueryCommand
    {
        public int ActorId { get; set; }

        public string Message { get; set; }

        public int PreMatchIssueId { get; set; }

        public int? FileId { get; set; }

        public override string GetSettingKey() => "OWP_PreMatchAssignment_InsertNewMessage";

        public override bool IsValid() => ActorId > 0 && PreMatchIssueId > 0;
    }
}