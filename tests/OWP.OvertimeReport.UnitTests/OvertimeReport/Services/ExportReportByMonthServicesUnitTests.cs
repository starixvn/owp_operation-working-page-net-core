namespace OWP.Report.UnitTests.OvertimeReport.Services;

using NSubstitute;
using OfficeOpenXml;
using Report.OvertimeReport.Models;
using Report.OvertimeReport.Services;
using Xunit;
using Xunit.Asserts.Compare;

public class ExportReportByMonthServicesUnitTests
{
    private readonly ReportByMonthServices reportService;
    private readonly ExportReportByMonthServicesImp service;

    public ExportReportByMonthServicesUnitTests()
    {
        reportService = Substitute.For<ReportByMonthServices>();

        service = new ExportReportByMonthServicesImp(reportService);
    }

    [Fact]
    public async Task ExportReportByMonth_WithCorrectModel_ReturnExcelPackageData()
    {
        reportService.GetReportByMonth(Arg.Any<OverTimeRequest>())
            .Returns(MockReportByMonthData());
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        var request = new OverTimeRequest
        {
            Location = string.Empty,
            Trader = string.Empty,
            Month = 10,
            Year = 2021
        };

        var actual = await service.ExportReportByMonth(request);
        var reportSheet = actual.Workbook.Worksheets["Oct-2021"];

        DeepAssert.Equal(reportSheet.Cells[1, 4].Value, "Fri");
        DeepAssert.Equal(reportSheet.Cells[1, 5].Value, "Sat");
        DeepAssert.Equal(reportSheet.Cells[1, 6].Value, "Sun");
        DeepAssert.Equal(reportSheet.Cells[1, 7].Value, "Mon");

        DeepAssert.Equal(reportSheet.Cells[2, 1].Value, "No");
        DeepAssert.Equal(reportSheet.Cells[2, 2].Value, "Location");
        DeepAssert.Equal(reportSheet.Cells[2, 3].Value, "Trader");
        DeepAssert.Equal(reportSheet.Cells[2, 4].Value, "01-Oct");
        DeepAssert.Equal(reportSheet.Cells[2, 5].Value, "02-Oct");
        DeepAssert.Equal(reportSheet.Cells[2, 6].Value, "03-Oct");
        DeepAssert.Equal(reportSheet.Cells[2, 7].Value, "04-Oct");
        DeepAssert.Equal(reportSheet.Cells[2, 8].Value, "Total");

        DeepAssert.Equal(reportSheet.Cells[3, 1].Value, 1);
        DeepAssert.Equal(reportSheet.Cells[3, 2].Value, "Phil");
        DeepAssert.Equal(reportSheet.Cells[3, 3].Value, "sa5");
        DeepAssert.Equal(reportSheet.Cells[3, 4].Value, "2.00");
        DeepAssert.Equal(reportSheet.Cells[3, 5].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[3, 6].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[3, 7].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[3, 8].Value, "2.00");
        DeepAssert.Equal(reportSheet.Cells[4, 1].Value, 2);
        DeepAssert.Equal(reportSheet.Cells[4, 2].Value, "SMS");
        DeepAssert.Equal(reportSheet.Cells[4, 3].Value, "sa");
        DeepAssert.Equal(reportSheet.Cells[4, 4].Value, "2.00");
        DeepAssert.Equal(reportSheet.Cells[4, 5].Value, "10.00");
        DeepAssert.Equal(reportSheet.Cells[4, 6].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[4, 7].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[4, 8].Value, "14.00");
        DeepAssert.Equal(reportSheet.Cells[5, 1].Value, null);
        DeepAssert.Equal(reportSheet.Cells[5, 2].Value, null);
        DeepAssert.Equal(reportSheet.Cells[5, 3].Value, null);
        DeepAssert.Equal(reportSheet.Cells[5, 4].Value, "-");
        DeepAssert.Equal(reportSheet.Cells[5, 5].Value, "1.00");
        DeepAssert.Equal(reportSheet.Cells[5, 6].Value, "50.00");
        DeepAssert.Equal(reportSheet.Cells[5, 7].Value, "1.00");
        DeepAssert.Equal(reportSheet.Cells[5, 8].Value, "16.00");
    }

    private OvertimeByMonth MockReportByMonthData()
        => new()
        {
            DateColumns = MockExpectDateColumnInfos(),
            TraderOvertime = MockTraderExpected(),
            TotalHoursByDate = new List<decimal> { 0, 1, 50, 1 }
        };

    private IEnumerable<DateColumnInfo> MockExpectDateColumnInfos()
        => new List<DateColumnInfo>
        {
            new() { Date = "01-Oct", DayOfWeek = "Fri", IsWeekend = false },
            new() { Date = "02-Oct", DayOfWeek = "Sat", IsWeekend = true },
            new() { Date = "03-Oct", DayOfWeek = "Sun", IsWeekend = true },
            new() { Date = "04-Oct", DayOfWeek = "Mon", IsWeekend = false }
        };

    private IEnumerable<TraderOvertime> MockTraderExpected()
        => new List<TraderOvertime>
        {
            new()
            {
                Trader = "sa5",
                Location = "Phil",
                Overtime = new Dictionary<string, OverTimeInfo>
                {
                    { "01-Oct", new OverTimeInfo { IsOffDay = true, Hour = 2 } }
                },
                Total = 2
            },
            new()
            {
                Trader = "sa",
                Location = "SMS",
                Overtime = new Dictionary<string, OverTimeInfo>
                {
                    { "01-Oct", new OverTimeInfo { IsOffDay = true, Hour = 2 } },
                    { "02-Oct", new OverTimeInfo { IsOffDay = false, Hour = 10 } },
                    { "03-Oct", new OverTimeInfo { IsOffDay = false, Hour = 0 } }
                },
                Total = 14
            }
        };
}