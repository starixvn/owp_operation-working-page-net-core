using System.Drawing;
using System.Globalization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OWP.Report.OvertimeReport.Constants;
using OWP.Report.OvertimeReport.Models;

namespace OWP.Report.OvertimeReport.Services;

public interface ExportReportByMonthServices
{
    Task<ExcelPackage> ExportReportByMonth(OverTimeRequest request);
}

public class ExportReportByMonthServicesImp : ExportReportByMonthServices, IDisposable
{
    private readonly ReportByMonthServices reportService;

    private ExcelPackage excelPackage;

    private ExcelWorksheet reportSheet;

    private int totalColumnIndex;

    public ExportReportByMonthServicesImp(ReportByMonthServices reportService)
    {
        this.reportService = reportService;
    }

    public async Task<ExcelPackage> ExportReportByMonth(OverTimeRequest request)
    {
        var model = await reportService.GetReportByMonth(request);

        BuildTemplate(request);
        BuildHeaderInfo(model.DateColumns.ToList());
        BuildReportBody(model);

        return excelPackage;
    }

    public void BuildTemplate(OverTimeRequest request)
    {
        var monthYearInString = new DateTimeOffset(request.Year, request.Month, 1, 0, 0, 0, TimeSpan.Zero).ToString("MMM-yyyy");
        excelPackage = new ExcelPackage();
        reportSheet = excelPackage.Workbook.Worksheets.Add(monthYearInString);
        reportSheet.View.FreezePanes(3, 4);
        reportSheet.Column(UserColConst.TraderColIndex).Width = 20;
    }

    public void BuildHeaderInfo(List<DateColumnInfo> dateColumns)
    {
        totalColumnIndex = ExcelConst.StartDataColIndex + dateColumns.Count;
        SetHeaderStyle();

        reportSheet.Cells[ExcelConst.HeaderRowIndex, UserColConst.NoColIndex].Value = UserColConst.NoColName;
        reportSheet.Cells[ExcelConst.HeaderRowIndex, UserColConst.LocationColIndex].Value = UserColConst.LocationColName;
        reportSheet.Cells[ExcelConst.HeaderRowIndex, UserColConst.TraderColIndex].Value = UserColConst.TraderColName;
        reportSheet.Cells[ExcelConst.HeaderRowIndex, totalColumnIndex].Value = UserColConst.TotalColName;

        for (var i = 0; i < dateColumns.Count; i++)
        {
            var dateColumn = dateColumns[i];
            var colIndex = i + ExcelConst.StartDataColIndex;
            reportSheet.Cells[1, colIndex].Value = dateColumn.DayOfWeek;
            reportSheet.Cells[2, colIndex].Value = dateColumn.Date;

            if (!dateColumn.IsWeekend)
            {
                continue;
            }

            SetStyleHeaderStyle(1, colIndex, ColorConst.Weekend);
            SetStyleHeaderStyle(2, colIndex, ColorConst.Weekend);
        }
    }

    public void BuildReportBody(OvertimeByMonth model)
    {
        var traderOvertimes = model.TraderOvertime.ToList();
        var dateColumns = model.DateColumns.ToList();

        for (var i = 0; i < traderOvertimes.Count; i++)
        {
            var rowIndex = ExcelConst.StartDataRowIndex + i;
            BuildUserInfo(traderOvertimes[i], rowIndex, i);
            BuildOvertimeData(traderOvertimes[i].Overtime, rowIndex, dateColumns);
        }

        var totalHoursRowIndex = ExcelConst.StartDataRowIndex + traderOvertimes.Count;
        BuildTotalHoursByDate(model.TotalHoursByDate.ToList(), totalHoursRowIndex);
        BuildTotalHours(model.TotalHours, totalHoursRowIndex);
    }

    public void BuildUserInfo(TraderOvertime traderOvertime, int rowIndex, int i)
    {
        reportSheet.Cells[rowIndex, UserColConst.NoColIndex].Value = i + 1;
        reportSheet.Cells[rowIndex, UserColConst.LocationColIndex].Value = traderOvertime.Location ?? "-";
        reportSheet.Cells[rowIndex, UserColConst.TraderColIndex].Value = traderOvertime.Trader;
        reportSheet.Cells[rowIndex, totalColumnIndex].Value = FormatHour(traderOvertime.Total);
        SetTotalStyle(rowIndex, totalColumnIndex);
    }

    public void BuildOvertimeData(Dictionary<string, OverTimeInfo> overtimes, int rowIndex, List<DateColumnInfo> dateColumns)
    {
        for (var i = 0; i < dateColumns.Count; i++)
        {
            var colIndex = i + ExcelConst.StartDataColIndex;
            var overtime = overtimes.GetValueOrDefault(dateColumns[i].Date, new OverTimeInfo());
            reportSheet.Cells[rowIndex, colIndex].Value = FormatHour(overtime.Hour);
            reportSheet.Cells[rowIndex, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            if (overtime.IsOffDay)
            {
                reportSheet.Cells[rowIndex, colIndex].Style.Font.Color.SetColor(ColorConst.OffDay);
            }
        }
    }

    private void BuildTotalHours(decimal totalHours, int footerRowIndex)
    {
        reportSheet.Cells[footerRowIndex, totalColumnIndex].Value = FormatHour(totalHours);
        SetTotalStyle(footerRowIndex, totalColumnIndex);
    }

    private void BuildTotalHoursByDate(List<decimal> totalHoursByDate, int footerRowIndex)
    {
        for (var i = 0; i < totalHoursByDate.Count; i++)
        {
            var colIndex = i + ExcelConst.StartDataColIndex;
            reportSheet.Cells[footerRowIndex, colIndex].Value = FormatHour(totalHoursByDate[i]);
            SetTotalStyle(footerRowIndex, colIndex);
        }
    }

    private void SetTotalStyle(int rowIndex, int colIndex)
    {
        SetBackgroundColor(rowIndex, colIndex, ColorConst.TotalBackground);
        reportSheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
        reportSheet.Cells[rowIndex, colIndex].Style.Font.Color.SetColor(ColorConst.TotalText);
        reportSheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin, ColorConst.HeaderBorder);
        reportSheet.Cells[rowIndex, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
    }

    private void SetHeaderStyle()
    {
        for (var i = 1; i <= totalColumnIndex; i++)
        {
            SetStyleHeaderStyle(ExcelConst.HeaderRowIndex, i, ColorConst.Header);
            SetStyleHeaderStyle(ExcelConst.DayOfWeekRowIndex, i, ColorConst.Header);
        }
    }

    private void SetStyleHeaderStyle(int rowIndex, int colIndex, Color color)
    {
        reportSheet.Cells[rowIndex, colIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
        reportSheet.Cells[rowIndex, colIndex].Style.Fill.BackgroundColor.SetColor(color);
        reportSheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
        reportSheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin, ColorConst.HeaderBorder);
        reportSheet.Cells[rowIndex, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
    }

    private void SetBackgroundColor(int rowIndex, int colIndex, Color color)
    {
        reportSheet.Cells[rowIndex, colIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
        reportSheet.Cells[rowIndex, colIndex].Style.Fill.BackgroundColor.SetColor(color);
        reportSheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin, ColorConst.Header);
    }

    private string FormatHour(decimal hour)
        => hour != 0
            ? hour.ToString("0.00", CultureInfo.InvariantCulture)
            : "-";

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