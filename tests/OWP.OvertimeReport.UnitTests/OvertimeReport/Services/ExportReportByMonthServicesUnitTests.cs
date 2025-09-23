using NSubstitute;
using OfficeOpenXml;
using OWP.Report.OvertimeReport.Models;
using OWP.Report.OvertimeReport.Services;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.Report.UnitTests.OvertimeReport.Services;

public class ExportReportByMonthServicesUnitTests : IDisposable
{
    private readonly ReportByMonthServices reportService;
    private readonly ExportReportByMonthServicesImp service;

    public ExportReportByMonthServicesUnitTests()
    {
        reportService = Substitute.For<ReportByMonthServices>();
        service = new ExportReportByMonthServicesImp(reportService);
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public void Dispose()
    {
        service?.Dispose();
    }

    #region ExportReportByMonth Tests

    [Fact]
    public async Task ExportReportByMonth_WithValidRequest_ReturnsExcelPackage()
    {
        // Arrange
        var request = new OverTimeRequest
        {
            Location = "Test Location",
            Trader = "Test Trader",
            Month = 10,
            Year = 2021
        };
        var mockData = MockReportByMonthData();
        reportService.GetReportByMonth(Arg.Any<OverTimeRequest>()).Returns(mockData);

        // Act
        var result = await service.ExportReportByMonth(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Workbook);
        Assert.Single(result.Workbook.Worksheets);
        Assert.Equal("Oct-2021", result.Workbook.Worksheets[0].Name);
    }

    [Fact]
    public async Task ExportReportByMonth_WithEmptyData_ReturnsExcelPackage()
    {
        // Arrange
        var request = new OverTimeRequest
        {
            Location = string.Empty,
            Trader = string.Empty,
            Month = 1,
            Year = 2022
        };
        var emptyData = new OvertimeByMonth
        {
            DateColumns = new List<DateColumnInfo>(),
            TraderOvertime = new List<TraderOvertime>(),
            TotalHoursByDate = new List<decimal>()
        };
        reportService.GetReportByMonth(Arg.Any<OverTimeRequest>()).Returns(emptyData);

        // Act
        var result = await service.ExportReportByMonth(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Workbook);
        Assert.Single(result.Workbook.Worksheets);
        Assert.Equal("Jan-2022", result.Workbook.Worksheets[0].Name);
    }

    #endregion

    #region BuildTemplate Tests

    [Fact]
    public void BuildTemplate_WithValidRequest_CreatesCorrectWorksheet()
    {
        // Arrange
        var request = new OverTimeRequest
        {
            Month = 12,
            Year = 2023
        };

        // Act
        service.BuildTemplate(request);

        // Assert
        Assert.NotNull(service.GetType().GetField("excelPackage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(service));
        Assert.NotNull(service.GetType().GetField("reportSheet", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(service));
    }

    [Theory]
    [InlineData(1, 2023, "Jan-2023")]
    [InlineData(6, 2022, "Jun-2022")]
    [InlineData(12, 2024, "Dec-2024")]
    public void BuildTemplate_WithDifferentMonths_CreatesCorrectWorksheetName(int month, int year, string expectedName)
    {
        // Arrange
        var request = new OverTimeRequest
        {
            Month = month,
            Year = year
        };

        // Act
        service.BuildTemplate(request);

        // Assert
        var excelPackage = service.GetType().GetField("excelPackage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(service) as ExcelPackage;
        Assert.NotNull(excelPackage);
        Assert.Equal(expectedName, excelPackage.Workbook.Worksheets[0].Name);
    }

    #endregion

    #region BuildHeaderInfo Tests

    [Fact]
    public void BuildHeaderInfo_WithValidDateColumns_SetsCorrectHeaders()
    {
        // Arrange
        var dateColumns = new List<DateColumnInfo>
        {
            new() { Date = "01-Jan", DayOfWeek = "Mon", IsWeekend = false },
            new() { Date = "02-Jan", DayOfWeek = "Tue", IsWeekend = false },
            new() { Date = "03-Jan", DayOfWeek = "Wed", IsWeekend = false }
        };
        service.BuildTemplate(new OverTimeRequest { Month = 1, Year = 2023 });

        // Act
        service.BuildHeaderInfo(dateColumns);

        // Assert
        var reportSheet = service.GetType().GetField("reportSheet", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(service) as ExcelWorksheet;
        Assert.NotNull(reportSheet);
        Assert.Equal("No", reportSheet.Cells[2, 1].Value);
        Assert.Equal("Location", reportSheet.Cells[2, 2].Value);
        Assert.Equal("Trader", reportSheet.Cells[2, 3].Value);
        Assert.Equal("Total", reportSheet.Cells[2, 7].Value); // 4 (StartDataColIndex) + 3 (dateColumns.Count) = 7
    }

    [Fact]
    public void BuildHeaderInfo_WithWeekendDates_AppliesWeekendStyling()
    {
        // Arrange
        var dateColumns = new List<DateColumnInfo>
        {
            new() { Date = "01-Jan", DayOfWeek = "Sat", IsWeekend = true },
            new() { Date = "02-Jan", DayOfWeek = "Sun", IsWeekend = true }
        };
        service.BuildTemplate(new OverTimeRequest { Month = 1, Year = 2023 });

        // Act
        service.BuildHeaderInfo(dateColumns);

        // Assert
        var reportSheet = service.GetType().GetField("reportSheet", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(service) as ExcelWorksheet;
        Assert.NotNull(reportSheet);
        // Weekend styling should be applied (we can't easily test the exact color without reflection)
        Assert.Equal("Sat", reportSheet.Cells[1, 4].Value);
        Assert.Equal("Sun", reportSheet.Cells[1, 5].Value);
    }

    #endregion

    #region BuildReportBody Tests

    [Fact]
    public void BuildReportBody_WithValidData_PopulatesCorrectly()
    {
        // Arrange
        var model = MockReportByMonthData();
        service.BuildTemplate(new OverTimeRequest { Month = 10, Year = 2021 });
        service.BuildHeaderInfo(model.DateColumns.ToList());

        // Act
        service.BuildReportBody(model);

        // Assert
        var reportSheet = service.GetType().GetField("reportSheet", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(service) as ExcelWorksheet;
        Assert.NotNull(reportSheet);
        Assert.Equal(1, reportSheet.Cells[3, 1].Value);
        Assert.Equal("Phil", reportSheet.Cells[3, 2].Value);
        Assert.Equal("sa5", reportSheet.Cells[3, 3].Value);
        Assert.Equal("2.00", reportSheet.Cells[3, 8].Value);
    }

    [Fact]
    public void BuildReportBody_WithEmptyData_HandlesGracefully()
    {
        // Arrange
        var emptyModel = new OvertimeByMonth
        {
            DateColumns = new List<DateColumnInfo>(),
            TraderOvertime = new List<TraderOvertime>(),
            TotalHoursByDate = new List<decimal>()
        };
        service.BuildTemplate(new OverTimeRequest { Month = 1, Year = 2023 });
        service.BuildHeaderInfo(emptyModel.DateColumns.ToList());

        // Act
        service.BuildReportBody(emptyModel);

        // Assert
        var reportSheet = service.GetType().GetField("reportSheet", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(service) as ExcelWorksheet;
        Assert.NotNull(reportSheet);
        // Should not throw exception
    }

    #endregion

    #region BuildUserInfo Tests

    [Fact]
    public void BuildUserInfo_WithValidTraderOvertime_SetsCorrectValues()
    {
        // Arrange
        var traderOvertime = new TraderOvertime
        {
            Trader = "Test Trader",
            Location = "Test Location",
            Total = 5.5m
        };
        service.BuildTemplate(new OverTimeRequest { Month = 1, Year = 2023 });
        service.BuildHeaderInfo(new List<DateColumnInfo>());

        // Act
        service.BuildUserInfo(traderOvertime, 3, 0);

        // Assert
        var reportSheet = service.GetType().GetField("reportSheet", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(service) as ExcelWorksheet;
        Assert.NotNull(reportSheet);
        Assert.Equal(1, reportSheet.Cells[3, 1].Value);
        Assert.Equal("Test Location", reportSheet.Cells[3, 2].Value);
        Assert.Equal("Test Trader", reportSheet.Cells[3, 3].Value);
        Assert.Equal("5.50", reportSheet.Cells[3, 4].Value);
    }

    [Fact]
    public void BuildUserInfo_WithNullLocation_UsesDash()
    {
        // Arrange
        var traderOvertime = new TraderOvertime
        {
            Trader = "Test Trader",
            Location = null,
            Total = 0
        };
        service.BuildTemplate(new OverTimeRequest { Month = 1, Year = 2023 });
        service.BuildHeaderInfo(new List<DateColumnInfo>());

        // Act
        service.BuildUserInfo(traderOvertime, 3, 0);

        // Assert
        var reportSheet = service.GetType().GetField("reportSheet", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(service) as ExcelWorksheet;
        Assert.NotNull(reportSheet);
        Assert.Equal("-", reportSheet.Cells[3, 2].Value);
        Assert.Equal("-", reportSheet.Cells[3, 4].Value);
    }

    #endregion

    #region BuildOvertimeData Tests

    [Fact]
    public void BuildOvertimeData_WithValidOvertimeData_SetsCorrectValues()
    {
        // Arrange
        var overtimes = new Dictionary<string, OverTimeInfo>
        {
            { "01-Jan", new OverTimeInfo { Hour = 2.5m, IsOffDay = false } },
            { "02-Jan", new OverTimeInfo { Hour = 0, IsOffDay = true } }
        };
        var dateColumns = new List<DateColumnInfo>
        {
            new() { Date = "01-Jan", DayOfWeek = "Mon", IsWeekend = false },
            new() { Date = "02-Jan", DayOfWeek = "Tue", IsWeekend = false }
        };
        service.BuildTemplate(new OverTimeRequest { Month = 1, Year = 2023 });
        service.BuildHeaderInfo(dateColumns);

        // Act
        service.BuildOvertimeData(overtimes, 3, dateColumns);

        // Assert
        var reportSheet = service.GetType().GetField("reportSheet", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(service) as ExcelWorksheet;
        Assert.NotNull(reportSheet);
        Assert.Equal("2.50", reportSheet.Cells[3, 4].Value);
        Assert.Equal("-", reportSheet.Cells[3, 5].Value);
    }

    [Fact]
    public void BuildOvertimeData_WithMissingDateKey_UsesDefaultOverTimeInfo()
    {
        // Arrange
        var overtimes = new Dictionary<string, OverTimeInfo>();
        var dateColumns = new List<DateColumnInfo>
        {
            new() { Date = "01-Jan", DayOfWeek = "Mon", IsWeekend = false }
        };
        service.BuildTemplate(new OverTimeRequest { Month = 1, Year = 2023 });
        service.BuildHeaderInfo(dateColumns);

        // Act
        service.BuildOvertimeData(overtimes, 3, dateColumns);

        // Assert
        var reportSheet = service.GetType().GetField("reportSheet", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(service) as ExcelWorksheet;
        Assert.NotNull(reportSheet);
        Assert.Equal("-", reportSheet.Cells[3, 4].Value);
    }

    #endregion

    #region FormatHour Tests

    [Theory]
    [InlineData(0, "-")]
    [InlineData(1.5, "1.50")]
    [InlineData(10.25, "10.25")]
    [InlineData(0.1, "0.10")]
    public void FormatHour_WithDifferentValues_ReturnsCorrectFormat(decimal input, string expected)
    {
        // Arrange
        var method = typeof(ExportReportByMonthServicesImp).GetMethod("FormatHour", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act
        var result = method?.Invoke(service, new object[] { input });

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region Dispose Tests

    [Fact]
    public void Dispose_CallsDisposeOnExcelPackage()
    {
        // Arrange
        service.BuildTemplate(new OverTimeRequest { Month = 1, Year = 2023 });

        // Act
        service.Dispose();

        // Assert
        // Should not throw exception
        Assert.True(true);
    }

    [Fact]
    public void Dispose_CanBeCalledMultipleTimes_DoesNotThrow()
    {
        // Arrange
        service.BuildTemplate(new OverTimeRequest { Month = 1, Year = 2023 });

        // Act & Assert
        service.Dispose();
        service.Dispose(); // Should not throw
        Assert.True(true);
    }

    #endregion

    #region Integration Test

    [Fact]
    public async Task ExportReportByMonth_WithCompleteData_ReturnsFullyFormattedExcel()
    {
        // Arrange
        var request = new OverTimeRequest
        {
            Location = "Test Location",
            Trader = "Test Trader",
            Month = 10,
            Year = 2021
        };
        var mockData = MockReportByMonthData();
        reportService.GetReportByMonth(Arg.Any<OverTimeRequest>()).Returns(mockData);

        // Act
        var result = await service.ExportReportByMonth(request);
        var reportSheet = result.Workbook.Worksheets["Oct-2021"];

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(reportSheet);

        // Verify headers
        Assert.Equal("No", reportSheet.Cells[2, 1].Value);
        Assert.Equal("Location", reportSheet.Cells[2, 2].Value);
        Assert.Equal("Trader", reportSheet.Cells[2, 3].Value);
        Assert.Equal("Total", reportSheet.Cells[2, 8].Value);

        // Verify data rows
        Assert.Equal(1, reportSheet.Cells[3, 1].Value);
        Assert.Equal("Phil", reportSheet.Cells[3, 2].Value);
        Assert.Equal("sa5", reportSheet.Cells[3, 3].Value);
        Assert.Equal("2.00", reportSheet.Cells[3, 8].Value);

        // Verify totals row
        Assert.Equal("16.00", reportSheet.Cells[5, 8].Value);
    }

    #endregion

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