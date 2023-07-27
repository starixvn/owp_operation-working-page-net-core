using Epplus.Mapper.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OWP.EventPicking.Misc;
using OWP.EventPicking.Models;
using OWP.EventPicking.Services;
using OWP.Models;
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

    private readonly AccessCounter accessCounter;

    private readonly AccessCounterModel counterModel;

    private ExcelPackage excelPackage;

    public ExportMiscMatchHandler(
        ConfigurationService configurationService,
        BuildMainAndChildMatches buildMainAndChildMatches,
        AccessCounter accessCounter,
        IOptions<AccessCounterModel> counterOptions)
    {
        this.configurationService = configurationService;
        this.buildMainAndChildMatches = buildMainAndChildMatches;
        this.accessCounter = accessCounter;
        counterModel = counterOptions.Value;
    }

    public async Task<FileResult> Handle(ExportMiscMatchRequest request, CancellationToken cancellationToken)
    {
        var matches = await buildMainAndChildMatches.GetMainAndChildMatchEvent(request.Date);

        var templatePath = GetReportTemplate();

        await using var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);

        excelPackage = new ExcelPackage(stream);
        var templateSheet = excelPackage.Workbook.Worksheets.First();
        templateSheet.ApplyVertical(matches, 2);
        FillRowColor(matches, templateSheet);

        var fileStream = new MemoryStream();
        await excelPackage.SaveAsAsync(fileStream, cancellationToken);
        fileStream.Position = 0;
        const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        var fileStreamResult = new FileStreamResult(fileStream, contentType)
        {
            FileDownloadName = $"Event Picking-{request.Date:yyyy-MM-dd}.xlsx"
        };
        await accessCounter.UpdateCounter(counterModel.ExportMiscMatch);
        return fileStreamResult;
    }

    private static void FillRowColor(IList<EventPickingModel> matches, ExcelWorksheet templateSheet)
    {
        for (int i = 0, row = 2; i < matches.Count; i++, row++)
        {
            var match = matches[i];
            for (var column = 1; column <= 90; column++)
            {
                if (match.RowColorHex == null || match.RowColorHex == Constant.ColorWhiteHex)
                {
                    continue;
                }

                var colorTraderFromHex = ColorTranslator.FromHtml(match.RowColorHex);
                templateSheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                templateSheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(colorTraderFromHex);
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
        const string templatePath = @"App_Data/Templates/EventPickingNewFormat.xlsx";
        return $"{configurationService.CurrentDirectory}{Path.DirectorySeparatorChar}{templatePath}";
    }

    protected virtual void Dispose(bool isDisposing)
    {
        excelPackage?.Dispose();
    }
}