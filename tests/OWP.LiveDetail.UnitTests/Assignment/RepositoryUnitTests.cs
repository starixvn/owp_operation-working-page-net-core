using AutoMapper;
using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using NSubstitute;
using OWP.LiveDetail.Assignment;
using OWP.LiveDetail.Common;
using Xunit;
using Xunit.Asserts.Compare;

[assembly: CollectionBehavior(MaxParallelThreads = 1)]

namespace OWP.LiveDetail.UnitTests.Assignment;

public class RepositoryUnitTests
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSession currentSession;
    private readonly AssignmentRepository repository;

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
        repository = new AssignmentRepositoryImp(mapper, dynamicRepository, currentSession);
    }

    [Fact]
    public async Task Query_Always_ReturnsData()
    {
        // Arrange
        var assignments = new List<LiveDetail.Assignment.Assignment>
        {
            new()
            {
                AssignmentId=1,
                AssignmentNote="Note",
                AssignmentType =1,
                Bettype=1,
                DisplayName="superman",
                MinorBettypeId=1,
                OWPMatchId=1,
                UserId=1,
                Username="sa"
            }
        };
        dynamicRepository.FetchAsync<LiveDetail.Assignment.Assignment>(Arg.Compat.Any<QueryCriteria>()).Returns(assignments);

        // Act
        var queryModel = new QueryModel
        {
            OWPMatchId = 1
        };
        var result = await repository.Query(queryModel);

        // Assert
        Assert.Equal(assignments, result);
    }

    [Fact]
    public async Task Validate_Always_ReturnsData()
    {
        // Arrange
        var model = new ValidateModel
        {
            OWPMatchId = 1,
            Username = "sa",
            UserId = 8,
            AssignmentType = 1,
            Bettype = 1
        };
        IEnumerable<ValidateEntity> validateEntities = new List<ValidateEntity>
        {
            new() { ResultType = 3 },
            new() { ResultType = 1, Message = "Fail1" }
        };

        dynamicRepository.FetchAsync<ValidateEntity>(Arg.Compat.Any<ValidateCriteria>()).Returns(Task.FromResult(validateEntities));

        // Act
        var result = await repository.Validate(model);

        // Assert
        var expectedValidateEntities = new List<ValidateEntity>
        {
            new() { ResultType = 1, Message = "Fail1" }
        };
        DeepAssert.Equal(expectedValidateEntities, result.Results.ToList());
    }

    [Fact]
    public async Task Create_Always_ReturnsData()
    {
        // Arrange
        var model = new CreateModel
        {
            OWPMatchId = 1,
            AssignmentNote = "Note",
            AssignmentType = 1,
            BetType = 1,
            UserId = 8,
            Username = "sa"
        };
        const int commandResult = 0;
        dynamicRepository.ExecuteAsync(Arg.Compat.Any<NonQueryCommand>()).Returns(Task.FromResult(commandResult));

        // Act
        var result = await repository.Create(model);

        // Assert
        Assert.Equal(commandResult, result.Code);
        Assert.NotEmpty(result.SessionId);
        await dynamicRepository.Received().ExecuteAsync(Arg.Compat.Is<CreateCriteria>(p =>
            p.ActorId == currentSession.UserId &&
            p.OWPMatchId == model.OWPMatchId
        ));
    }

    [Fact]
    public async Task Update_Always_ReturnsData()
    {
        // Arrange
        var model = new UpdateModel
        {
            OWPMatchId = 1,
            AssignmentNote = "Note",
            AssignmentType = 1,
            BetType = 1,
            UserId = 8,
            Username = "sa",
            AssignmentId = 1
        };
        const int commandResult = 0;
        dynamicRepository.ExecuteAsync(Arg.Compat.Any<NonQueryCommand>()).Returns(Task.FromResult(commandResult));

        // Act
        var result = await repository.Update(model);

        // Assert
        Assert.Equal(commandResult, result.Code);
        Assert.NotEmpty(result.SessionId);
    }

    [Fact]
    public async Task Delete_Always_ReturnsData()
    {
        // Arrange
        var model = new DeleteModel
        {
            OWPMatchId = 1,
            AssignmentType = 1,
            BetType = 1,
            AssignmentId = 1
        };
        const int commandResult = 0;
        dynamicRepository.ExecuteAsync(Arg.Compat.Any<NonQueryCommand>()).Returns(Task.FromResult(commandResult));

        // Act
        var result = await repository.Delete(model);

        // Assert
        Assert.Equal(commandResult, result.Code);
        Assert.NotEmpty(result.SessionId);
    }
}