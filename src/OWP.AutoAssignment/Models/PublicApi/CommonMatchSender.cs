using Newtonsoft.Json;

namespace OWP.AutoAssignment.Models.PublicApi;

public class CommonMatchSender
{
    public int[] PickedBettypes { get; set; } = { 0 };

    public string KOTime { get; set; }

    public int MatchId { get; set; }

    [JsonIgnore]
    public int? LeagueSettingId { get; set; }
}