using Fanex.Data.Repository;

namespace OWP.AssignmentUpload.Submit.Criteria;

public class InsertOrUpdateMatchTelevisionsCommand : NonQueryCommand
{
    public string MatchTVs { get; set; }

    public string MatchIds { get; set; }

    public override string GetSettingKey() => "OWP_AssignmentUpload_MatchTelevisionsInsertOrUpdate";

    public override bool IsValid() => true;
}