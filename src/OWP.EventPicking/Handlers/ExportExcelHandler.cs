using System.Drawing;
using Epplus.Mapper.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OWP.EventPicking.Middleware.Models;
using OWP.EventPicking.Models;
using OWP.EventPicking.Services;
using OWP.Services;

namespace OWP.EventPicking.Handlers;

public class ExportExcelRequest : EventPickingRequest, IRequest<FileResult>
{ }

public class ExportExcelHandler : IRequestHandler<ExportExcelRequest, FileResult>, IDisposable
{
    private readonly QueryEventPickingService queryEventPickingService;

    private readonly ConfigurationService configurationService;

    private ExcelPackage excelPackage;

    public ExportExcelHandler(QueryEventPickingService queryEventPickingService, ConfigurationService configurationService)
    {
        this.queryEventPickingService = queryEventPickingService;
        this.configurationService = configurationService;
    }

    public async Task<FileResult> Handle(ExportExcelRequest request, CancellationToken cancellationToken)
    {
        var matches = await queryEventPickingService.Execute(request);

        var templatePath = Utils.GetMiscMatchTemplate(configurationService.CurrentDirectory);

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

        return fileStreamResult;
    }

    private static void FillRowColor(IList<EventPickingModel> matches, ExcelWorksheet templateSheet)
    {
        for (int i = 0, row = 2; i < matches.Count; i++, row++)
        {
            var match = matches[i];
            for (var column = 1; column <= 90; column++)
            {
                if (match.RowColorHex == null || match.RowColorHex == Misc.Constant.ColorWhiteHex)
                {
                    continue;
                }

                var colorTraderFromHex = ColorTranslator.FromHtml(match.RowColorHex);
                var cell = templateSheet.Cells[row, column];
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(colorTraderFromHex);
                templateSheet.Cells[row, 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(Misc.Constant.ColorPinkHex));
            }
        }
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