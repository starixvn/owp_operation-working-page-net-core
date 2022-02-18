namespace OWP.OtherSport.Submit.Criteria
{
    using Fanex.Data.Repository;

    public class InsertOrUpdateMatchAssignmentsCommand : NonQueryCommand
    {
        public string MatchAssignments { get; set; }

        public string MatchIds { get; set; }

        public override string GetSettingKey() => "OWP_OSAssignmentUpload_MatchAssignmentsInsertOrUpdate";

        public override bool IsValid() => true;
    }
}