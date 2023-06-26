using AssignmentTypeEnum = OWP.Enums.AssignmentType;

namespace OWP.LiveDetail.Assignment;

public class CreateCriteria : CommandCriteria
{
    public int OWPMatchId { get; set; }

    public int BetType { get; set; }

    public short AssignmentType { get; set; }

    public int? UserId { get; set; }

    public string AssignmentNote { get; set; }

    public override bool IsValid()
    {
        return base.IsValid()
               && OWPMatchId > 0
               && BetType != 0
               && HasAssignment();
    }

    private bool HasAssignment()
    {
        var hasMainAssignment = AssignmentType == (int)AssignmentTypeEnum.Main && UserId is > 0;
        var hasOtherAssignment = AssignmentType == (int)AssignmentTypeEnum.Others && !string.IsNullOrWhiteSpace(AssignmentNote);
        return hasMainAssignment || hasOtherAssignment;
    }

    public override string GetSettingKey() => "OWP_Assignment_Create";
}