using Fanex.Data.Repository;

namespace OWP.AssignmentUpload.Submit.Criteria;

public class InsertOrUpdateUploadHistoryCommand : NonQueryCommand
{
    public DateTime EventDate { get; set; }
    public override string GetSettingKey() => "OWP_AssignmentUpload_UploadHistoryInsertOrUpdate";

    public override bool IsValid() => true;
}