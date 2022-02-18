

namespace OWP.PreMatchAssignment.Criteria
{
    
    using Fanex.Data.Repository;

    public class EditNoteMessageCriteria : NonQueryCommand
    {
        public int ActorId { get; set; }

        public string Message { get; set; }

        public int PreMatchNoteMessageId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Username { get; set; }

        public override string GetSettingKey() => "OWP_PreMatchAssignment_EditNoteMessage";

        public override bool IsValid() => ActorId > 0 && PreMatchNoteMessageId > 0 && !string.IsNullOrWhiteSpace(Message);
    }
}