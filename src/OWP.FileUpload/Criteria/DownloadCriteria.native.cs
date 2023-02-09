namespace OWP.FileUpload.Criteria;

using Fanex.Data.Repository;

public class DownloadCriteria : CriteriaBase
{
    public int FileId { get; set; }

    public override string GetSettingKey() => "OWP_FileUpload_QueryById";

    public override bool IsValid() => FileId > 0;
}