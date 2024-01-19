namespace OWP.Event.LiveGameAssignment.Download.Models.EventAssignment;

using OWP.Extensions;
using System;

[Serializable]
public class BettypeAssignmentBaseModel
{
    [ExcelIgnore]
    public int Bettype { get; set; }

    [ExcelIgnore]
    public int MonitorType { get; set; }
}