namespace OWP.WorkingShift.Models
{
    using System;

    public class UpdateDefaultShiftModel
    {
        public int EmployeeId { get; set; }

        public string DefaultShiftIds { get; set; }

        public bool IsApplyCurrentWeek { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
