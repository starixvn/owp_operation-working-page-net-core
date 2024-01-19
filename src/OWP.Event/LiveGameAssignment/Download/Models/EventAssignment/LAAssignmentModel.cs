namespace OWP.Event.LiveGameAssignment.Download.Models.EventAssignment;

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