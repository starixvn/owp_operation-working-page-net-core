namespace OWP.SimpleDataApi.Criteria.EmployeeWorkingShift
{
    using Fanex.Data.Repository;

    internal class EmployeeWorkingShiftUpdateNoteCriteria : CriteriaBase
    {
        public int ActorId { get; set; }

        public int EmployeeWorkingShiftNoteId { get; set; }

        public string Note { get; set; }

        public override bool IsValid() => ActorId > 0 && EmployeeWorkingShiftNoteId > 0 && Note != null;

        public override string GetSettingKey() => "OWP_EmployeeWorkingShift_UpdateNote";
    }
}