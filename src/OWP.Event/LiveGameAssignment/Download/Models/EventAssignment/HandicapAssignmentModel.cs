namespace OWP.Event.LiveGameAssignment.Download.Models.EventAssignment;

using OWP.Event.EventShared.Enum;
using System;

[Serializable]
public class HandicapAssignmentModel : TraderAssignmentModel
{
    public HandicapAssignmentModel()
    {
        Bettype = (int)MajorBetType.Handicap;
    }

    [Column(ColumnName.MaxBet_HDP)]
    public string MaxBet { get; set; }

    [Column(ColumnName.Spread_HDP)]
    public string Spread { get; set; }

    [Column(ColumnName.Trader_HDP)]
    public string MainTrader { get; set; }

    [Column(ColumnName.OtherTrader_HDP)]
    public string OtherTraders { get; set; }
}