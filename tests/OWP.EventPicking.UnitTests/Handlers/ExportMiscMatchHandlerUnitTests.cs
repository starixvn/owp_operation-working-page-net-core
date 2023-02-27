using NSubstitute;
using OfficeOpenXml;
using OWP.EventPicking.Handlers;
using OWP.EventPicking.Models;
using OWP.EventPicking.Services;
using OWP.Services;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.EventPicking.UnitTests.Handlers;

public class ExportMiscMatchHandlerUnitTests
{
    private readonly BuildMainAndChildMatches buildMainAndChildMatches;

    private readonly ConfigurationService configurationService;

    private readonly ExportMiscMatchHandler service;

    public ExportMiscMatchHandlerUnitTests()
    {
        buildMainAndChildMatches = Substitute.For<BuildMainAndChildMatches>();
        configurationService = Substitute.For<ConfigurationService>();
        service = new ExportMiscMatchHandler(configurationService, buildMainAndChildMatches);
    }

    [Fact]
    public async Task ExportMiscMatch_ReturnValues()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        configurationService.CurrentDirectory.Returns(AppDomain.CurrentDomain.BaseDirectory);

        var request = new ExportMiscMatchRequest
        {
            Date = new DateTime(2022, 11, 07)
        };

        var matches = new List<EventPickingModel>
        {
            new(){ InfoSiteMatchId = 111, IsNoOffer = true, RowColor = "Red"},
            new(){ InfoSiteMatchId = 222, IsNoOffer = true, RowColor = "Gray" }
        };

        buildMainAndChildMatches.GetMainAndChildMatchEvent(new DateTime(2022, 11, 07)).Returns(matches);

        var actual = await service.Handle(request, CancellationToken.None);

        DeepAssert.Equal("Event Picking-2022-11-07.xlsx", actual.FileDownloadName);
    }
}
