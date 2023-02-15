namespace OWP.LiveGameList.Models;

using Helper;
using Newtonsoft.Json;

public class MatchSetting
{
    [JsonIgnore]
    public int OWPMatchId { get; set; }

    [JsonIgnore]
    public int ParentMatchId { get; set; }

    [JsonIgnore]
    public int? MinorBettypeId { get; set; }

    public int BettypeId { get; set; }

    public decimal? Maxbet { get; set; }

    public decimal? Spread { get; set; }

    public string MaxbetSpreadToDisplay => LiveGameListHelper.BuildDisplayMaxbetSpread(Maxbet, Spread);

    [JsonIgnore]
    public bool IsHasMaxbetSpread => Maxbet.HasValue || Spread.HasValue;
}