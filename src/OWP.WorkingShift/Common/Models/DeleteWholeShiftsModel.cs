namespace OWP.WorkingShift.Common.Models
{
    using System;

    public class DeleteWholeShiftsModel
    {
        public int UserId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}