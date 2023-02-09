namespace OWP.AssignmentUpload.Submit.Criteria;

using Fanex.Data.Repository;

public class InsertOrUpdateMatchSettingsCommand : NonQueryCommand
{
    public string MatchSettings { get; set; }

    public bool IsUploadFMAndAIO { get; set; }

    public string MatchIds { get; set; }

    public override string GetSettingKey() => "OWP_AssignmentUpload_MatchSettingsInsertOrUpdate";

    public override bool IsValid() => true;
}