namespace OWP.DailyWorkingShift.Models
{
    using System;

    public class EmployeeWorkingShiftNote
    {
        public int EmployeeWorkingShiftNoteId { get; set; }

        public int UserId { get; set; }

        public string Note { get; set; }

        public string Username { get; set; }

        public DateTime LastUpdated { get; set; }

        public DateTime ShiftDate { get; set; }
    }
}