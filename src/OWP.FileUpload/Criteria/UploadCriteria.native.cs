using Fanex.Data.Repository;

namespace OWP.FileUpload.Criteria;

public class UploadCriteria : CriteriaBase
{
    public string Title { get; set; }

    public override string GetSettingKey() => "OWP_FileUpload_Upload";

    public override bool IsValid() => !string.IsNullOrEmpty(Title);
}