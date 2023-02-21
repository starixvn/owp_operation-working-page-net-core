using NSubstitute;
using OWP.AutoAssignment.Criteria.InternalApi;
using OWP.AutoAssignment.Models.InternalApi;
using OWP.AutoAssignment.Services.InternalApi;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.AutoAssignment.UnitTests.Services.InternalApi;

public class LABuilderServiceUnitTests
{
    private readonly Repository repository;

    public LABuilderServiceUnitTests()
    {
        repository = Substitute.For<Repository>();
    }

    [Fact]
    public async Task BuildLaInfo_CompareWith_Model()
    {
        repository.GetLAExportInfo(Arg.Compat.Any<GetLAExportInfoCriteria>()).Returns(MockLaExportInfos());

        var models = new List<ExportMatchModel>
        {
            new ()
            {
                InfoSiteMatchId = 100,
                Bettype = 0,
                LA = null,
                Trader1HHDP = "Ti",
                Trader1HOU = "Ti",
                TraderHDP = "Ti",
                TraderOU = "Ti"
            },
            new ()
            {
                InfoSiteMatchId = 101,
                Bettype = -3,
                LA = null,
                Trader1HHDP = "Ti",
                Trader1HOU = "Ti",
                TraderHDP = "Ti",
                TraderOU = "Ti"
            }
        };

        var autoAssignment = MockAssignmentResultModels();

        var service = new LABuilderServiceImp(repository);

        await service.Build(models, autoAssignment);

        var expected = ExpectedModel();

        DeepAssert.Equal(expected, models);
    }

    public List<ExportMatchModel> ExpectedModel()
        => new()
        {
            new ExportMatchModel
            {
                InfoSiteMatchId = 100,
                Bettype = 0,
                LA = "Teo",
                Trader1HHDP = "Ti",
                Trader1HOU = "Ti",
                TraderHDP = "Ti",
                TraderOU = "Ti"
            },
            new ExportMatchModel
            {
                InfoSiteMatchId = 101,
                Bettype = -3,
                LA = "Teo",
                Trader1HHDP = "Ti",
                Trader1HOU = "Ti",
                TraderHDP = "Ti",
                TraderOU = "Ti"
            }
        };

    public List<AutoAssignmentResultModel> MockAssignmentResultModels()
        => new()
        {
            new AutoAssignmentResultModel
            {
                Bettype = 0,
                InfoSiteMatchId = 100,
                RequestId = "abc-xyz",
                UserId_LA = 123,
                UserId_1HHDP = 456,
                UserId_1HOU = 456,
                UserId_FTHDP = 456,
                UserId_FTOU = 456
            },
            new AutoAssignmentResultModel
            {
                Bettype = -3,
                InfoSiteMatchId = 101,
                RequestId = "abc-xyz",
                UserId_LA = 123,
                UserId_1HHDP = 456,
                UserId_1HOU = 456,
                UserId_FTHDP = 456,
                UserId_FTOU = 456
            }
        };

    public List<LAExportInfo> MockLaExportInfos()
        => new()
        {
            new LAExportInfo
            {
                InfoSiteMatchId = 100,
                Bettype = 0,
                UserName = "Teo"
            },
            new LAExportInfo
            {
                InfoSiteMatchId = 101,
                Bettype = -3,
                UserName = "Teo"
            }
        };
}