using Epplus.Mapper.Extensions;
using OfficeOpenXml.Style;
using System.Drawing;

namespace OWP.ProviderVerification.ProviderVerification.Export;

public record ExportRequest(string SessionId, string Template) : Query<ExcelPackage>;

public record ExportCheckDifferenceRequest(string SessionId, string Template) : Query<ExcelPackage>;

public class ExportHandler(ICache cache, ConfigurationService configuration) : QueryHandler<ExportRequest, ExcelPackage>
{
    private ExcelPackage ExcelPackage { get; set; }
    private ExcelWorksheet reportSheet;

    const int START_TITLE_INDEX = 1;
    const int START_ROW_INDEX = 3;
    const int FONT_SIZE_TITLE = 12;
    const int TOTAL_COLUMNS = 12;

    public async Task<ExcelPackage> Handle(ExportRequest request, CancellationToken cancellationToken)
    {
        LoadTemplate(request.Template);
        BuildExcel(await GetProviderVerificationModelFromCache(request.SessionId));
        ClearTemplate();
        return ExcelPackage;
    }

    public async Task<ExcelPackage> Handle(ExportCheckDifferenceRequest request, CancellationToken cancellationToken)
    {
        LoadTemplate(request.Template);
        BuildExcel(await GetCheckDifferenceProviderIdModelFromCache(request.SessionId));
        ClearTemplate();
        return ExcelPackage;
    }

    private async Task<IList<ProviderVerificationModel>> GetProviderVerificationModelFromCache(string sessionId)
    {
        var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
        var (found, providerVerificationModel) = await cache.TryGetAsync<string>(cacheKey);
        var providerVerificationModels = JsonConvert.DeserializeObject<IList<ProviderVerificationModel>>(providerVerificationModel);

        return found ? providerVerificationModels : new List<ProviderVerificationModel>();
    }
    private async Task<IList<ProviderVerificationModel>> GetCheckDifferenceProviderIdModelFromCache(string sessionId)
    {
        var cacheKey = Utils.BuildProviderVerificationDifferenceKey(sessionId);
        var (found, providerVerificationModel) = await cache.TryGetAsync<string>(cacheKey);
        var providerVerificationModels = JsonConvert.DeserializeObject<IList<ProviderVerificationModel>>(providerVerificationModel);

        return found ? providerVerificationModels : new List<ProviderVerificationModel>();
    }

    private void LoadTemplate(string template)
    {
        var templatePath = @$"{configuration.CurrentDirectory}{Path.DirectorySeparatorChar}App_Data{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}{template}";

        using var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
        ExcelPackage = new ExcelPackage(stream);
        reportSheet = ExcelPackage.Workbook.Worksheets.Copy(Const.Const.TEMPLATE_NAME, Const.Const.SHEET_NAME);
    }

    private void BuildExcel(IList<ProviderVerificationModel> model)
    {
        SetExcelTitle(model[0].EventDateTime);
        reportSheet.ApplyVertical(model, START_ROW_INDEX);
        SetColorRow(model);
    }

    private void SetExcelTitle(DateTime? date)
    {
        string title = "Provider Verification";
        string formattedDate = date?.ToString("MM/dd/yyyy") ?? "N/A";
        var range = reportSheet.Cells[START_TITLE_INDEX, START_TITLE_INDEX, START_TITLE_INDEX, TOTAL_COLUMNS];
        range.Value = $"{title} - {formattedDate}";
        range.Style.Font.Size = FONT_SIZE_TITLE;
    }

    private void SetColorRow(IList<ProviderVerificationModel> model)
    {
        const string BORDER_COLOR_HEX = "#c8ced3";
        Color borderColor = ColorTranslator.FromHtml(BORDER_COLOR_HEX);

        for (int i = 0; i < model.Count; i++)
        {
            int rowIndex = i + START_ROW_INDEX;
            ApplyRowStyle(rowIndex, model[i].RowColor, borderColor, TOTAL_COLUMNS);
        }
    }

    private void ApplyRowStyle(int rowIndex, string hexColor, Color borderColor, int totalColumns)
    {
        Color backgroundColor = ColorTranslator.FromHtml(hexColor);
        var range = reportSheet.Cells[rowIndex, 1, rowIndex, totalColumns];
        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        range.Style.Fill.BackgroundColor.SetColor(backgroundColor);
        ApplyBorderStyle(range, borderColor);
        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        range.Style.Font.Bold = false;
    }

