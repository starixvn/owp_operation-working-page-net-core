using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Fanex.NAP.Client;
using MediatR;
using NSubstitute;
using OWP.CQRS;
using OWP.NewLeagueSetting.Command;
using OWP.NewLeagueSetting.Criteria;
using OWP.NewLeagueSetting.Data;
using Xunit;

namespace OWP.NewLeagueSetting.UnitTests;

public class InsertLeagueSettingHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldBuildStandardizedInfo_AndCallRepository_AndReturnLeagueSettingId()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        var mapper = Substitute.For<IMapper>();

        session.UserId.Returns(123);

        var handler = new InsertLeagueSettingHandler(repository, session, mapper);

        var request = new InsertLeagueSettingRequest
        {
            MaxbetWD_HDP = "1000",
            MaxbetWK_HDP = "2000",
            WD_NoOfHDP = "1",
            WK_NoOfHDP = "2",
            Spread = "0.05"
        };

        var criteria = new InsertLeagueSettingCriteria
        {
            LeagueSettingId = 456
        };

        mapper.Map<InsertLeagueSettingCriteria>(request).Returns(criteria);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(
            Arg.Is<InsertLeagueSettingCriteria>(c =>
                c.ActorId == 123 &&
                c.LeagueSettingId == 456));

        Assert.Equal(456, result);
    }

    [Fact]
    public async Task Handle_WithDifferentValues_ShouldCallRepositoryWithCorrectActorId()
    {
        // Arrange
        var repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        var mapper = Substitute.For<IMapper>();

        session.UserId.Returns(789);

        var handler = new InsertLeagueSettingHandler(repository, session, mapper);

        var request = new InsertLeagueSettingRequest
        {
            MaxbetWD_HDP = "3000",
            MaxbetWK_HDP = "4000",
            WD_NoOfHDP = "3",
            WK_NoOfHDP = "4",
            Spread = "0.10"
        };

        var criteria = new InsertLeagueSettingCriteria
        {
            LeagueSettingId = 999
        };

        mapper.Map<InsertLeagueSettingCriteria>(request).Returns(criteria);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await repository.Received(1).ExecuteNonQueryCommand(
            Arg.Is<InsertLeagueSettingCriteria>(c =>
                c.ActorId == 789 &&
                c.LeagueSettingId == 999));

        Assert.Equal(999, result);
    }
}