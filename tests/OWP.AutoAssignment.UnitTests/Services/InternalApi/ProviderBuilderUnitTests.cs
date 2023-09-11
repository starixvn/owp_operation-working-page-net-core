using NSubstitute;
using OWP.AutoAssignment.Models.InternalApi;
using OWP.AutoAssignment.Services.InternalApi;
using Xunit;

namespace OWP.AutoAssignment.UnitTests.Services.InternalApi;

using Xunit.Asserts.Compare;

public class ProviderBuilderUnitTests
{
    private readonly Repository repository;

    public ProviderBuilderUnitTests()
    {
        repository = Substitute.For<Repository>();
    }

    [Fact]
    public async Task Build_Always_ReturnListRealMatchInfo()
    {
        // Arrange
        repository.GetProviderInfo(Arg.Compat.Any<List<int>>(), Arg.Compat.Any<string>())
            .Returns(MockListProviderInfo());

        IList<ExportMatchModel> models = new List<ExportMatchModel>
        {
            new (){InfoSiteMatchId = 1},new (){InfoSiteMatchId = 2},new() {InfoSiteMatchId = 3}
        };
        IList<AutoAssignmentResultModel> autoAssignModels = new List<AutoAssignmentResultModel>
        {
            new (){InfoSiteMatchId = 1,IsSaba = false,IsFutsal = true,PcCode = "code1"},
            new (){InfoSiteMatchId = 2,IsSaba = false,IsFutsal = false,PcCode = "code2"},
            new() {InfoSiteMatchId = 3, IsSaba = false,IsFutsal = false,PcCode = "code3"}
        };

        const string requestId = "ten ten";
        var expected = MockExpected();
        var service = new ProviderBuilderServiceImp(repository);

        // Act
        await service.Build(models, autoAssignModels, requestId);

        // Assert
        DeepAssert.Equal(models, expected);
    }

    public List<ProviderInfo> MockListProviderInfo()
        => new()
        {
            new ProviderInfo
            {
                MatchId = 1,
                OtherProvider = "TV/BGTV/RB2TV/BETTV/RTSTV",
                Provider = "helloWOO",
                ProviderTv = "TV"
            },
            new ProviderInfo
            {
                MatchId = 2,
                OtherProvider = "TV/BGTV/RB2TV/BET/RTSTV",
                Provider = "hiAL",
                ProviderTv = ""
            },
            new ProviderInfo
            {
                MatchId = 3,
                OtherProvider = "TV/BET/RTSTV",
                Provider = "SLRAL",
                ProviderTv = "TV"
            }
        };

    public List<ExportMatchModel> MockExpected()
        => new()
        {
            new ExportMatchModel
            {
                InfoSiteMatchId = 1,
                OtherProvider = "TV/BGTV/RB2TV/BETTV/RTSTV",
                Provider = "TV/BGTV/(WOO)/code1"
            },
            new ExportMatchModel
            {
                InfoSiteMatchId = 2,
                OtherProvider = "TV/BGTV/RB2TV/BET/RTSTV",
                Provider = "BET/(AL)"
            },
            new ExportMatchModel
            {
                InfoSiteMatchId = 3,
                OtherProvider = "TV/BET/RTSTV",
                Provider = "TV/BET/(AL)(SLR)"
            }
        };
}