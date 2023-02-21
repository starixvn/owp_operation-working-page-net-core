using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using NSubstitute;
using OWP.AssignmentUpload.Submit;
using OWP.AssignmentUpload.Submit.Criteria;
using OWP.AssignmentUpload.Submit.Models;
using OWP.AssignmentUpload.Submit.Services;
using OWP.AssignmentUpload.Validation.Models;
using OWP.Services;
using Xunit;

namespace OWP.AssignmentUpload.UnitTests.Submit.Services;

public class CommandRepositoryIntegrationTest
{
    [Fact]
    public async Task Write_InsertDataToDB_ReturnMatchByHashKey()
    {
        var request = new SubmitRequest
        {
            SessionId = "793f6d1977a600d4a8a7df4407e19f67c1c376a8d1832cd19e7b12486d93565d",
            EventDate = DateTime.Parse("05/30/2020")
        };
        var assignments = GetListAssignmentModel();
        var hashKey = new List<MatchByHashKey> { new()
        {
            HashKey = "793f6d1977a600d4a8a7df4407e19f67c1c376a8d1832cd19e7b12486d93565d",
            OWPMatchId = 111
        } };

        var dynamicRepository = Substitute.For<IDynamicRepository>();
        dynamicRepository.FetchAsync<MatchByHashKey>(Arg.Any<InsertOrUpdateMatchCriteria>()).Returns(hashKey);
        var currentSession = Substitute.For<INapSession>();
        currentSession.UserId.Returns(0);

        var analyzer = new AssignmentAnalyzerImp();
        var logService = Substitute.For<LogServiceWrapper>();
        var cacheManipulator = Substitute.For<CacheManipulator>();

        var realCommandRepository = new CommandRepositoryImp(dynamicRepository, currentSession, analyzer, logService, cacheManipulator);
        var emptyMatchIds = new List<OldMatch>();
        await realCommandRepository.Update(emptyMatchIds, assignments, request);
    }

    private static List<AssignmentModel> GetListAssignmentModel()
    {
        var assignment = new AssignmentModel
        {
            SessionId = "793f6d1977a600d4a8a7df4407e19f67c1c376a8d1832cd19e7b12486d93565d",
            HashKey = "793f6d1977a600d4a8a7df4407e19f67c1c376a8d1832cd19e7b12486d93565d",
            Away = "Barcelona",
            Home = "Aletico Marid",
            League = "Laliga",
            Agent = "IPSD",
            Channel = "",
            IPTV = "",
            Division = "",
            EventDate = DateTime.Parse("05/30/2020"),
            InfoSiteMatchId = 0,
            AdminSiteId = 0,
            IsImplicitMinorMatch = false,
            OWPMatchId = 123,
            MatchNo = "",
            MatchOffer = "20s Accept",
            MaxBetHDP = 2000,
            MaxBet1H1X2 = 2000,
            MaxBet1HHDP = 2000,
            MaxBet1HOU = 2000,
            MaxBet1X2 = 2000,
            MaxBetFM = 2000,
            MaxBetOU = 2000,
            MaxBetXMins = 4000,
            Spread1H1X2 = 50,
            Spread1HHDP = 50,
            Spread1HOU = 50,
            Spread1X2 = 50,
            SpreadFM = 50,
            SpreadHDP = 50,
            SpreadOU = 50,
            SpreadXMins = 50,
            MatchStatus = "",
            MinorBettypeId = 0,
            Phone = "023049223",
            Remark = "ABCD",
            Round = "2",
            NoHDP = "",
            Notes = new List<string> { string.Empty },
            Trader10Mins = 123,
            Trader15Mins = 123,
            Trader1H1X2 = 123,
            Trader1HHDP = 123,
            Trader1HOU = 123,
            Trader1X2 = 1234,
            TraderAIO = 1234,
            TraderFM = 123,
            TraderHDP = 1234,
            TraderLA = 1234,
            TraderOU = 124,
            WB = "",
            Website1 = "Web1",
            Website2 = "Web2",
            Website3 = "Web3",
            Website4 = "Web4",
            Provider = "",
            OtherProvider = "",
            TV = "",
            OtherTradersHDP = "123,starix1 abc<o>456,starix16 abc",
            OtherTraders10Mins = "",
            OtherTraders15Mins = "",
            OtherTraders1H1X2 = "",
            OtherTraders1HHDP = "",
            OtherTraders1HOU = "",
            OtherTraders1X2 = "",
            OtherTradersAIO = "",
            OtherTradersFM = "",
            OtherTradersLA = "",
            OtherTradersOU = "",
            OtherTime = new TimeSpan(12, 30, 62),
            KickOffTime = new TimeSpan(12, 30, 62)
        };

        var listAssignment = new List<AssignmentModel> { assignment };
        return listAssignment;
    }
}