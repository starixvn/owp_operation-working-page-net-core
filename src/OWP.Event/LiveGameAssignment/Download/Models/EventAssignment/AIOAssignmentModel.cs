namespace OWP.Event.LiveGameAssignment.Download.Models.EventAssignment;

public class AIOAssignmentModel
{
    [Column(ColumnName.Trader_AIO)]
    public string MainTrader { get; set; }

    [Column(ColumnName.OtherTrader_AIO)]
    public string OtherTraders { get; set; }
}