namespace OWP.WorkingShift.Models
{
    using System;

    public class WorkingShiftQueryBySingleUserModel
    {
        public int UserId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
