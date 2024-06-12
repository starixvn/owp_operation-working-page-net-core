namespace OWP.WorkingShift.Criteria
{
    using Fanex.Data.Repository;

    public class GetAllWorkingShiftColorCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int? TimeZoneOffset { get; set; }

        public override bool IsValid() => UserId >= 0;

        public override string GetSettingKey() => "OWP_EmployeeWorkingShift_GetAllShift";
    }
}
