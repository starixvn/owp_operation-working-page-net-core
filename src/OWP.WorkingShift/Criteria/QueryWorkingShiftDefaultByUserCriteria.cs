namespace OWP.WorkingShift.Criteria
{
    using Fanex.Data.Repository;

    public class QueryWorkingShiftDefaultByUserCriteria : CriteriaBase
    {
        public string UserIds { get; set; }

        public override bool IsValid() => !string.IsNullOrEmpty(UserIds);

        public override string GetSettingKey() => "OWP_EmployeeWorkingShiftDefault_QueryByUser";
    }
}
