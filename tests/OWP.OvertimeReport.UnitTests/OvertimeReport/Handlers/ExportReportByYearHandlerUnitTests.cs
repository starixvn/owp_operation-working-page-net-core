namespace OWP.Report.UnitTests.OvertimeReport.Handlers;

using AutoMapper;
using MediatR;
using NSubstitute;
using OfficeOpenXml;
using OWP.Services;
using Report.OvertimeReport.Constants;
using Report.OvertimeReport.Handlers;
using Report.OvertimeReport.Models;
using Xunit;
using Xunit.Asserts.Compare;

public class ExportReportByYearHandlerUnitTests
{
    private readonly ExportReportByYearHandler exportExcel;
    private readonly ConfigurationService configurationService;
    private readonly IMediator mediator;
    private ExcelWorksheet reportSheet;

    public ExportReportByYearHandlerUnitTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ExportReportByYearRequest, QueryReportByYearRequest>();
        });
        var iMapper = config.CreateMapper();
        mediator = Substitute.For<IMediator>();
        configurationService = Substitute.For<ConfigurationService>();

        exportExcel = new ExportReportByYearHandler(iMapper, configurationService, mediator);
    }

    [Fact]
    public async Task Handle_ValidModel_ReturnExcelPackage()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        configurationService.CurrentDirectory.Returns(AppDomain.CurrentDomain.BaseDirectory);

        var request = new ExportReportByYearRequest
        {
            Location = "",
            Trader = "",
            Year = 2021
        };
        mediator.Send(Arg.Any<QueryReportByYearRequest>()).Returns(MockData());

        var actual = await exportExcel.Handle(request, new CancellationToken(false));
        reportSheet = actual.Workbook.Worksheets[ExcelConst.YearSheetName];

        DeepAssert.Equal(reportSheet.Cells[1, 1].Value, "No");
        DeepAssert.Equal(reportSheet.Cells[1, 2].Value, "Location");
        DeepAssert.Equal(reportSheet.Cells[1, 3].Value, "Trader");
        DeepAssert.Equal(reportSheet.Cells[1, 4].Value, "JAN");
        DeepAssert.Equal(reportSheet.Cells[1, 5].Value, "FEB");
        DeepAssert.Equal(reportSheet.Cells[1, 6].Value, "MAR");
        DeepAssert.Equal(reportSheet.Cells[1, 7].Value, "APR");
        DeepAssert.Equal(reportSheet.Cells[1, 8].Value, "MAY");
        DeepAssert.Equal(reportSheet.Cells[1, 9].Value, "JUN");
        DeepAssert.Equal(reportSheet.Cells[1, 10].Value, "JUL");
        DeepAssert.Equal(reportSheet.Cells[1, 11].Value, "AUG");
        DeepAssert.Equal(reportSheet.Cells[1, 12].Value, "SEP");
        DeepAssert.Equal(reportSheet.Cells[1, 13].Value, "OCT");
        DeepAssert.Equal(reportSheet.Cells[1, 14].Value, "NOV");
        DeepAssert.Equal(reportSheet.Cells[1, 15].Value, "DEC");
        DeepAssert.Equal(reportSheet.Cells[1, 16].Value, "Total");
        DeepAssert.Equal(reportSheet.Cells[2, 1].Value, 1);
        DeepAssert.Equal(reportSheet.Cells[2, 2].Value, "Phil");
        DeepAssert.Equal(reportSheet.Cells[2, 3].Value, "sa5");
        DeepAssert.Equal(reportSheet.Cells[2, 4].Value, "2.00");
        DeepAssert.Equal(reportSheet.Cells[2, 5].Value, "1.00");
        DeepAssert.Equal(reportSheet.Cells[2, 6].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[2, 7].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[2, 8].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[2, 9].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[2, 10].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[2, 11].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[2, 12].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[2, 13].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[2, 14].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[2, 15].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[2, 16].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[3, 1].Value, 2);
        DeepAssert.Equal(reportSheet.Cells[3, 2].Value, "SMS");
        DeepAssert.Equal(reportSheet.Cells[3, 3].Value, "sa");
        DeepAssert.Equal(reportSheet.Cells[3, 4].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[3, 5].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[3, 6].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[3, 7].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[3, 8].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[3, 9].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[3, 10].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[3, 11].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[3, 12].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[3, 13].Value, "12.00");
        DeepAssert.Equal(reportSheet.Cells[3, 14].Value, "1.00");
        DeepAssert.Equal(reportSheet.Cells[3, 15].Value, "1.00");
        DeepAssert.Equal(reportSheet.Cells[3, 16].Value, "14.00");
        DeepAssert.Equal(reportSheet.Cells[4, 1].Value, null);
        DeepAssert.Equal(reportSheet.Cells[4, 2].Value, null);
        DeepAssert.Equal(reportSheet.Cells[4, 3].Value, null);
        DeepAssert.Equal(reportSheet.Cells[4, 4].Value, "2.00");
        DeepAssert.Equal(reportSheet.Cells[4, 5].Value, "1.00");
        DeepAssert.Equal(reportSheet.Cells[4, 6].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[4, 7].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[4, 8].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[4, 9].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[4, 10].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[4, 11].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[4, 12].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[4, 13].Value, "12.00");
        DeepAssert.Equal(reportSheet.Cells[4, 14].Value, "1.00");
        DeepAssert.Equal(reportSheet.Cells[4, 15].Value, "1.00");
        DeepAssert.Equal(reportSheet.Cells[4, 16].Value, "17.00");
    }

    private IEnumerable<OvertimeByYear> MockData()
        => new List<OvertimeByYear>
        {
            new()
            {
                Trader = "sa5",
                Location = "Phil",
                OvertimeData = new List<decimal?> { 2, 1, null, null, null, null, null, null, null, null, null, null },
                Total = null
            },
            new()
            {
                Trader = "sa",
                Location = "SMS",
                OvertimeData = new List<decimal?> { null, null, null, null, null, null, null, null, null, 12, 1, 1 },
                Total = 14
            }
        };
}