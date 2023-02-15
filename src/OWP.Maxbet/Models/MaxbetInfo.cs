namespace OWP.Maxbet.Models;

using Newtonsoft.Json;

public class MaxbetInfo
{
    public int AdminsiteID { get; set; }

    public TraderAssignmentSender Traders { get; set; } = new();

    public int? SecondAcceptValue { get; set; }

    public MaxbetSpread Spread { get; set; }

    public MaxbetSpread Maxbet { get; set; }

    public MaxbetSpread MaxbetCN88 { get; set; }

    public NumberOfLines NumberOfLines { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int? Ft1stLineHandicap { get; set; }
}