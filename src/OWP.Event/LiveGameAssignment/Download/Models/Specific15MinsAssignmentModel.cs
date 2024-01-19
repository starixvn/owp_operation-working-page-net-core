namespace OWP.Event.LiveGameAssignment.Download.Models;

using System;

[Serializable]
public class Specific15MinsAssignmentModel : TraderAssignmentModel
{
    [Column(ColumnName.Trader_15Mins)]
    public string MainTrader { get; set; }

    [Column(ColumnName.OtherTrader_15Mins)]
    public string OtherTraders { get; set; }
}