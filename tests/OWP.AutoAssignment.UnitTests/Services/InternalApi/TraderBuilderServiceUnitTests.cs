namespace OWP.AutoAssignment.UnitTests.Services.InternalApi
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoAssignment.Services.InternalApi;
    using Criteria.InternalApi;
    using Models.InternalApi;
    using NSubstitute;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class TraderBuilderServiceUnitTests
    {
        private readonly Repository repository;

        public TraderBuilderServiceUnitTests()
        {
            this.repository = Substitute.For<Repository>();
        }

        [Fact]
        public async Task BuildLaInfo_CompareWith_Model()
        {
            repository.GetTraderExportInfo(Arg.Compat.Any<GetTraderExportInfoCriteria>()).Returns(MockTraderExportInfos());

            var models = new List<ExportMatchModel>()
            {
                new()
                {
                    InfoSiteMatchId = 100,
                    Bettype = 0,
                    LA = "Teo",
                    Trader1HHDP = null,
                    Trader1HOU = null,
                    TraderHDP = null,
                    TraderOU = null,
                },
                new()
                {
                    InfoSiteMatchId = 101,
                    Bettype = -3,
                    LA = "Teo",
                    Trader1HHDP = null,
                    Trader1HOU = null,
                    TraderHDP = null,
                    TraderOU = null,
                }
            };

            var autoAssignment = MockAssignmentResultModels();

            var service = new TraderBuilderServiceImp(repository);

            await service.Build(models, autoAssignment);

            var expected = ExpectedModel();

            DeepAssert.Equal(expected, models);
        }
        public List<ExportMatchModel> ExpectedModel()
            => new()
            {
                new()
                {
                    InfoSiteMatchId = 100,
                    Bettype = 0,
                    LA = "Teo",
                    Trader1HHDP = "Ti",
                    Trader1HOU = "Ti",
                    TraderHDP = "Ti",
                    TraderOU = "Ti",
                },
                new()
                {
                    InfoSiteMatchId = 101,
                    Bettype = -3,
                    LA = "Teo",
                    Trader1HHDP = "Ti",
                    Trader1HOU = "Ti",
                    TraderHDP = "Ti",
                    TraderOU = "Ti",
                }
            };

        public List<AutoAssignmentResultModel> MockAssignmentResultModels()
            => new()
            {
                new AutoAssignmentResultModel()
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
                new AutoAssignmentResultModel()
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

        public List<TraderExportInfo> MockTraderExportInfos()
            => new()
            {
                new TraderExportInfo()
                {
                    InfoSiteMatchId = 100,
                    Trader1HHDP = "Ti",
                    Trader1HOU = "Ti",
                    TraderFTHDP = "Ti",
                    TraderFTOU = "Ti"
                },
                new TraderExportInfo()
                {
                    InfoSiteMatchId = 101,
                    Trader1HHDP = "Ti",
                    Trader1HOU = "Ti",
                    TraderFTHDP = "Ti",
                    TraderFTOU = "Ti"
                }
            };
    }
}
