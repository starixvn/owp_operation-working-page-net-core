using OWP.Enums;

namespace OWP.Event.LiveGameAssignment.Download.Models;

[Serializable]
public class Handicap1stAssignmentModel : TraderAssignmentModel
{
    public Handicap1stAssignmentModel()
    {
        Bettype = (int)MajorBetType.Handicap1St;
    }

    [Column(ColumnName.MaxBet_1HHDP)]
    public string MaxBet { get; set; }

    [Column(ColumnName.Spread_1HHDP)]
    public string Spread { get; set; }

    [Column(ColumnName.Trader_1HHDP)]
    public string MainTrader { get; set; }

    [Column(ColumnName.OtherTrader_1HHDP)]
    public string OtherTraders { get; set; }
}