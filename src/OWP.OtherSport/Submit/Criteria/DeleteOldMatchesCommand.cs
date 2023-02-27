using Fanex.Data.Repository;

namespace OWP.OtherSport.Submit.Criteria;

public class DeleteOldMatchesCommand : NonQueryCommand
{
    public string MatchIds { get; set; }

    public override string GetSettingKey() => "OWP_OSAssignmentUpload_MatchesDelete";

    public override bool IsValid() => true;
}