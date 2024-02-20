namespace OWP.Event.LiveGameAssignment.Download.Models;

using OWP.Event.EventShared.Enum;
using System;

[Serializable]
public class LAAssignmentModel : BettypeAssignmentBaseModel
{
    public LAAssignmentModel()
    {
        MonitorType = (int)EventMonitorType.LiveAssistant;
    }
}