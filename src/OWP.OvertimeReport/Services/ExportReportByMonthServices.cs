namespace OWP.OvertimeReport.Services
{
    using Constants;
    using Extensions;
    using Models;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ExportReportByMonthServices
    {
        Task<ExcelPackage> ExportReportByMonth(OverTimeByMonthRequest request);
    }

    public class ExportReportByMonthServicesImp : ExportReportByMonthServices
    {
        private readonly ReportByMonthServices reportService;

        private ExcelPackage ExcelPackage { get; set; }

        private ExcelWorksheet ReportSheet { get; set; }

        private int TotalColIndex { get; set; }

        private int DateColumnNumber { get; set; }

        public ExportReportByMonthServicesImp(ReportByMonthServices reportService)
        {
            this.reportService = reportService;
        }

        public async Task<ExcelPackage> ExportReportByMonth(OverTimeByMonthRequest request)
        {
            var model = await reportService.GetReportByMonth(request);

            BuildTemplate(request);
            BuildHeaderInfo(model.DateColumns.ToList());
            BuildReportBody(model);

            return ExcelPackage;
        }

        public void BuildTemplate(OverTimeByMonthRequest request)
        {
            var monthYearInString = new DateTime(request.Year, request.Month, 1).ToString("MMM-yyyy");
            ExcelPackage = new ExcelPackage();
            ReportSheet = ExcelPackage.Workbook.Worksheets.Add(monthYearInString);
            ReportSheet.View.FreezePanes(3, 4);
            ReportSheet.Column(UserColConst.TraderColIndex).Width = 20;
        }

        public void BuildHeaderInfo(List<DateColumnInfo> dateColumns)
        {
            DateColumnNumber = dateColumns.Count;
            TotalColIndex = ExcelConst.StartDataColIndex + DateColumnNumber;
            SetHeaderStyle();

            ReportSheet.Cells[ExcelConst.HeaderRowIndex, UserColConst.NoColIndex].Value = UserColConst.NoColName;
            ReportSheet.Cells[ExcelConst.HeaderRowIndex, UserColConst.LocationColIndex].Value = UserColConst.LocationColName;
            ReportSheet.Cells[ExcelConst.HeaderRowIndex, UserColConst.TraderColIndex].Value = UserColConst.TraderColName;
            ReportSheet.Cells[ExcelConst.HeaderRowIndex, TotalColIndex].Value = UserColConst.TotalColName;

            for (var i = 0; i < DateColumnNumber; i++)
            {
                var dateColumn = dateColumns[i];
                var colIndex = i + ExcelConst.StartDataColIndex;
                ReportSheet.Cells[1, colIndex].Value = dateColumn.DayOfWeek;
                ReportSheet.Cells[2, colIndex].Value = dateColumn.Date;

                if (dateColumn.IsWeekend)
                {
                    SetStyleHeaderStyle(1, colIndex, ColorConst.Weekend);
                    SetStyleHeaderStyle(2, colIndex, ColorConst.Weekend);
                }
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
            ReportSheet.Cells[rowIndex, UserColConst.NoColIndex].Value = i + 1;
            ReportSheet.Cells[rowIndex, UserColConst.LocationColIndex].Value = traderOvertime.Location;
            ReportSheet.Cells[rowIndex, UserColConst.TraderColIndex].Value = traderOvertime.Trader;
            ReportSheet.Cells[rowIndex, TotalColIndex].Value = FormatHour(traderOvertime.Total);
            SetTotalStyle(rowIndex, TotalColIndex);
        }

        public void BuildOvertimeData(Dictionary<string, OverTimeInfo> overtimes, int rowIndex, List<DateColumnInfo> dateColumns)
        {
            for (var i = 0; i < dateColumns.Count; i++)
            {
                var colIndex = i + ExcelConst.StartDataColIndex;
                var overtime = overtimes.GetValue(dateColumns[i].Date, new OverTimeInfo());
                ReportSheet.Cells[rowIndex, colIndex].Value = FormatHour(overtime.Hour);
                ReportSheet.Cells[rowIndex, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                if (overtime.IsOffDay)
                {
                    SetBackgroundColor(rowIndex, colIndex, ColorConst.OffDay);
                }
            }
        }

        private void BuildTotalHours(decimal totalHours, int footerRowIndex)
        {
            ReportSheet.Cells[footerRowIndex, TotalColIndex].Value = FormatHour(totalHours);
            SetTotalStyle(footerRowIndex, TotalColIndex);
        }

        private void BuildTotalHoursByDate(List<decimal> totalHoursByDate, int footerRowIndex)
        {
            for (var i = 0; i < totalHoursByDate.Count; i++)
            {
                var colIndex = i + ExcelConst.StartDataColIndex;
                ReportSheet.Cells[footerRowIndex, colIndex].Value = FormatHour(totalHoursByDate[i]);
                SetTotalStyle(footerRowIndex, colIndex);
            }
        }

        private void SetTotalStyle(int rowIndex, int colIndex)
        {
            SetBackgroundColor(rowIndex, colIndex, ColorConst.TotalBackground);
            ReportSheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
            ReportSheet.Cells[rowIndex, colIndex].Style.Font.Color.SetColor(ColorConst.TotalText);
            ReportSheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin, ColorConst.HeaderBorder);
            ReportSheet.Cells[rowIndex, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        }

        private void SetHeaderStyle()
        {
            for (var i = 1; i <= TotalColIndex; i++)
            {
                SetStyleHeaderStyle(ExcelConst.HeaderRowIndex, i, ColorConst.Header);
                SetStyleHeaderStyle(ExcelConst.DayOfWeekRowIndex, i, ColorConst.Header);
            }
        }


        private void SetStyleHeaderStyle(int rowIndex, int colIndex, Color color)
        {
            ReportSheet.Cells[rowIndex, colIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ReportSheet.Cells[rowIndex, colIndex].Style.Fill.BackgroundColor.SetColor(color);
            ReportSheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
            ReportSheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin, ColorConst.HeaderBorder);
            ReportSheet.Cells[rowIndex, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        private void SetBackgroundColor(int rowIndex, int colIndex, Color color)
        {
            ReportSheet.Cells[rowIndex, colIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ReportSheet.Cells[rowIndex, colIndex].Style.Fill.BackgroundColor.SetColor(color);
            ReportSheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin, ColorConst.Header);
        }

        private string FormatHour(decimal hour)
            => hour != 0
                ? hour.ToString("0.00", CultureInfo.InvariantCulture)
                : "-";
    }
}
