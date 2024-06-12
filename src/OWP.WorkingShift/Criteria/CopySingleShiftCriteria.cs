namespace OWP.WorkingShift.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class CopySingleShiftCriteria : NonQueryCommand
    {
        public int ActorId { get; set; }

        public int UserId { get; set; }

        public DateTime? ShiftDate { get; set; }

        public int ShiftId { get; set; }

        public int? RefShiftId { get; set; }

        public override bool IsValid() => ActorId > 0
            && ShiftId > 0
            && UserId > 0
            && ShiftDate.HasValue;

        public override string GetSettingKey() => "OWP_EmployeeWorkingShift_CopySingleShift";
    }
}