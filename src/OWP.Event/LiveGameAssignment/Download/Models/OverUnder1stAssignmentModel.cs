using OWP.Enums;

namespace OWP.Event.LiveGameAssignment.Download.Models;

using System;

[Serializable]
public class OverUnder1stAssignmentModel : TraderAssignmentModel
{
    public OverUnder1stAssignmentModel()
    {
        Bettype = (int)MajorBetType.OverUnder1St;
    }

    [Column(ColumnName.MaxBet_1HOU)]
    public string MaxBet { get; set; }

    [Column(ColumnName.Spread_1HOU)]
    public string Spread { get; set; }

    [Column(ColumnName.Trader_1HOU)]
    public string MainTrader { get; set; }

    [Column(ColumnName.OtherTrader_1HOU)]
    public string OtherTraders { get; set; }
}