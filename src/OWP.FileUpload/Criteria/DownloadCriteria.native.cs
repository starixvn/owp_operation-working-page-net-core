using Fanex.Data.Repository;

namespace OWP.FileUpload.Criteria;

public class DownloadCriteria : CriteriaBase
{
    public int FileId { get; set; }

    public override string GetSettingKey() => "OWP_FileUpload_QueryById";

    public override bool IsValid() => FileId > 0;
}