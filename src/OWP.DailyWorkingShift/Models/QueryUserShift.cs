namespace OWP.DailyWorkingShift.Models
{
    using System;
    using Attributes;
    using Newtonsoft.Json;

    public class QueryUserShift
    {
        public int EmployeeId { get; set; }

        public DateTime ShiftDate { get; set; }

        public int ShiftId { get; set; }

        [JsonConverter(typeof(DateTimeWithoutGmtConverter))]
        public DateTime ShiftStartTime { get; set; }

        [JsonConverter(typeof(DateTimeWithoutGmtConverter))]
        public DateTime ShiftEndTime { get; set; }
    }
}
