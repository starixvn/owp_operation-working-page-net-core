namespace OWP.Report.PreMatchReport.UnitTests.Services;

using Models;
using NSubstitute;
using OfficeOpenXml;
using OWP.Services;
using PreMatchReport.Services;
using Xunit;

public class PreMatchReportExportServiceUnitTests
{
    private readonly ConfigurationService configurationService;
    private readonly GetPreMatchReportService preMatchReportService;

    public PreMatchReportExportServiceUnitTests()
    {
        preMatchReportService = Substitute.For<GetPreMatchReportService>();
        configurationService = Substitute.For<ConfigurationService>();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    [Fact]
    public async Task Export_Return_FileExport()
    {
        configurationService.CurrentDirectory.Returns(AppDomain.CurrentDomain.BaseDirectory);

        var query = new QueryModel
        {
            FromDate = DateTime.Now,
            ToDate = DateTime.Now.AddDays(1)
        };

        preMatchReportService.GetReports(query).Returns(mockExportPreMatchModels);

        var service = new PreMatchReportExportServiceImp(configurationService, preMatchReportService);

        var actual = await service.Export(query);

        Assert.NotNull(actual);
    }

    private List<ExportPreMatchModel> mockExportPreMatchModels => new()
    {
        new ExportPreMatchModel
        {
            TraderName = "ABC",
            TotalMatches = "1",
            TotalLeague = "2",
            LeagueHdpM1 = "1"
        }
    };
}