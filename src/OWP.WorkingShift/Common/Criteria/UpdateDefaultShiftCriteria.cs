namespace OWP.WorkingShift.Common.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class UpdateDefaultShiftCriteria : NonQueryCommand
    {
        public int Actor { get; set; }

        public int EmployeeId { get; set; }

        public string DefaultShiftIds { get; set; }

        public bool IsApplyCurrentWeek { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public override bool IsValid() => Actor > 0
            && EmployeeId > 0
            && !string.IsNullOrWhiteSpace(DefaultShiftIds)
            && IsHasValidTimeRange();

        private bool IsHasValidTimeRange() => FromDate.HasValue && ToDate.HasValue;

        public override string GetSettingKey() => "OWP_EmployeeWorkingShift_UpdateDefaultShift";
    }
}