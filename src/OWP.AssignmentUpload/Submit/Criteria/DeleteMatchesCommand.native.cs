using Fanex.Data.Repository;

namespace OWP.AssignmentUpload.Submit.Criteria;

public class DeleteMatchesCommand : NonQueryCommand
{
    public string MatchIds { get; set; }

    public override string GetSettingKey() => "OWP_AssignmentUpload_MatchesDel";

    public override bool IsValid() => true;
}