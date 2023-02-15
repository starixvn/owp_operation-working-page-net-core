namespace OWP.LiveGameList.UnitTests.Services;

using Criteria;
using Fanex.Data.Repository;
using LiveGameList.Services;
using Models;
using NSubstitute;
using Xunit;

public class GetAdminSiteMatchIdServiceUnitTest
{
    private readonly IDynamicRepository dynamicRepository;

    public GetAdminSiteMatchIdServiceUnitTest()
    {
        dynamicRepository = Substitute.For<IDynamicRepository>();
    }
    [Fact]
    public async Task UpdateMinorAdminMatchId_Always_ReturnExpect()
    {
        var matchInfo = MockMatchInfo();
        dynamicRepository.FetchAsync<MinorMatchInfo>(Arg.Any<GetMinorAdminSiteIdsCriteria>())
            .Returns(MockMinorMatchInfo());

        var service = new GetAdminSiteMatchIdServiceImp(dynamicRepository);
        await service.UpdateMinorAdminMatchId(matchInfo);

        Assert.Equal(2, matchInfo.First().AdminSiteId);
        Assert.Equal(1, matchInfo.Last().AdminSiteId);
    }

    public List<MatchInfo> MockMatchInfo()
        => new()
        {
            new MatchInfo { OWPMatchId = 17, MinorBettypeId = 112, AdminSiteId = 1},
            new MatchInfo { OWPMatchId = 18, MinorBettypeId = 113,AdminSiteId = 1},
            new MatchInfo { OWPMatchId = 18, MinorBettypeId = -4,AdminSiteId = 1},
            new MatchInfo { OWPMatchId = 18, MinorBettypeId = -3,AdminSiteId = 1}
        };

    public IEnumerable<MinorMatchInfo> MockMinorMatchInfo()
        => new List<MinorMatchInfo> { new() { AdminMatchId = 2, OwpMatchId = 17 } };
}

