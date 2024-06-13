namespace OWP.WorkingShift.Common.Criteria
{
    using Fanex.Data.Repository;

    public class UpdateWorkingShiftColorCriteria : NonQueryCommand
    {
        public int ActorId { get; set; }

        public int ShiftId { get; set; }

        public string Background { get; set; }

        public string Color { get; set; }

        public override bool IsValid() => ActorId > 0 && ShiftId > 0;

        public override string GetSettingKey() => "OWP_EmployeeWorkingShiftColorUpd";
    }
}