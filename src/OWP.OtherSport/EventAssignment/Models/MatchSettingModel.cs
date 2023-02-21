using OWP.OtherSport.Submit.Constants;

namespace OWP.OtherSport.EventAssignment.Models;

using Extensions;

public class MatchSettingModel
{
    public string BetTypeName { get; set; }

    public decimal? MaxBet { get; set; }

    public string Spread { get; set; }

    public int Order => MinorBettypeOrder.GetValue(BetTypeName, 0);

    private readonly Dictionary<string, int> MinorBettypeOrder = new()
    {
        { MinorBettype.HDP, 1 },
        { MinorBettype.OU, 2 },
        { MinorBettype.OE, 3 }
    };
}