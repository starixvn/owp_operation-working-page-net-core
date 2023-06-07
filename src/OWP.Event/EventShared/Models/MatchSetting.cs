using OWP.Event.EventShared.Helper;
using System.Text.Json.Serialization;

namespace OWP.Event.EventShared.Models;

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

    public string MaxbetSpreadToDisplay => EventHelper.BuildDisplayMaxbetSpread(Maxbet, Spread);

    [JsonIgnore]
    public bool IsHasMaxbetSpread => Maxbet.HasValue || Spread.HasValue;
}