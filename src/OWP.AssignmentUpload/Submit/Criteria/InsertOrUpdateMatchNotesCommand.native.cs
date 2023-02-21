using Fanex.Data.Repository;

namespace OWP.AssignmentUpload.Submit.Criteria;

public class InsertOrUpdateMatchNotesCommand : NonQueryCommand
{
    public string MatchNotes { get; set; }

    public int UserId { get; set; }

    public string MatchIds { get; set; }

    public override string GetSettingKey() => "OWP_AssignmentUpload_MatchNotesInsertOrUpdate";

    public override bool IsValid() => true;
}