    private static void ApplyBorderStyle(ExcelRange range, Color borderColor)
    {
        var border = range.Style.Border;
        border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        border.Top.Color.SetColor(borderColor);
        border.Bottom.Color.SetColor(borderColor);
        border.Left.Color.SetColor(borderColor);
        border.Right.Color.SetColor(borderColor);
    }

    private void ClearTemplate()
    {
        ExcelPackage.Workbook.Worksheets.Delete(Const.Const.TEMPLATE_NAME);
    }
}

public class ExportCheckDifferenceHandler(ICache cache, ConfigurationService configuration) : QueryHandler<ExportCheckDifferenceRequest, ExcelPackage>
{
    private ExcelPackage ExcelPackage { get; set; }
    private ExcelWorksheet reportSheet;

    const int START_TITLE_INDEX = 1;
    const int START_ROW_INDEX = 3;
    const int FONT_SIZE_TITLE = 12;
    const int TOTAL_COLUMNS = 14;

    public async Task<ExcelPackage> Handle(ExportCheckDifferenceRequest request, CancellationToken cancellationToken)
    {
        LoadTemplate(request.Template);
        BuildExcel(await GetCheckDifferenceProviderIdModelFromCache(request.SessionId));
        ClearTemplate();
        return ExcelPackage;
    }

    private async Task<IList<ProviderVerificationModel>> GetCheckDifferenceProviderIdModelFromCache(string sessionId)
    {
        var cacheKey = Utils.BuildProviderVerificationDifferenceKey(sessionId);
        var (found, providerVerificationModel) = await cache.TryGetAsync<string>(cacheKey);
        var providerVerificationModels = JsonConvert.DeserializeObject<IList<ProviderVerificationModel>>(providerVerificationModel);

        return found ? providerVerificationModels : new List<ProviderVerificationModel>();
    }

    private void LoadTemplate(string template)
    {
        var templatePath = @$"{configuration.CurrentDirectory}{Path.DirectorySeparatorChar}App_Data{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}{template}";

        using var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
        ExcelPackage = new ExcelPackage(stream);
        reportSheet = ExcelPackage.Workbook.Worksheets.Copy(Const.Const.TEMPLATE_NAME, Const.Const.SHEET_NAME);
    }

    private void BuildExcel(IList<ProviderVerificationModel> model)
    {
        SetExcelTitle(model[0].EventDateTime);
        reportSheet.ApplyVertical(model, START_ROW_INDEX);
        SetColorRow(model);
    }

    private void SetExcelTitle(DateTime? date)
    {
        string title = "Provider Verification - Check Difference ProviderID";
        string formattedDate = date?.ToString("MM/dd/yyyy") ?? "N/A";
        var range = reportSheet.Cells[START_TITLE_INDEX, START_TITLE_INDEX, START_TITLE_INDEX, TOTAL_COLUMNS];
        range.Value = $"{title} - {formattedDate}";
        range.Style.Font.Size = FONT_SIZE_TITLE;
    }

    private void SetColorRow(IList<ProviderVerificationModel> model)
    {
        const string BORDER_COLOR_HEX = "#c8ced3";
        Color borderColor = ColorTranslator.FromHtml(BORDER_COLOR_HEX);

        for (int i = 0; i < model.Count; i++)
        {
            int rowIndex = i + START_ROW_INDEX;
            ApplyRowStyle(rowIndex, model[i].RowColor, borderColor, TOTAL_COLUMNS);
        }
    }

    private void ApplyRowStyle(int rowIndex, string hexColor, Color borderColor, int totalColumns)
    {
        Color backgroundColor = ColorTranslator.FromHtml(hexColor);
        var range = reportSheet.Cells[rowIndex, 1, rowIndex, totalColumns];
        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        range.Style.Fill.BackgroundColor.SetColor(backgroundColor);
        ApplyBorderStyle(range, borderColor);
        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        range.Style.Font.Bold = false;
    }

    private static void ApplyBorderStyle(ExcelRange range, Color borderColor)
    {
        var border = range.Style.Border;
        border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        border.Top.Color.SetColor(borderColor);
        border.Bottom.Color.SetColor(borderColor);
        border.Left.Color.SetColor(borderColor);
        border.Right.Color.SetColor(borderColor);
    }

    private void ClearTemplate()
    {
        ExcelPackage.Workbook.Worksheets.Delete(Const.Const.TEMPLATE_NAME);
    }
}
