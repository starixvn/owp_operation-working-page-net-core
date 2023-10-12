using Epplus.Mapper.Extensions;
using MediatR;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OWP.Report.MatchCountLiveReport.Models;
using OWP.Services;
using System.Drawing;

// ReSharper disable InvertIf

namespace OWP.Report.MatchCountLiveReport.Handlers;

public class ExportRequest : IRequest<ExcelPackage>
{
    public IList<ReportInfo> DataSource { get; set; }

}
public class ExportHandler : IRequestHandler<ExportRequest, ExcelPackage>, IDisposable
{
    private const int START_ROW_INDEX = 3;

    private readonly Color headerSectionBackgroundColor = ColorTranslator.FromHtml("#eaf1dd");

    private readonly IList<string> headerSectionTitle = new List<string>{
        "Real Soccer",
        "SABA"
    };

    private readonly ConfigurationService configurationService;

    private ExcelPackage excelPackage;

    private ExcelWorksheet reportSheet;

    public ExportHandler(ConfigurationService configurationService)
    {
        this.configurationService = configurationService;
    }

    public async Task<ExcelPackage> Handle(ExportRequest request, CancellationToken cancellationToken)
    {
        var templatePath = GetReportTemplate();
        LoadTemplate(templatePath);
        BuildExcelBody(request);

        return await Task.FromResult(excelPackage);
    }


    private void BuildExcelBody(ExportRequest request)
    {
        reportSheet.ApplyVertical(request.DataSource, 3);
        SetStyleSectionHeader();

    }

    private void SetStyleSectionHeader()
    {
        var dimension = reportSheet.Dimension;
        var end = dimension.End;
        for (var index = START_ROW_INDEX; index < end.Row; index++)
        {
            var bettype = reportSheet.Cells[index, 2].Text;
            if (headerSectionTitle.Any(x => string.Equals(x, bettype)))
            {
                reportSheet.Row(index).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                reportSheet.Row(index).Style.Fill.PatternType = ExcelFillStyle.Solid;
                reportSheet.Row(index).Style.Font.Bold = true;
                reportSheet.Row(index).Style.Fill.BackgroundColor.SetColor(headerSectionBackgroundColor);
            }
        }
    }

    private void LoadTemplate(string templatePath)
    {
        using var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);

        excelPackage = new ExcelPackage(stream);
        reportSheet = excelPackage.Workbook.Worksheets[0];
    }

    private string GetReportTemplate()
    {
        const string templatePath = @"App_Data/Templates/MatchCountLiveReportTemplate.xlsx";
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