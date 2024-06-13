namespace OWP.WorkingShift.Common.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class CopyWholeShiftsCriteria : NonQueryCommand
    {
        public int ActorId { get; set; }

        public int FromUserId { get; set; }

        public int ToUserId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public override bool IsValid() => ActorId > 0
            && IsHasValidUsers()
            && IsHasValidTimeRange();

        private bool IsHasValidUsers()
        {
            return FromUserId != ToUserId
            && FromUserId > 0
            && ToUserId > 0;
        }

        private bool IsHasValidTimeRange()
        {
            return FromDate.HasValue
            && ToDate.HasValue;
        }

        public override string GetSettingKey() => "OWP_EmployeeWorkingShift_CopyWholeShifts";
    }
}