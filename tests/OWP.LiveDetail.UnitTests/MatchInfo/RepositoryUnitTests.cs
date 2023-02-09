namespace OWP.LiveDetail.UnitTests.MatchInfo;

using AutoMapper;
using Common;
using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using LiveDetail.MatchInfo;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Asserts.Compare;
using AgentSpace = LiveDetail.Agent;
using OtherProviderSpace = LiveDetail.OtherProvider;
using ProviderSpace = LiveDetail.Provider;
using TelevisionSpace = LiveDetail.Television;

public class RepositoryUnitTests
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSession currentSession;
    private readonly MatchInfoRepository repository;

    public RepositoryUnitTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperProfile());
        });
        var mapper = config.CreateMapper();
        dynamicRepository = Substitute.For<IDynamicRepository>();
        currentSession = Substitute.For<INapSession>();
        currentSession.UserId = 1;
        repository = new MatchInfoRepositoryImp(mapper, dynamicRepository, currentSession);
    }

    [Fact]
    public async Task Query_ValidMatchId_Always_ReturnsData()
    {
        // Arrange
        const int owpMatchId = 1;
        var matchInfo = new MatchInfoModel
        {
            Channel = "cn1",
            WB = "wb",
            IpTv = "IpTv",
            Phone = "090",
            Remark = "rm",
            IsSelected = true,
            NoOfHDPId = 1,
            IsSoloMatch = true,
            Round = "round",
            Division = "divi",
            Venue = "venue",
            OtherTime = TimeSpan.Parse("23:00"),
            LeagueName = "league",
            MatchNo = "1c",
            EventDate = new DateTime(2018, 12, 19),
            KickOffTime = new DateTime(2018, 12, 19, 15, 45, 30)
        };
        dynamicRepository.GetAsync<MatchInfoModel>(Arg.Compat.Any<QueryCriteria>()).Returns(Task.FromResult(matchInfo));

        // Act
        var result = await repository.Query(owpMatchId);

        // Assert
        Assert.Equal(matchInfo, result);
        Assert.Equal("Dec 19, 2018", result.EventDateDisplay);
    }

    [Fact]
    public async Task Query_InvalidMatchIdAlways_ReturnsNull()
    {
        // Arrange
        const int owpMatchId = 1;
        MatchInfoModel nullMatchInfo = null;
        dynamicRepository.GetAsync<MatchInfoModel>(Arg.Compat.Any<QueryCriteria>()).Returns(Task.FromResult(nullMatchInfo));

        // Act
        var result = await repository.Query(owpMatchId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ValidatePhone_Always_ReturnsData()
    {
        // Arrange
        var model = new ValidatePhoneModel
        {
            OWPMatchId = 1
        };
        IEnumerable<ValidateEntity> validateEntities = new List<ValidateEntity>
        {
            new ValidateEntity { ResultType = 3 },
            new ValidateEntity { ResultType = 1, Message = "Fail1" }
        };
        dynamicRepository.FetchAsync<ValidateEntity>(Arg.Compat.Any<ValidatePhoneCriteria>()).Returns(Task.FromResult(validateEntities));

        // Act
        var result = await repository.ValidatePhone(model);

        // Assert
        var expectedValidateEntities = new List<ValidateEntity>
        {
            new ValidateEntity { ResultType = 1, Message = "Fail1" }
        };
        DeepAssert.Equal(expectedValidateEntities, result.Results.ToList());
    }

    [Fact]
    public async Task UpdateMatchInfo_Always_ReturnsData()
    {
        // Arrange
        var model = new UpdateMatchInfoModel
        {
            OWPMatchId = 1,
            IsSelected = true
        };
        const int commandResult = 0;
        dynamicRepository.ExecuteAsync(Arg.Compat.Any<NonQueryCommand>()).Returns(Task.FromResult(commandResult));

        // Act
        var result = await repository.UpdateMatchInfo(model);

        // Assert
        Assert.Equal(commandResult, result.Code);
        Assert.NotEmpty(result.Data as string);
        await dynamicRepository.Received().ExecuteAsync(Arg.Compat.Is<UpdateMatchInfoCriteria>(p =>
            p.ActorId == currentSession.UserId &&
            p.OWPMatchId == model.OWPMatchId
        ));
    }

    [Fact]
    public async Task GetTelevisionsByMatchId_Always_ReturnsData()
    {
        // Arrange
        const int owpMatchId = 1;
        IEnumerable<TelevisionSpace.Television> televisions = new List<TelevisionSpace.Television>();
        dynamicRepository.FetchAsync<TelevisionSpace.Television>(Arg.Compat.Any<GetTelevisionsByMatchIdCriteria>()).Returns(Task.FromResult(televisions));

        // Act
        var result = await repository.GetTelevisionsByMatchId(owpMatchId);

        // Assert
        Assert.Equal(televisions, result);
        await dynamicRepository.Received().FetchAsync<TelevisionSpace.Television>(Arg.Compat.Is<GetTelevisionsByMatchIdCriteria>(p =>
            p.OWPMatchId == owpMatchId
        ));
    }

    [Fact]
    public async Task GetOtherProvidersByMatchId_Always_ReturnsData()
    {
        // Arrange
        const int owpMatchId = 1;
        IEnumerable<OtherProviderSpace.OtherProvider> otherProviders = new List<OtherProviderSpace.OtherProvider>();
        dynamicRepository.FetchAsync<OtherProviderSpace.OtherProvider>(Arg.Compat.Any<GetOtherProvidersByMatchIdCriteria>()).Returns(Task.FromResult(otherProviders));

        // Act
        var result = await repository.GetOtherProvidersByMatchId(owpMatchId);

        // Assert
        Assert.Equal(otherProviders, result);
        await dynamicRepository.Received().FetchAsync<OtherProviderSpace.OtherProvider>(Arg.Compat.Is<GetOtherProvidersByMatchIdCriteria>(p =>
            p.OWPMatchId == owpMatchId
        ));
    }

    [Fact]
    public async Task GetProvidersByMatchId_Always_ReturnsData()
    {
        // Arrange
        const int owpMatchId = 1;
        IEnumerable<ProviderSpace.Provider> providers = new List<ProviderSpace.Provider>();
        dynamicRepository.FetchAsync<ProviderSpace.Provider>(Arg.Compat.Any<GetProvidersByMatchIdCriteria>()).Returns(Task.FromResult(providers));

        // Act
        var result = await repository.GetProvidersByMatchId(owpMatchId);

        // Assert
        Assert.Equal(providers, result);
        await dynamicRepository.Received().FetchAsync<ProviderSpace.Provider>(Arg.Compat.Is<GetProvidersByMatchIdCriteria>(p =>
            p.OWPMatchId == owpMatchId
        ));
    }

    [Fact]
    public async Task GetAgentsByMatchId_Always_ReturnsData()
    {
        // Arrange
        const int owpMatchId = 1;
        IEnumerable<AgentSpace.Agent> agents = new List<AgentSpace.Agent>();
        dynamicRepository.FetchAsync<AgentSpace.Agent>(Arg.Compat.Any<GetAgentsByMatchIdCriteria>()).Returns(Task.FromResult(agents));

        // Act
        var result = await repository.GetAgentsByMatchId(owpMatchId);

        // Assert
        Assert.Equal(agents, result);
        await dynamicRepository.Received().FetchAsync<AgentSpace.Agent>(Arg.Compat.Is<GetAgentsByMatchIdCriteria>(p =>
            p.OWPMatchId == owpMatchId
        ));
    }

    [Fact]
    public async Task GetMatchStatusByMatchId_Always_ReturnsData()
    {
        // Arrange
        const int owpMatchId = 1;
        IEnumerable<MatchStatusModel> matchStatusList = new List<MatchStatusModel> {
            new MatchStatusModel
            {
                Value = "cancel", DisplayText = "cancel", IsSelected = true
            }
        };
        dynamicRepository.FetchAsync<MatchStatusModel>(Arg.Compat.Any<GetMatchStatusByMatchIdCriteria>()).Returns(Task.FromResult(matchStatusList));

        // Act
        var result = await repository.GetMatchStatusByMatchId(owpMatchId);

        // Assert
        Assert.Equal(matchStatusList, result);
        await dynamicRepository.Received().FetchAsync<MatchStatusModel>(Arg.Compat.Is<GetMatchStatusByMatchIdCriteria>(p =>
            p.OWPMatchId == owpMatchId
        ));
    }
}