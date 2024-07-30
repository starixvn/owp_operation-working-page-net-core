namespace OWP.DailyWorkingShift.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class QueryAssignmentStatsCriteria : CriteriaBase
    {
        public DateTime? Date { get; set; }

        public override string GetSettingKey() => "OWP_EmployeeWorkingShift_QueryAssignmentStats";

        public override bool IsValid() => Date.HasValue;
    }
}