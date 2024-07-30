using Fanex.Data.Repository;
namespace OWP.OddsCompiler.Criteria;
public class UpdateOddsCompilerCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public int MatchRecommendOddsId { get; set; }

    public int? TeamChoice { get; set; }

    public decimal? HDP { get; set; }

    public decimal? Odds1 { get; set; }

    public decimal? Odds2 { get; set; }

    public decimal? Odds3 { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "OWP_OddsCompiler_UpdateOdds";

    public override bool IsValid() => MatchRecommendOddsId > 0 && ActorId > 0;
}