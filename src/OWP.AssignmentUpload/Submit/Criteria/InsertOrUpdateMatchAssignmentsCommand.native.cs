namespace OWP.AssignmentUpload.Submit.Criteria
{
    using Fanex.Data.Repository;

    public class InsertOrUpdateMatchAssignmentsCommand : NonQueryCommand
    {
        public string MatchAssignments { get; set; }

        public bool IsUploadFMAndAIO { get; set; }

        public string MatchIds { get; set; }

        public override string GetSettingKey() => "OWP_AssignmentUpload_MatchAssignmentsInsertOrUpdate";

        public override bool IsValid() => true;
    }
}
