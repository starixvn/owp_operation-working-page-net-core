namespace OWP.ProviderVerification.ProviderVerification.Export;

public record ExportRequest(DateTimeOffset filterDate, string SessionId, string Template) : Query<ExcelPackage>;

public class ExportHandler(ICache cache, ConfigurationService configuration) : QueryHandler<ExportRequest, ExcelPackage>
{
    private ExcelPackage excelPackage { get; set; }

    private ExcelWorksheet reportSheet;

    public async Task<ExcelPackage> Handle(ExportRequest request, CancellationToken cancellationToken)
    {
        LoadTemplate(request.Template);
        BuildExcelBody(await GetProviderVerificationModelFromCache(request.SessionId));
        ClearTemplate();
        return excelPackage;
    }

    public async Task<IList<ProviderVerificationModel>> GetProviderVerificationModelFromCache(string sessionId)
    {
        var (found, providerVerificationModel) = await cache.TryGetAsync<IList<ProviderVerificationModel>>(Utils.BuildProviderVerificationKey(sessionId));
        return found ? providerVerificationModel : new List<ProviderVerificationModel>();
    }

    public void LoadTemplate(string template)
    {
        var templatePath = @$"{configuration.CurrentDirectory}{Path.DirectorySeparatorChar}App_Data{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}{template}";

        using var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
        excelPackage = new ExcelPackage(stream);
        reportSheet = excelPackage.Workbook.Worksheets.Copy(Const.Const.TEMPLATE_NAME, Const.Const.SHEET_NAME);
    }

    public void BuildExcelBody(IList<ProviderVerificationModel> model)
    {
        reportSheet.Cells["A2"].LoadFromCollection(model);
    }

    public void ClearTemplate()
    {
        excelPackage.Workbook.Worksheets.Delete(Const.Const.TEMPLATE_NAME);
    }
}
