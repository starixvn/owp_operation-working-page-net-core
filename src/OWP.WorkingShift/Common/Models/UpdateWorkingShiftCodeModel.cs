namespace OWP.WorkingShift.Common.Models
{
    using System;

    public class UpdateWorkingShiftCodeModel
    {
        public int EmployeeId { get; set; }

        public int ShiftId { get; set; }

        public DateTime ShiftDate { get; set; }

        public int? RefShiftId { get; set; }

        public bool IsOvertime { get; set; }
    }
}
