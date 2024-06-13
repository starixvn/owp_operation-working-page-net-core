namespace OWP.WorkingShift.WeeklyWorkingShift.Models
{
    using System;

    public class ImportValidCell : ImportCell
    {
        public ImportValidCell(string address, int employeeId, DateTime shiftDate, int shiftId) : base(address)
        {
            EmployeeId = employeeId;
            ShiftDate = shiftDate;
            ShiftId = shiftId;
        }

        public int EmployeeId { get; set; }

        public int ShiftId { get; set; }

        public DateTime ShiftDate { get; set; }
    }
}
