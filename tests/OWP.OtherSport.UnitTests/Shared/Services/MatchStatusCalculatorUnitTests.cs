namespace OWP.OtherSport.UnitTests.Shared.Services;

using Extensions;
using Fanex.Data.Repository;
using NSubstitute;
using OtherSport.Shared.Services;
using OtherSport.Submit.Models;
using Xunit;

public class MatchStatusCalculatorUnitTests
{
    private readonly MatchStatusCalculator service;

    private readonly IDynamicRepository dynamicRepository;

    private readonly DateTime nowDateTime = DateTime.Now.ToGmt8();

    public MatchStatusCalculatorUnitTests()
    {
        dynamicRepository = Substitute.For<IDynamicRepository>();
        service = new MatchStatusCalculatorImp(dynamicRepository);
    }

    [Fact]
    public void Map()
    {
        dynamicRepository.FetchAsync<EventStatus>(Arg.Any<GetEventStatusFromMainDBCriteria>())
            .Returns(MockEventStatus());
        var matchInfo = GetAssignmentSnapshot();

        service.Map(matchInfo);

        Assert.Equal("Completed", matchInfo[1].MatchStatus);
        Assert.Equal("In-play", matchInfo[2].MatchStatus);
        Assert.Equal("Completed", matchInfo[3].MatchStatus);
        Assert.Equal("Prematch", matchInfo[4].MatchStatus);
        Assert.Equal("Early", matchInfo[5].MatchStatus);
    }

    public Dictionary<int, MatchInfo> GetAssignmentSnapshot()
    {
        return new Dictionary<int, MatchInfo>
        {
            { 1, new MatchInfo { OSMatchId = 1, AdminsiteMatchId = 111, DateTime = nowDateTime.AddDays(1) } },
            { 2, new MatchInfo { OSMatchId = 2, AdminsiteMatchId = 222, DateTime = nowDateTime.AddDays(-1) } },
            { 3, new MatchInfo { OSMatchId = 3, AdminsiteMatchId = 333, DateTime = nowDateTime } },
            { 4, new MatchInfo { OSMatchId = 3, AdminsiteMatchId = 444, DateTime = nowDateTime.AddHours(1) } },
            { 5, new MatchInfo { OSMatchId = 4, AdminsiteMatchId = 555, DateTime = nowDateTime.AddDays(1) } }
        };
    }

    private static List<EventStatus> MockEventStatus()
        => new()
        {
            new EventStatus { AdminsiteMatchId = 111, MatchStatus = "Completed" },
            new EventStatus { AdminsiteMatchId = 222, MatchStatus = "Live" },
            new EventStatus { AdminsiteMatchId = 333, MatchStatus = "completed" },
            new EventStatus { AdminsiteMatchId = 444, MatchStatus = "" }
        };
}