namespace OWP.WorkingShift.Common.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class QueryWorkingShiftBySectionCriteria : CriteriaBase
    {
        public string SectionId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public override bool IsValid() => !string.IsNullOrEmpty(SectionId) && FromDate.HasValue && ToDate.HasValue;

        public override string GetSettingKey() => "OWP_EmployeeWorkingShift_QueryWeeklyBySection";
    }
}