namespace OWP.FileUpload.Criteria;

using Fanex.Data.Repository;

public class UploadCriteria : CriteriaBase
{
    public string Title { get; set; }

    public byte[] FileContent { get; set; }

    public override string GetSettingKey() => "OWP_FileUpload_Upload";

    public override bool IsValid() => !string.IsNullOrEmpty(Title) && FileContent.Length > 0;
}