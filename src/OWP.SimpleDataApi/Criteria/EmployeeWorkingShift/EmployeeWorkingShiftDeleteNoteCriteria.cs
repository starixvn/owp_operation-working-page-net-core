namespace OWP.SimpleDataApi.Criteria.EmployeeWorkingShift;

using Fanex.Data.Repository;

public class EmployeeWorkingShiftDeleteNoteCriteria : NonQueryCommand
{
    public int ActorId { get; set; }

    public int EmployeeWorkingShiftNoteId { get; set; }

    public override bool IsValid() => EmployeeWorkingShiftNoteId > 0;

    public override string GetSettingKey() => "OWP_EmployeeWorkingShift_DeleteNote";
}