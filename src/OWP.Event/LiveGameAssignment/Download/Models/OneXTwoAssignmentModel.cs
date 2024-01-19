namespace OWP.Event.LiveGameAssignment.Download.Models;

using OWP.Event.EventShared.Enum;
using System;

[Serializable]
public class OneXTwoAssignmentModel : TraderAssignmentModel
{
    public OneXTwoAssignmentModel()
    {
        Bettype = (int)MajorBetType.OneXTwo;
    }

    [Column(ColumnName.MaxBet_1X2)]
    public string MaxBet { get; set; }

    [Column(ColumnName.Spread_1X2)]
    public string Spread { get; set; }

    [Column(ColumnName.Trader_1X2)]
    public string MainTrader { get; set; }

    [Column(ColumnName.OtherTrader_1X2)]
    public string OtherTraders { get; set; }
}