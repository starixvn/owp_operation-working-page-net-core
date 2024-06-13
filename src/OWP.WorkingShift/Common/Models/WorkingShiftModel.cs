namespace OWP.WorkingShift.Common.Models
{
    using System;

    public class WorkingShiftModel
    {
        public int UserId { get; set; }

        public int ShiftId { get; set; }

        public DateTime ShiftDate { get; set; }

        public DateTime? ShiftStartTime { get; set; }

        public DateTime? ShiftEndTime { get; set; }

        public int? RefShiftId { get; set; }

        public string StartTimeString => ShiftStartTime.HasValue ? ShiftStartTime.Value.ToString("HH:mm") : string.Empty;

        public string EndTimeString => ShiftEndTime.HasValue ? ShiftEndTime.Value.ToString("HH:mm") : string.Empty;
    }
}