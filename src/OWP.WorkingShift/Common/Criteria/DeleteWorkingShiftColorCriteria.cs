namespace OWP.WorkingShift.Common.Criteria
{
    using Fanex.Data.Repository;

    public class DeleteWorkingShiftColorCriteria : NonQueryCommand
    {
        public int ActorId { get; set; }

        public int ShiftId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid() => ActorId > 0 && ShiftId > 0;


        public override string GetSettingKey() => "OWP_EmployeeWorkingShift_QuickDelete_WorkingShiftColor";
    }
}