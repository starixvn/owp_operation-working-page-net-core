namespace OWP.LiveDetail.Assignment;

public class UpdateCriteria : CommandCriteria
{
    public int AssignmentId { get; set; }

    public int? UserId { get; set; }

    public string AssignmentNote { get; set; }

    public override bool IsValid()
    {
#pragma warning disable S1067 // Expressions should not be too complex
        return base.IsValid()
               && AssignmentId > 0
               && (UserId != null && UserId.Value > 0 || !string.IsNullOrWhiteSpace(AssignmentNote));
#pragma warning restore S1067 // Expressions should not be too complex
    }

    public override string GetSettingKey() => "OWP_Assignment_Update";
}