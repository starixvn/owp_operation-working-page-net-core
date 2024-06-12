namespace OWP.WorkingShift.Criteria
{
    using Fanex.Data.Repository;

    public class ValidateBeforeDeleteWorkingShiftColorCriteria : NonQueryCommand
    {
        public int ShiftId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid() => ShiftId > 0;

        public override string GetSettingKey() => "OWP_EmployeeWorkingShift_ValidateBeforeDelete_WorkingShiftColor";
    }
}