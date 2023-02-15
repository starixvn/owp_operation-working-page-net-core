namespace OWP.LiveDetail.UnitTests.MatchSetting;

using AutoMapper;
using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using LiveDetail.MatchSetting;
using NSubstitute;
using Xunit;

public class RepositoryUnitTests
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly MatchSettingRepository matchSettingRepository;

    public RepositoryUnitTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperProfile());
        });
        var mapper = config.CreateMapper();
        var session = Substitute.For<INapSession>();
        dynamicRepository = Substitute.For<IDynamicRepository>();
        matchSettingRepository = new MatchSettingRepositoryImp(mapper, dynamicRepository, session);
    }

    [Fact]
    public async Task Query_Always_ReturnMatchSettingList()
    {
        // Arrange
        var queryModel = new QueryModel { OWPMatchId = 22 };
        var matchSettings = new List<MatchSetting> {
            new() { OWPMatchId = 12, Bettype = 1, MinorBettypeId = -2, DisplayName = "hdp", Spread = 11, Maxbet = 12, IsFromMaxbetSetting = false }
        };

        dynamicRepository.FetchAsync<MatchSetting>(Arg.Compat.Any<QueryCriteria>()).Returns(matchSettings);

        // Act
        var actual = await matchSettingRepository.Query(queryModel);

        // Assert
        Assert.Equal(matchSettings, actual);
        await dynamicRepository.Received().FetchAsync<MatchSetting>(Arg.Compat.Is<QueryCriteria>(x => x.OWPMatchId == queryModel.OWPMatchId));
    }
}