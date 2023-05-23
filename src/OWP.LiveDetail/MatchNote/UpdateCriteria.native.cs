namespace OWP.LiveDetail.MatchNote;

public class UpdateCriteria : CommandCriteria
{
    public int MatchNoteId { get; set; }

    public string Note { get; set; }

    public override bool IsValid() => base.IsValid() && MatchNoteId > 0 && !string.IsNullOrWhiteSpace(Note);

    public override string GetSettingKey() => "OWP_LiveDetail_UpdateMatchNote";
}