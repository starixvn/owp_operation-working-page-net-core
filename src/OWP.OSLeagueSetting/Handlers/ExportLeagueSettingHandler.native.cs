using MediatR;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OWP.OSLeagueSetting.Services;
using OWP.Services;
using System.Drawing;

namespace OWP.OSLeagueSetting.Handlers;

public class ExportLeagueSettingRequest : IRequest<ExcelPackage>
{
    public string League { get; set; }

    public string StatusId { get; set; }

    public string CategoryId { get; set; }

    public string RoundId { get; set; }
}

public class ExportLeagueSettingHandler : IRequestHandler<ExportLeagueSettingRequest, ExcelPackage>, IDisposable
{
    private ExcelPackage excelPackage;

    private ExcelWorksheet reportSheet;

    private readonly ConfigurationService configurationService;

    private readonly ExportLeagueSettingService exportLeagueSettingService;

    public ExportLeagueSettingHandler(ExportLeagueSettingService exportLeagueSettingService, ConfigurationService configurationService)
    {
        this.exportLeagueSettingService = exportLeagueSettingService;
        this.configurationService = configurationService;
    }

    public async Task<ExcelPackage> Handle(ExportLeagueSettingRequest request, CancellationToken cancellationToken)
    {
        var templatePath = GetReportTemplate();
        LoadTemplate(templatePath);
        await BuildExcelBody(request);
        return await Task.FromResult(excelPackage);
    }

    private void LoadTemplate(string templatePath)
    {
        using var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
        excelPackage = new ExcelPackage(stream);
        reportSheet = excelPackage.Workbook.Worksheets[0];
    }

    private async Task BuildExcelBody(ExportLeagueSettingRequest request)
    {
        var leagueSettings= await exportLeagueSettingService.ExportLeagueSetting(request);
        int rowIndex = 2;
        foreach (var item in leagueSettings)
        {
            reportSheet.Cells[rowIndex, 1].Value = rowIndex-1;
            reportSheet.Cells[rowIndex, 2].Value = item.Status;
            reportSheet.Cells[rowIndex, 3].RichText.Add(item.SummaryFirst); 
            reportSheet.Cells[rowIndex, 3].RichText.Add(item.SummarySecond).Color = ColorTranslator.FromHtml("#c9211e"); 
            reportSheet.Cells[rowIndex, 4].Value = item.OSLeagueName;
            reportSheet.Cells[rowIndex, 5].Value = item.Region;
            reportSheet.Cells[rowIndex, 6].Value = item.CategoryName;
            reportSheet.Cells[rowIndex, 7].Value = item.RoundName;
            reportSheet.Cells[rowIndex, 8].Value = item.OfferLines;
            reportSheet.Cells[rowIndex, 9].Value = item.BetDelays;
            reportSheet.Cells[rowIndex, 10].RichText.Add(item.Remark).Color = ColorTranslator.FromHtml("#c9211e");
            var cellRange = reportSheet.Cells[rowIndex, 1, rowIndex, 13];
            cellRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            cellRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            cellRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            cellRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            cellRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            rowIndex++;
        }
    }

    private string GetReportTemplate()
    {
        const string templatePath = @"App_Data/Templates/BasketballMaxbetSetting.xlsx";
        return $"{configurationService.CurrentDirectory}{Path.DirectorySeparatorChar}{templatePath}";
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isDisposing)
    {
        excelPackage?.Dispose();
    }
}