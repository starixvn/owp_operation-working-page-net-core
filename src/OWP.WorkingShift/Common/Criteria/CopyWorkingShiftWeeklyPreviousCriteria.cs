namespace OWP.WorkingShift.Common.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class CopyWorkingShiftWeeklyPreviousCriteria : NonQueryCommand
    {
        public int ActorId { get; set; }

        public int UserId { get; set; }

        public DateTime? StartDateOfWeek { get; set; }

        public override bool IsValid() => ActorId > 0 && UserId > 0 && StartDateOfWeek.HasValue;

        public override string GetSettingKey() => "OWP_EmployeeWorkingShift_CopyWeekly";
    }
}