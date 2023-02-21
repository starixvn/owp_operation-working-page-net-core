namespace OWP.LiveDetail.UnitTests.MatchNote;

using AutoMapper;
using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using LiveDetail.MatchNote;
using NSubstitute;
using Xunit;
using Xunit.Asserts.Compare;

public class RepositoryUnitTests
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSession currentSession;
    private readonly MatchNoteRepository repository;

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
        repository = new MatchNoteRepositoryImp(mapper, dynamicRepository, currentSession);
    }

    [Fact]
    public async Task Query_Always_ReturnMatchNoteList()
    {
        // Arrange
        var queryModel = new QueryModel { OWPMatchId = 22 };
        var matchNotes = new List<MatchNoteModel>
        {
            new() { MatchNoteId = 1, Username = "sa", Note="note" }
        };

        dynamicRepository.FetchAsync<MatchNoteModel>(Arg.Compat.Any<QueryCriteria>()).Returns(matchNotes);

        // Act
        var actual = await repository.Query(queryModel);

        // Assert
        Assert.Equal(matchNotes, actual);
        await dynamicRepository.Received().FetchAsync<MatchNoteModel>(Arg.Compat.Is<QueryCriteria>(x => x.OWPMatchId == queryModel.OWPMatchId));
    }

    [Fact]
    public async Task Create_Always_ReturnResult()
    {
        // Arrange
        var model = new CreateModel
        {
            OWPMatchId = 1,
            Note = "note"
        };
        const int commandResult = 0;
        var sessionId = string.Empty;
        dynamicRepository.ExecuteAsync(Arg.Compat.Any<CreateCriteria>()).Returns(x =>
        {
            var criteria = x.Arg<CreateCriteria>();
            criteria.MatchNoteId = 123;
            criteria.ErrMsg = "123";
            sessionId = criteria.SessionId;
            return Task.FromResult(commandResult);
        });

        // Act
        var result = await repository.Create(model);

        // Assert
        var expected = new CreateResult
        {
            SessionId = sessionId,
            MatchNoteId = 123,
            Username = "us"
        };
        DeepAssert.Equal(expected, result);
        await dynamicRepository.Received().ExecuteAsync(Arg.Compat.Is<CreateCriteria>(p =>
            p.UserId == currentSession.UserId &&
            p.OWPMatchId == model.OWPMatchId &&
            p.Note == model.Note
        ));
    }

    [Fact]
    public async Task Update_Always_ReturnResult()
    {
        // Arrange
        var model = new UpdateModel
        {
            MatchNoteId = 1,
            Note = "note"
        };
        const int commandResult = 0;
        var sessionId = string.Empty;
        dynamicRepository.ExecuteAsync(Arg.Compat.Any<UpdateCriteria>()).Returns(x =>
        {
            var criteria = x.Arg<UpdateCriteria>();
            sessionId = criteria.SessionId;
            return Task.FromResult(commandResult);
        });

        // Act
        var actual = await repository.Update(model);

        // Assert
        Assert.Equal(sessionId, actual);
        await dynamicRepository.Received().ExecuteAsync(Arg.Compat.Is<UpdateCriteria>(p =>
            p.UserId == currentSession.UserId &&
            p.MatchNoteId == model.MatchNoteId &&
            p.Note == model.Note
        ));
    }

    [Fact]
    public async Task Delete_Always_ReturnResult()
    {
        // Arrange
        var model = new DeleteModel
        {
            MatchNoteId = 1
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
        Assert.Equal(sessionId, actual);
        await dynamicRepository.Received().ExecuteAsync(Arg.Compat.Is<DeleteCriteria>(p =>
            p.UserId == currentSession.UserId &&
            p.MatchNoteId == model.MatchNoteId
        ));
    }
}