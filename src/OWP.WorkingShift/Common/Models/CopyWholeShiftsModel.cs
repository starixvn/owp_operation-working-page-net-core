namespace OWP.WorkingShift.Common.Models
{
    using System;

    public class CopyWholeShiftsModel
    {
        public int FromUserId { get; set; }

        public int ToUserId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}