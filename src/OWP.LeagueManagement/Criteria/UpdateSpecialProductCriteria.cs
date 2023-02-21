using Fanex.Data.Repository;

namespace OWP.LeagueManagement.Criteria;

public class UpdateSpecialProductCriteria : CriteriaBase
{
    public int ActorId { get; set; }

    public int ProductId { get; set; } = -1;

    public decimal? Maxbet_FTHDP { get; set; }

    public decimal? Maxbet_NoOfCorner { get; set; }

    public decimal? Spread_NoOfCorner { get; set; }

    public decimal? Maxbet_NextCorner { get; set; }

    public decimal? Spread_NextCorner { get; set; }

    public decimal? Maxbet_10_15Mins { get; set; }

    public decimal? Spread_10_15Mins { get; set; }

    public decimal? Maxbet_NoOfTB { get; set; }

    public decimal? Spread_NoOfTB { get; set; }

    public decimal? Maxbet_NextTB { get; set; }

    public decimal? Spread_NextTB { get; set; }

    public decimal? Maxbet_TB { get; set; }

    public decimal? Spread_TB { get; set; }

    public decimal? Maxbet_PS { get; set; }

    public decimal? Spread_PS { get; set; }

    public decimal? Maxbet_Winner { get; set; }

    public decimal? Spread_Winner { get; set; }

    public string Remark { get; set; }

    public string Type { get; set; }

    public override string GetSettingKey() => "OWP_SpecialProduct_Update";

    public override bool IsValid() => true;
}