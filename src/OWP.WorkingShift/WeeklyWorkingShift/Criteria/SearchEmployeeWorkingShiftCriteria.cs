namespace OWP.WorkingShift.WeeklyWorkingShift.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class SearchEmployeeWorkingShiftCriteria : CriteriaBase
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string User { get; set; }

        public int SiteId { get; set; }

        public string SectionId { get; set; }

        public string DepartmentId { get; set; }

        public string CategoryId { get; set; }

        public string LocationCode { get; set; }

        public bool ExcludeClosedAccount { get; set; }

        public int? OrderUserId { get; set; }

        public override string GetSettingKey() => "OWP_EmployeeWorkingShift_QueryWeekly";

        public override bool IsValid() => SiteId > 0 && FromDate.HasValue && ToDate.HasValue;
    }
}