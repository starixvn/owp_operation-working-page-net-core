using Epplus.Mapper.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OWP.EventPicking.Misc;
using OWP.EventPicking.Models;
using OWP.EventPicking.Services;
using OWP.Services;
using System.Drawing;

namespace OWP.EventPicking.Handlers;

public class ExportMiscMatchRequest : IRequest<FileResult>
{
    public DateTime Date { get; set; }
}

public class ExportMiscMatchHandler : IRequestHandler<ExportMiscMatchRequest, FileResult>, IDisposable
{
    private readonly ConfigurationService configurationService;

    private readonly BuildMainAndChildMatches buildMainAndChildMatches;

    private ExcelPackage excelPackage;

    public ExportMiscMatchHandler(ConfigurationService configurationService, BuildMainAndChildMatches buildMainAndChildMatches)
    {
        this.configurationService = configurationService;
        this.buildMainAndChildMatches = buildMainAndChildMatches;
    }

    public async Task<FileResult> Handle(ExportMiscMatchRequest request, CancellationToken cancellationToken)
    {
        var matches = await buildMainAndChildMatches.GetMainAndChildMatchEvent(request.Date);

        var templatePath = GetReportTemplate();

        using var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);

        excelPackage = new ExcelPackage(stream);
        var templateSheet = excelPackage.Workbook.Worksheets.First();
        templateSheet.ApplyVertical(matches, 2);
        FillRowColor(matches, templateSheet);

        var fileStream = new MemoryStream();
        excelPackage.SaveAs(fileStream);
        fileStream.Position = 0;
        const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        var fileStreamResult = new FileStreamResult(fileStream, contentType);
        fileStreamResult.FileDownloadName = $"Event Picking-{request.Date.ToString("yyyy-MM-dd")}.xlsx";

        return fileStreamResult;
    }

    private static void FillRowColor(IList<EventPickingModel> matches, ExcelWorksheet templateSheet)
    {
        for (int i = 0, row = 2; i < matches.Count; i++, row++)
        {
            var match = matches[i];
            for (int column = 1; column <= 90; column++)
            {
                if (match.RowColorHex != null && match.RowColorHex != Constant.ColorWhiteHex)
                {
                    var colorTraderFromHex = ColorTranslator.FromHtml(match.RowColorHex);
                    templateSheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    templateSheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(colorTraderFromHex);
                }
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private string GetReportTemplate()
    {
        const string templatePath = @$"App_Data/Templates/EventPickingNewFormat.xlsx";
        return $"{configurationService.CurrentDirectory}{Path.DirectorySeparatorChar}{templatePath}";
    }

    protected virtual void Dispose(bool isDisposing)
    {
        excelPackage?.Dispose();
    }
}