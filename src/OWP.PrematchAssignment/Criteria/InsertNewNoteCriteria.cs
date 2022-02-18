namespace OWP.PreMatchAssignment.Criteria
{
    using Fanex.Data.Repository;

    public class InsertNewNoteCriteria : NonQueryCommand
    {
        public int ActorId { get; set; }

        public string Message { get; set; }

        public int? OWPLeagueId { get; set; }

        public DateTime EventDate { get; set; }

        public int? InfoSiteMatchId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Username { get; set; }

        public int PreMatchNoteId { get; set; }

        public int PreMatchNoteMessageId { get; set; }

        public override string GetSettingKey() => "OWP_PreMatchAssignment_InsertNewNote";

        public override bool IsValid() => ActorId > 0 && (OWPLeagueId > 0 || InfoSiteMatchId > 0) && !string.IsNullOrWhiteSpace(Message);
    }
}