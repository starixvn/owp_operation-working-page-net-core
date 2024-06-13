namespace OWP.WorkingShift.WeeklyWorkingShift.Criteria
{
    using Fanex.Data.Repository;

    public class EmployeeWorkingShiftViewLogCriteria : CriteriaBase
    {
        public int ObjectId { get; set; }

        public int PageSize { get; set; }

        public override string GetSettingKey() => "OWP_EmployeeWorkingShift_ViewLog";

        public override bool IsValid() => ObjectId > 0 && PageSize > 0;
    }
}
