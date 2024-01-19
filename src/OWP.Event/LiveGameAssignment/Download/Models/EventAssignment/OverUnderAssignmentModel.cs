namespace OWP.Event.LiveGameAssignment.Download.Models.EventAssignment;

using OWP.Event.EventShared.Enum;
using System;

[Serializable]
public class OverUnderAssignmentModel : TraderAssignmentModel
{
    public OverUnderAssignmentModel()
    {
        Bettype = (int)MajorBetType.OverUnder;
    }

    [Column(ColumnName.MaxBet_OU)]
    public string MaxBet { get; set; }

    [Column(ColumnName.Spread_OU)]
    public string Spread { get; set; }

    [Column(ColumnName.Trader_OU)]
    public string MainTrader { get; set; }

    [Column(ColumnName.OtherTrader_OU)]
    public string OtherTraders { get; set; }
}