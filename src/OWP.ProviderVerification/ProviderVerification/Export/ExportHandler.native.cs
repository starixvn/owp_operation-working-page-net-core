using Epplus.Mapper.Extensions;
using OfficeOpenXml.Style;
using System.Drawing;
using NPOI.OOXML.XSSF.UserModel;

namespace OWP.ProviderVerification.ProviderVerification.Export;

public record ExportRequest(string SessionId, string Template) : Query<ExcelPackage>;

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

    private async Task<IList<ProviderVerificationModel>> GetProviderVerificationModelFromCache(string sessionId)
    {
        var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
        var (found, providerVerificationModel) = await cache.TryGetAsync<string>(cacheKey);
        var providerVerificationModels = JsonConvert.DeserializeObject<IList<ProviderVerificationModel>>(providerVerificationModel);

        return found ? providerVerificationModels : new List<ProviderVerificationModel>();
    }

    private void LoadTemplate(string template)
    {
        var templatePath = @$"{configuration.CurrentDirectory}{Path.DirectorySeparatorChar}App_Data{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}{template}";

        using var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
        excelPackage = new ExcelPackage(stream);
        reportSheet = excelPackage.Workbook.Worksheets.Copy(Const.Const.TEMPLATE_NAME, Const.Const.SHEET_NAME);
    }

    private void BuildExcelBody(IList<ProviderVerificationModel> model)
    {
        reportSheet.ApplyVertical(model, 2);
        SetColorRow(model);
    }
    private void SetColorRow(IList<ProviderVerificationModel> model)
    {
        const int START_ROW_INDEX = 2;
        string defaultColor = "#FFF";

        for (var index = START_ROW_INDEX; index < model.Count + START_ROW_INDEX; index++)
        {
            var hexColor = model[index - START_ROW_INDEX].RowColor;
            string colorLineHex = "#c8ced3";
            Color colorLine = ColorTranslator.FromHtml(colorLineHex);

            if (hexColor != defaultColor)
            {
                Color backgroundColor = ColorTranslator.FromHtml(hexColor);
                var range = reportSheet.Cells[index, 1, index, 12];
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(backgroundColor);
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Top.Color.SetColor(colorLine);
                range.Style.Border.Bottom.Color.SetColor(colorLine);
                range.Style.Border.Left.Color.SetColor(colorLine);
                range.Style.Border.Right.Color.SetColor(colorLine);
            }
        }
    }

    private void ClearTemplate()
    {
        excelPackage.Workbook.Worksheets.Delete(Const.Const.TEMPLATE_NAME);
    }
}
