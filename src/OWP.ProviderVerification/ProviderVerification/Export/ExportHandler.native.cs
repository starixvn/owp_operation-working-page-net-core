using Epplus.Mapper.Extensions;
using NPOI.SS.Formula.Functions;
using OfficeOpenXml.Style;
using System.Drawing;

namespace OWP.ProviderVerification.ProviderVerification.Export;

public record ExportRequest(DateTime StartDate, DateTime EndDate, bool OnlyShowIsMismatchEvent, string ConfirmProvider, string LstBookProvider, string Template) : Query<ExcelPackage>;

public record ExportCheckDifferenceRequest(DateTime EventDate, string SessionId, string Template) : Query<ExcelPackage>;

public class ExportHandler( ConfigurationService configuration, ProviderVerificationRepository repository) : QueryHandler<ExportRequest, ExcelPackage>
{
    private ExcelPackage ExcelPackage { get; set; }
    private ExcelWorksheet reportSheet;

    private const int START_TITLE_INDEX = 1;
    private const int START_ROW_INDEX = 3;
    private const int FONT_SIZE_TITLE = 12;
    private const int TOTAL_COLUMNS = 12;

    public async Task<ExcelPackage> Handle(ExportRequest request, CancellationToken cancellationToken)
    {
        LoadTemplate(request.Template);
        await BuildExcel(request);
        ClearTemplate();
        return ExcelPackage;
    }

    private async Task<IList<ProviderVerificationModel>> GetProviderVerification(ExportRequest request)
    {
        var criteria = new GetProviderVerificationCriteria
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            OnlyShowIsMismatchEvent = request.OnlyShowIsMismatchEvent,
            ConfirmProvider = request.ConfirmProvider,
            LSTBookProvider = request.LstBookProvider
        };

        var providerVerification = await repository.GetProviderVerification(criteria);

