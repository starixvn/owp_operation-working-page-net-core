using OWP.Extensions;
using OWP.OtherSport.Submit.Contants;

namespace OWP.OtherSport.EventAssignment.Models;

public class MatchSettingModel
{
    public string BetTypeName { get; set; }

    public decimal? MaxBet { get; set; }

    public string Spread { get; set; }

    public int Order
    {
        get => MinorBettypeOrder.GetValue(BetTypeName, 0);
    }

    private readonly Dictionary<string, int> MinorBettypeOrder = new Dictionary<string, int>
    {
        { MinorBettype.HDP, 1 },
        { MinorBettype.OU, 2 },
        { MinorBettype.OE, 3 }
    };
}