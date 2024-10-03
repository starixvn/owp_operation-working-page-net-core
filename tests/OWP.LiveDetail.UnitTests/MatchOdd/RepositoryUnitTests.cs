using AutoMapper;
using Fanex.Data.Repository;
using Fanex.NAP.Client;
using KellermanSoftware.CompareNetObjects;
using NSubstitute;
using OWP.LiveDetail.MatchOdd;
using Xunit;

namespace OWP.LiveDetail.UnitTests.MatchOdd;

using Xunit.Asserts.Compare;

public class RepositoryUnitTests
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSession currentSession;
    private readonly MatchOddRepository repository;

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
        currentSession.UserName = "us";
        repository = new MatchOddRepositoryImp(mapper, dynamicRepository, currentSession);
    }

    [Fact]
    public async Task Query_Always_ReturnMatchOddList()
    {
        // Arrange
        var queryModel = new QueryModel { OWPMatchId = 22 };
        var matchOdds = new List<LiveDetail.MatchOdd.MatchOdd>
        {
            new() { Id = 1, Bettype = 1, TeamChoice = 1, HDP = 1, Choice = 1, Odds = 1 }
        };

        dynamicRepository.FetchAsync<LiveDetail.MatchOdd.MatchOdd>(Arg.Compat.Any<QueryCriteria>()).Returns(matchOdds);

        // Act
        var actual = await repository.Query(queryModel);

        // Assert
        Assert.Equal(matchOdds, actual);
        await dynamicRepository.Received().FetchAsync<LiveDetail.MatchOdd.MatchOdd>(Arg.Compat.Is<QueryCriteria>(x => x.OWPMatchId == queryModel.OWPMatchId));
    }
    [Fact]
    public async Task Delete_Always_ReturnResult()
    {
        // Arrange
        var model = new DeleteModel
        {
            Id = 1
        };
        const int commandResult = 0;
        var sessionId = string.Empty;
        dynamicRepository.ExecuteAsync(Arg.Compat.Any<DeleteCriteria>()).Returns(x =>
        {
            var criteria = x.Arg<DeleteCriteria>();
            sessionId = criteria.SessionId;
            return Task.FromResult(commandResult);
        });

        // Act
        var actual = await repository.Delete(model);

        // Assert
        var expected = new ResponseResult
        {
            Code = 0,
            Message = "Ok",
            SessionId = sessionId
        };
        DeepAssert.Equal(expected, actual);
        var compareLogic = new CompareLogic();
        await dynamicRepository.Received().ExecuteAsync(Arg.Compat.Is<DeleteCriteria>(p =>
            compareLogic.Compare(
                new
                {
                    p.UserId,
                    p.Id,
                    p.SessionId
                },
                new
                {
                    currentSession.UserId,
                    model.Id,
                    SessionId = sessionId
                }).AreEqual
        ));
    }
}