        return providerVerification
            .OrderBy(pv => pv.EventDateTime)
            .ThenBy(pv => pv.MatchNo)
            .ToList();
    }


    private void LoadTemplate(string template)
    {
        var templatePath = @$"{configuration.CurrentDirectory}{Path.DirectorySeparatorChar}App_Data{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}{template}";

        using var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
        ExcelPackage = new ExcelPackage(stream);
        reportSheet = ExcelPackage.Workbook.Worksheets.Copy(Const.Const.TEMPLATE_NAME, Const.Const.SHEET_NAME);
    }

    private async Task BuildExcel(ExportRequest request)
    {
        var data = await GetProviderVerification(request);
        SetExcelTitle(request.StartDate, request.EndDate);
        reportSheet.ApplyVertical(data, START_ROW_INDEX);
        SetColorRow(data);
    }

    private void SetExcelTitle(DateTime? startDate, DateTime? endDate)
    {
        const string title = "Provider Verification";
        if (!startDate.HasValue || !endDate.HasValue)
        {
            throw new ArgumentException($"Both {nameof(startDate)} and {nameof(endDate)} must have values.");
        }

        var formattedDate = startDate.Value.Date == endDate.Value.Date ? startDate.Value.ToString("MM/dd/yyyy") : $"From {startDate.Value:MM/dd/yyyy} To {endDate.Value:MM/dd/yyyy}";

        var range = reportSheet.Cells[START_TITLE_INDEX, START_TITLE_INDEX, START_TITLE_INDEX, TOTAL_COLUMNS];
        range.Value = $"{title} - {formattedDate}";
        range.Style.Font.Size = FONT_SIZE_TITLE;
    }

    private void SetColorRow(IList<ProviderVerificationModel> model)
    {
        const string BORDER_COLOR_HEX = "#c8ced3";
        var borderColor = ColorTranslator.FromHtml(BORDER_COLOR_HEX);

        for (var i = 0; i < model.Count; i++)
        {
            var rowIndex = i + START_ROW_INDEX;
            ApplyRowStyle(rowIndex, model[i].RowColor, borderColor, TOTAL_COLUMNS);
        }
    }

    private void ApplyRowStyle(int rowIndex, string hexColor, Color borderColor, int totalColumns)
    {
        var backgroundColor = ColorTranslator.FromHtml(hexColor);
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

public class ExportCheckDifferenceHandler(ICache cache, ConfigurationService configuration, ProviderVerificationRepository repository) : QueryHandler<ExportCheckDifferenceRequest, ExcelPackage>
{
    private ExcelPackage ExcelPackage { get; set; }
    private ExcelWorksheet reportSheet;

    private const int START_TITLE_INDEX = 1;
    private const int START_ROW_INDEX = 3;
    private const int FONT_SIZE_TITLE = 12;
    private const int TOTAL_COLUMNS = 14;

    public async Task<ExcelPackage> Handle(ExportCheckDifferenceRequest request, CancellationToken cancellationToken)
    {
        LoadTemplate(request.Template);
        BuildExcel(await GetProviderVerificationDiff(request));
        ClearTemplate();
        return ExcelPackage;
    }

    private async Task<IList<ProviderVerificationDifferenceModel>> GetProviderVerificationDiff(ExportCheckDifferenceRequest request)
    {
        var criteria = new GetProviderVerificationCriteria
        {
            StartDate = request.EventDate,
            EndDate = request.EventDate,
            OnlyShowIsMismatchEvent = true,
            ConfirmProvider = null,
            LSTBookProvider = null
        };

        var providerVerificationDiff = await repository.GetProviderVerificationDiff(criteria);

        var cacheKey = Utils.BuildFileAgentUnmapKey(request.SessionId);
        var (_, excelProviderModel) = await cache.TryGetAsync<string>(cacheKey);
        var excelProviderModels = JsonConvert.DeserializeObject<IList<ExcelProviderModel>>(excelProviderModel);
        var unMapInExcel = excelProviderModels
            .Select(e => new ProviderVerificationDifferenceModel
            {
                OWPMatchId = null,
                MatchNo = string.Empty,
                EventDateTime = null,
                KoTime = string.Empty,
                League = string.Empty,
                Home = string.Empty,
                Away = string.Empty,
                LstBookProvider = string.Empty,
                InHouseProviderId = string.Empty,
                Provider = e.Provider,
                ProviderMatchId = e.ProviderMatchID
            });

        var mergedList = providerVerificationDiff.Concat(unMapInExcel).ToList();

        return mergedList;
    }

    private void LoadTemplate(string template)
    {
        var templatePath = @$"{configuration.CurrentDirectory}{Path.DirectorySeparatorChar}App_Data{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}{template}";

        using var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
        ExcelPackage = new ExcelPackage(stream);
        reportSheet = ExcelPackage.Workbook.Worksheets.Copy(Const.Const.TEMPLATE_NAME, Const.Const.SHEET_NAME);
    }

    private void BuildExcel(IList<ProviderVerificationDifferenceModel> model)
    {
        SetExcelTitle(model[0].EventDateTime);
        reportSheet.ApplyVertical(model, START_ROW_INDEX);
        SetColorRow(model);
    }

    private void SetExcelTitle(DateTime? date)
    {
        const string title = "Provider Verification - Check Difference ProviderId";
        var formattedDate = date?.ToString("MM/dd/yyyy") ?? "N/A";
        var range = reportSheet.Cells[START_TITLE_INDEX, START_TITLE_INDEX, START_TITLE_INDEX, TOTAL_COLUMNS];
        range.Value = $"{title} - {formattedDate}";
        range.Style.Font.Size = FONT_SIZE_TITLE;
    }

    private void SetColorRow(IList<ProviderVerificationDifferenceModel> model)
    {
        const string BORDER_COLOR_HEX = "#c8ced3";
        var borderColor = ColorTranslator.FromHtml(BORDER_COLOR_HEX);

        for (var i = 0; i < model.Count; i++)
        {
            var rowIndex = i + START_ROW_INDEX;
            ApplyRowStyle(rowIndex, model[i].RowColor, borderColor, TOTAL_COLUMNS);
        }
    }

    private void ApplyRowStyle(int rowIndex, string hexColor, Color borderColor, int totalColumns)
    {
        var backgroundColor = ColorTranslator.FromHtml(hexColor);
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