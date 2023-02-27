using Fanex.Data.Repository;

namespace OWP.AssignmentUpload.Submit.Criteria;

public class InsertOrUpdateMatchAgentsCommand : NonQueryCommand
{
    public string MatchAgents { get; set; }

    public string MatchIds { get; set; }

    public override string GetSettingKey() => "OWP_AssignmentUpload_MatchAgentsInsertOrUpdate";

    public override bool IsValid() => true;
}