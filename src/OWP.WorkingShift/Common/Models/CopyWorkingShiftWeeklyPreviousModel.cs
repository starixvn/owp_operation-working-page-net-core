namespace OWP.WorkingShift.Common.Models
{
    using System;

    public class CopyWorkingShiftWeeklyPreviousModel
    {
        public int UserId { get; set; }

        public DateTime? StartDateOfWeek { get; set; }
    }
}