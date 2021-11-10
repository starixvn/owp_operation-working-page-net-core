namespace OWP.OvertimeReport.Handlers
{
    using AutoMapper;
    using Constants;
    using Epplus.Mapper.Extensions;
    using MediatR;
    using Models;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;
    using OWP.Services;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class ExportReportByYearRequest : IRequest<ExcelPackage>
    {
        public string Location { get; set; } = string.Empty;

        public string Trader { get; set; } = string.Empty;

        public int Year { get; set; }
    }

    public class ExportReportByYearHandler : IRequestHandler<ExportReportByYearRequest, ExcelPackage>
    {
        private readonly IMapper mapper;

        private readonly IMediator mediator;

        private readonly ConfigurationService configurationService;

        private ExcelPackage ExcelPackage { get; set; }

        private ExcelWorksheet ReportSheet { get; set; }

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
            var result = await mediator.Send(exportRequest);

            return CreateExcelPackage(request, result.ToList());
        }

        public ExcelPackage CreateExcelPackage(ExportReportByYearRequest request, List<OvertimeByYear> model)
        {
            var templatePath = @$"App_Data{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}AnnualOvertimeReportTemplate.xlsx";
            var absoluteTemplatePath = $"{configurationService.CurrentDirectory}{Path.DirectorySeparatorChar}{templatePath}";
            var excelModel = BuildExcelModel(model);

            return CreateExcelPackage(absoluteTemplatePath, excelModel, request.Year);
        }

        private ExcelYearModel BuildExcelModel(List<OvertimeByYear> model)
        {
            return new ExcelYearModel
            {
                HourByMonth = BuildExcelDataModel(model),
                TotalHoursByMonth = BuildTotalByMonth(model)
            };
        }

        public ExcelPackage CreateExcelPackage(string templatePath, ExcelYearModel model, int year)
        {
            LoadTemplate(templatePath);
            BuildReportBody(model, year);
            ClearTemplate();

            return ExcelPackage;
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
            ReportSheet.ApplyVertical(model.HourByMonth, YearConst.StartDataRowIndex);
            BuildTotalHoursFooter(model);
        }

        private void LoadTemplate(string templatePath)
        {
            using (var stream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
            {
                ExcelPackage = new ExcelPackage(stream);
                ReportSheet = ExcelPackage.Workbook.Worksheets.Copy(ExcelConst.YearTemplateSheetName, ExcelConst.YearSheetName);
            }
        }

        private void BuildTotalHoursFooter(ExcelYearModel model)
        {
            var totalHoursRowIndex = YearConst.StartDataRowIndex + model.HourByMonth.Count();
            var traderOvertimes = model.TotalHoursByMonth.ToList();
            for (var i = 0; i < 12; i++)
            {
                var colIndex = i + ExcelConst.StartDataColIndex;
                ReportSheet.Cells[totalHoursRowIndex, colIndex].Value = FormatHour(traderOvertimes[i]);
                SetTotalStyle(totalHoursRowIndex, colIndex);
            }

            var totalHours = model.TotalHoursByMonth.Sum(x => x);
            ReportSheet.Cells[totalHoursRowIndex, YearConst.TotalColIndex].Value = FormatHour(totalHours);
            SetTotalStyle(totalHoursRowIndex, YearConst.TotalColIndex);
        }

        private void SetTotalStyle(int rowIndex, int colIndex)
        {
            SetBackgroundColor(rowIndex, colIndex, ColorConst.TotalBackground);
            ReportSheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
            ReportSheet.Cells[rowIndex, colIndex].Style.Font.Color.SetColor(ColorConst.TotalText);
            ReportSheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin, ColorConst.HeaderBorder);
            ReportSheet.Cells[rowIndex, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        }

        private void SetBackgroundColor(int rowIndex, int colIndex, Color color)
        {
            ReportSheet.Cells[rowIndex, colIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ReportSheet.Cells[rowIndex, colIndex].Style.Fill.BackgroundColor.SetColor(color);
            ReportSheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin, ColorConst.Header);
        }

        private void ClearTemplate()
        {
            ExcelPackage.Workbook.Worksheets.Delete(ExcelConst.YearTemplateSheetName);
        }

        private string FormatHour(decimal? hour)
        {
            var hourDecimal = hour ?? 0;
            return hourDecimal != 0
                ? hourDecimal.ToString("0.00", CultureInfo.InvariantCulture)
                : "-";
        }
    }
}
