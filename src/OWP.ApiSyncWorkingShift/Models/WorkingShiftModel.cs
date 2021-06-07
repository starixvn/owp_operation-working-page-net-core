using System;

namespace OWP.ApiSyncWorkingShift.Models
{
    public class WorkingShiftModel
    {
        public int EmployeeID { get; set; }

        public DateTime? ShiftDate { get; set; }

        public string ShiftCode { get; set; }

        public DateTime? ShiftStartTime { get; set; }

        public DateTime? ShiftEndTime { get; set; }

        public DateTime? LastUpdated { get; set; }
    }
}
