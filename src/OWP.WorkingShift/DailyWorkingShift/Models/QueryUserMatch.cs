namespace OWP.WorkingShift.DailyWorkingShift.Models
{
    using System;

    public class QueryUserMatch
    {
        public int? UserId { get; set; }

        public int OWPMatchId { get; set; }

        public int Bettype { get; set; }

        public DateTime EventDateTime { get; set; }

        public string AssignmentNote { get; set; }

        public bool IsAllowedToViewDetail { get; set; }
    }
}