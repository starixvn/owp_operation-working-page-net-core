using Fanex.Data.Repository;

namespace OWP.AssignmentUpload.Submit.Criteria;

public class InsertOrUpdateMatchOtherProvidersCommand : NonQueryCommand
{
    public string MatchProviders { get; set; }

    public string MatchIds { get; set; }

    public override string GetSettingKey() => "OWP_AssignmentUpload_MatchOtherProvidersInsertOrUpdate";

    public override bool IsValid() => true;
}