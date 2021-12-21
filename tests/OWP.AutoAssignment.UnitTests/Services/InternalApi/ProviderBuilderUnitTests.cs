namespace OWP.AutoAssignment.UnitTests.Services.InternalApi
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoAssignment.Services.InternalApi;
    using Models.InternalApi;
    using NSubstitute;
    using Xunit;
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

            IList<ExportMatchModel> models = new List<ExportMatchModel>()
            {
                new (){InfoSiteMatchId = 1},new (){InfoSiteMatchId = 2},new() {InfoSiteMatchId = 3}
            };
            IList<AutoAssignmentResultModel> autoAssignModels = new List<AutoAssignmentResultModel>();
            string requestId = "ten ten";
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
                new()
                {
                    MatchId = 1,
                    OtherProvider = "TV/BGTV/RB2TV/BETTV/RTSTV",
                    Provider = "helloWOO",
                    ProviderTv = "TV"
                },
                new()
                {
                    MatchId = 2,
                    OtherProvider = "TV/BGTV/RB2TV/BET/RTSTV",
                    Provider = "hiAL",
                    ProviderTv = ""
                },
                new()
                {
                    MatchId = 3,
                    OtherProvider = "TV/BET/RTSTV",
                    Provider = "SLRAL",
                    ProviderTv = "TV"
                },

            };

        public List<ExportMatchModel> MockExpected()
            => new()
            {
                new()
                {
                    InfoSiteMatchId = 1,
                    OtherProvider = "TV/BGTV/RB2TV/BETTV/RTSTV",
                    Provider = "TV/BGTV/(WOO)",
                },
                new()
                {
                    InfoSiteMatchId = 2,
                    OtherProvider = "TV/BGTV/RB2TV/BET/RTSTV",
                    Provider = "BET/(AL)",
                },
                new()
                {
                    InfoSiteMatchId = 3,
                    OtherProvider = "TV/BET/RTSTV",
                    Provider = "TV/BET/(AL)(SLR)",
                },
            };
    }
}
