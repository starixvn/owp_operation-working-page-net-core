using AutoMapper;
using Epplus.Mapper.Extensions;
using MediatR;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OWP.Report.OvertimeReport.Constants;
using OWP.Report.OvertimeReport.Models;
using OWP.Services;
using System.Drawing;
using System.Globalization;

namespace OWP.Report.OvertimeReport.Handlers;

public class ExportReportByYearRequest : IRequest<ExcelPackage>
{
    public string Location { get; set; } = string.Empty;

    public string Trader { get; set; } = string.Empty;

    public int Year { get; set; }
}

public class ExportReportByYearHandler : IRequestHandler<ExportReportByYearRequest, ExcelPackage>, IDisposable
{
    private readonly IMapper mapper;

    private readonly IMediator mediator;

    private readonly ConfigurationService configurationService;

    private ExcelPackage excelPackage;

    private ExcelWorksheet reportSheet;

    public ExportReportByYearHandler(
        IMapper mapper,
        ConfigurationService configurationService,
        IMediator mediator)
    {
        this.mapper = mapper;
        this.mediator = mediator;
        this.configurationService = configurationService;
    }

    public async Task<ExcelPackage> Handle(ExportReportByYearRequest request, CancellationToken cancellationToken)
    {
        var exportRequest = mapper.Map<QueryReportByYearRequest>(request);
        var result = await mediator.Send(exportRequest, cancellationToken);

        return CreateExcelPackage(request, result.ToList());
    }

    public ExcelPackage CreateExcelPackage(ExportReportByYearRequest request, List<OvertimeByYear> model)
    {
        var templatePath = @$"App_Data{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}AnnualOvertimeReportTemplate.xlsx";
        var absoluteTemplatePath = $"{configurationService.CurrentDirectory}{Path.DirectorySeparatorChar}{templatePath}";
        var excelModel = BuildExcelModel(model);

        return CreateExcelPackage(absoluteTemplatePath, excelModel, request.Year);
    }

    public ExcelPackage CreateExcelPackage(string templatePath, ExcelYearModel model, int year)
    {
        LoadTemplate(templatePath);
        BuildReportBody(model, year);
        ClearTemplate();

        return excelPackage;
    }

    private ExcelYearModel BuildExcelModel(List<OvertimeByYear> model)
    {
        return new ExcelYearModel
        {
            HourByMonth = BuildExcelDataModel(model),
            TotalHoursByMonth = BuildTotalByMonth(model)
        };
    }

    public IEnumerable<ExcelYearData> BuildExcelDataModel(List<OvertimeByYear> traderOvertimes)
    {
        return traderOvertimes.Select((trader, i) => new ExcelYearData
        {
            No = i + 1,
            Location = trader.Location ?? "-",
            Trader = trader.Trader,
            Jan = FormatHour(trader.OvertimeData[0]),
            Feb = FormatHour(trader.OvertimeData[1]),
            Mar = FormatHour(trader.OvertimeData[2]),
            Apr = FormatHour(trader.OvertimeData[3]),
            May = FormatHour(trader.OvertimeData[4]),
            Jun = FormatHour(trader.OvertimeData[5]),
            Jul = FormatHour(trader.OvertimeData[6]),
            Aug = FormatHour(trader.OvertimeData[7]),
            Sep = FormatHour(trader.OvertimeData[8]),
            Oct = FormatHour(trader.OvertimeData[9]),
            Nov = FormatHour(trader.OvertimeData[10]),
            Dec = FormatHour(trader.OvertimeData[11]),
            Total = FormatHour(trader.Total)
        });
    }

    public IEnumerable<decimal?> BuildTotalByMonth(List<OvertimeByYear> traderOvertimes)
    {
        for (var i = 0; i < 12; i++)
        {
            yield return traderOvertimes.Sum(x => x.OvertimeData[i]);
        }
    }

    public void BuildReportBody(ExcelYearModel model, int year)
    {
        reportSheet.ApplyVertical(model.HourByMonth, YearConst.StartDataRowIndex);
        BuildTotalHoursFooter(model);
    }

    private void LoadTemplate(string templatePath)
    {
        using var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
        excelPackage = new ExcelPackage(stream);
        reportSheet = excelPackage.Workbook.Worksheets.Copy(ExcelConst.YearTemplateSheetName, ExcelConst.YearSheetName);
    }

    private void BuildTotalHoursFooter(ExcelYearModel model)
    {
        var totalHoursRowIndex = YearConst.StartDataRowIndex + model.HourByMonth.Count();
        var traderOvertimes = model.TotalHoursByMonth.ToList();
        for (var i = 0; i < 12; i++)
        {
            var colIndex = i + ExcelConst.StartDataColIndex;
            reportSheet.Cells[totalHoursRowIndex, colIndex].Value = FormatHour(traderOvertimes[i]);
            SetTotalStyle(totalHoursRowIndex, colIndex);
        }

        var totalHours = model.TotalHoursByMonth.Sum(x => x);
        reportSheet.Cells[totalHoursRowIndex, YearConst.TotalColIndex].Value = FormatHour(totalHours);
        SetTotalStyle(totalHoursRowIndex, YearConst.TotalColIndex);
    }

    private void SetTotalStyle(int rowIndex, int colIndex)
    {
        SetBackgroundColor(rowIndex, colIndex, ColorConst.TotalBackground);
        reportSheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
        reportSheet.Cells[rowIndex, colIndex].Style.Font.Color.SetColor(ColorConst.TotalText);
        reportSheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin, ColorConst.HeaderBorder);
        reportSheet.Cells[rowIndex, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
    }

    private void SetBackgroundColor(int rowIndex, int colIndex, Color color)
    {
        reportSheet.Cells[rowIndex, colIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
        reportSheet.Cells[rowIndex, colIndex].Style.Fill.BackgroundColor.SetColor(color);
        reportSheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin, ColorConst.Header);
    }

    private void ClearTemplate()
    {
        excelPackage.Workbook.Worksheets.Delete(ExcelConst.YearTemplateSheetName);
    }

    private string FormatHour(decimal? hour)
    {
        var hourDecimal = hour ?? 0;
        return hourDecimal != 0
            ? hourDecimal.ToString("0.00", CultureInfo.InvariantCulture)
            : "-";
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