namespace OWP.ProviderVerification.Models;

public class AnalyzedResult
{
    public List<ExcelProviderModel> ProviderModel { get; set; }

    public int OriginalRecordCount { get; set; }
}