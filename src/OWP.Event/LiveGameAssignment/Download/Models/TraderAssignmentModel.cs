namespace OWP.Event.LiveGameAssignment.Download.Models;

using OWP.Event.EventShared.Enum;
using System;

[Serializable]
public class TraderAssignmentModel : BettypeAssignmentBaseModel
{
    protected TraderAssignmentModel()
    {
        MonitorType = (int)EventMonitorType.Trader;
    }
}