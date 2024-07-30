

using Newtonsoft.Json;

namespace OWP.EventPicking.Api.Models;

public class LeagueSetting
{
    public int LeagueSettingId { get; set; }

    public int OWPLeagueId { get; set; }

    public string LeagueName { get; set; }

    public string LSTNote { get; set; }

    public string FormatedLSTNote => ConvertNullToEmpty(LSTNote);

    public bool HasTV { get; set; }

    public string FullConditionDescription { get; set; }

    public decimal? Spread_HDP { get; set; }

    public decimal? Spread_OU { get; set; }

    public decimal? Spread_1HHDP { get; set; }

    public decimal? Spread_1HOU { get; set; }

    public decimal? MaxBet_FT1X2 { get; set; }

    public decimal? MaxBet_1X2Spread { get; set; }

    public decimal? MaxBet_1H1X2 { get; set; }

    public decimal? Spread_1H1X2 { get; set; }

    //WD
    public string MaxbetWD_MatchOffer { get; set; }

    public decimal? MaxbetWD_HDP { get; set; }

    public decimal? MaxbetWD_OU { get; set; }

    public string MaxbetWD_NoOfHDP { get; set; }

    public decimal? MaxbetWD_1HHDP { get; set; }

    public decimal? MaxbetWD_1HOU { get; set; }

    //WK
    public string MaxbetWK_MatchOffer { get; set; }

    public decimal? MaxbetWK_HDP { get; set; }

    public decimal? MaxbetWK_OU { get; set; }

    public string MaxbetWK_NoOfHDP { get; set; }

    public decimal? MaxbetWK_1HHDP { get; set; }

    public decimal? MaxbetWK_1HOU { get; set; }

    private static string ConvertNullToEmpty(string value)
    {
        return string.IsNullOrEmpty(value)
            ? string.Empty
            : value.Trim();
    }

    public LeagueSettingCondition GetFullConditionDes() => string.IsNullOrEmpty(FullConditionDescription)
        ? new LeagueSettingCondition()
        : JsonConvert.DeserializeObject<LeagueSettingCondition>(FullConditionDescription);
}

public class LeagueSettingCondition
{
    public IEnumerable<Condition> Rounds { get; set; } = new List<Condition>();

    public IEnumerable<Condition> Divisions { get; set; } = new List<Condition>();
}

public class Condition
{
    public string Type { get; set; }

    public string Value { get; set; }
}

