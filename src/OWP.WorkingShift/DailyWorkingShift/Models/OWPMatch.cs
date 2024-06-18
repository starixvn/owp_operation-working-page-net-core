namespace OWP.WorkingShift.DailyWorkingShift.Models;

using System;

public class OWPMatch
{
    public int OWPMatchId { get; set; }

    public DateTime EventDateTime { get; set; }

    public bool IsMinorMatch { get; set; }

    public int? MinorBettypeId { get; set; }

    public int ParentMatchId { get; set; }
}