namespace OWP.OddsCompiler.Criteria;
using Fanex.Data.Repository;

public class DeleteOddsCompilerCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public string MatchOddsIds { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "OWP_OddsCompiler_DeleteOdds";

    public override bool IsValid() => ActorId > 0 && !string.IsNullOrWhiteSpace(MatchOddsIds);
}