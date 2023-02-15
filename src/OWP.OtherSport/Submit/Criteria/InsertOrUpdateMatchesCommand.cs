namespace OWP.OtherSport.Submit.Criteria;

using Fanex.Data.Repository;

public class InsertOrUpdateMatchesCommand : NonQueryCommand
{
    public string Matches { get; set; }

    public int UserId { get; set; }

    public override string GetSettingKey() => "OWP_OSAssignmentUpload_MatchInsertOrUpdate";

    public override bool IsValid() => true;
}