using Fanex.Data.Repository;

namespace OWP.NewLeagueSetting.Criteria;
public class InsertLeagueSettingCriteria : NonQueryCommand
{
    public string LeagueName { get; set; }

    public int? LeagueGroupId { get; set; }

    public decimal? Spread_HDP_Value { get; set; } = default!;

    public decimal? Spread_OU_Value { get; set; } = default!;

    public string Spread_Note { get; set; } = default!;

    public decimal? MaxbetWD_HDP_Value { get; set; } = default!;

    public string MaxbetWD_HDP_Note { get; set; } = default!;

    public string WD_NoOfHDP_Value { get; set; } = default!;

    public string WD_NoOfHDP_Note { get; set; } = default!;

    public decimal? MaxbetWK_HDP_Value { get; set; } = default!;

    public string MaxbetWK_HDP_Note { get; set; } = default!;

    public string WK_NoOfHDP_Value { get; set; } = default!;

    public string WK_NoOfHDP_Note { get; set; } = default!;

    public string LstNote { get; set; }

    public string Remark2 { get; set; }

    public string Comment { get; set; }

    public int ActorId { get; set; }

    public int LeagueSettingId { get; set; }

    public string FullConditionDescription { get; set; }

    public override string GetSettingKey() => "OWP_NewLeagueSetting_InsertLeagueSetting";

    public override bool IsValid() => LeagueName is not null;
}