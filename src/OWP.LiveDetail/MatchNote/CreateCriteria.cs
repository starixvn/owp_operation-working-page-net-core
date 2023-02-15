namespace OWP.LiveDetail.MatchNote;

public class CreateCriteria : CommandCriteria
{
    public int MatchNoteId { get; set; }

    public int OWPMatchId { get; set; }

    public string Note { get; set; }

    public override bool IsValid() => base.IsValid() && OWPMatchId > 0 && !string.IsNullOrWhiteSpace(Note);

    public override string GetSettingKey() => "OWP_LiveDetail_InsertMatchNote";
}