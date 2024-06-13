namespace OWP.WorkingShift.Common.Criteria
{
    using Fanex.Data.Repository;

    public class GetTimezoneSettingCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public override bool IsValid() => UserId >= 0;

        public override string GetSettingKey() => "OWP_EmployeeWorkingShiftTimeZone_GetByUser";
    }
}
