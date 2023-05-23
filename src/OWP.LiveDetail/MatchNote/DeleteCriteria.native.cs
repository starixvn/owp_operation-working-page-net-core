namespace OWP.LiveDetail.MatchNote;

public class DeleteCriteria : CommandCriteria
{
    public int MatchNoteId { get; set; }

    public override bool IsValid() => base.IsValid() && MatchNoteId > 0;

    public override string GetSettingKey() => "OWP_LiveDetail_DeleteMatchNote";
}