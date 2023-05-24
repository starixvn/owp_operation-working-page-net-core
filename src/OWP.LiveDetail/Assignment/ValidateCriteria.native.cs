using Fanex.Data.Repository;

namespace OWP.LiveDetail.Assignment;

public class ValidateCriteria : CriteriaBase
{
    public int OWPMatchId { get; set; }

    public int Bettype { get; set; }

    public int AssignmentType { get; set; }

    public string Username { get; set; }

    public int? UserId { get; set; }

    public bool IsValidateAssignment { get; set; }

    public override bool IsValid() => OWPMatchId > 0 && Bettype != 0 && AssignmentType > 0;

    public override string GetSettingKey() => "OWP_ValidationRule_CheckWhenEdit";
}