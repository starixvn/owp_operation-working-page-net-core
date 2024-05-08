using Fanex.Data.Repository;

namespace OWP.EventPicking.Criteria;
public class UpdateMaxbetSpreadCriteria : NonQueryCommand
{
    public int InfoSiteMatchId { get; set; }

    public decimal? SpreadHdp { get; set; }

    public decimal? SpreadOu { get; set; }

    public decimal? MaxbetHdp { get; set; }

    public string NoOfHDP { get; set; }

    public int ActorId { get; set; }

    public override string GetSettingKey() => "OWP_EventPicking_UpdateMaxbetSpread";

    public override bool IsValid() => true;
}

