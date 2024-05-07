namespace OWP.NewLeagueSetting.Criteria;
public class UpdateLeagueSettingCriteria: NonQueryCommand
{
    public int LeagueSettingId { get; set; }

    public int OWPLeagueId { get; set; }

    public int LeagueGroupId { get; set; }

    public string LeagueGroup { get; set; }

    public decimal? Spread_HDP_value { get; set; } = default!;

    public decimal? Spread_OU_value { get; set; } = default!;

    public string Spread_Note { get; set; } = default!;

    public decimal? MaxbetWD_HDP_value { get; set; } = default!;

    public string MaxbetWD_HDP_Note { get; set; } = default!;

    public string MaxbetWD_NoOfHDP_value { get; set; } = default!;

    public string MaxbetWD_NoOfHDP_Note { get; set; } = default!;

    public decimal? MaxbetWK_HDP_value { get; set; } = default!;

    public string MaxbetWK_HDP_Note { get; set; } = default!;

    public string MaxbetWK_NoOfHDP_value { get; set; } = default!;

    public string MaxbetWK_NoOfHDP_Note { get; set; } = default!;

    public string LstNote { get; set; }

    public string Remark2 { get; set; }

    public string Comment { get; set; }

    public string FullConditionDescription { get; set; }

    public override string GetSettingKey() => "OWP_NewLeagueSetting_UpdateLeagueSetting";

    public override bool IsValid() => true;
}
