namespace OWP.Event.LiveGameAssignment.Download.Models;

using System;

[Serializable]
public class FMAssignmentModel
{
    [Column(ColumnName.MaxBet_FM)]
    public string MaxBet { get; set; }

    [Column(ColumnName.Spread_FM)]
    public string Spread { get; set; }

    [Column(ColumnName.Trader_FM)]
    public string MainTrader { get; set; }

    [Column(ColumnName.OtherTrader_FM)]
    public string OtherTraders { get; set; }
}