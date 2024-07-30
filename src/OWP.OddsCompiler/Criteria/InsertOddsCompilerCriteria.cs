using Fanex.Data.Repository;


namespace OWP.OddsCompiler.Criteria;
public class InsertOddsCompilerCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public int OWPMatchId { get; set; }

    public int Bettype { get; set; }

    public int? TeamChoice { get; set; }

    public decimal? HDP { get; set; }

    public decimal? Odds1 { get; set; }

    public decimal? Odds2 { get; set; }

    public decimal? Odds3 { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "OWP_OddsCompiler_InsertOdds";

    public override bool IsValid() => OWPMatchId > 0 && ActorId > 0 && Bettype > 0;
}