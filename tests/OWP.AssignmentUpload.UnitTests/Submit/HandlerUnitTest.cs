using Fanex.NAP.Client;
using NSubstitute;
using OWP.AssignmentUpload.Submit;
using OWP.AssignmentUpload.Submit.Models;
using OWP.AssignmentUpload.Submit.Services;
using OWP.AssignmentUpload.Validation.Models;
using Xunit;

namespace OWP.AssignmentUpload.UnitTests.Submit;

using Xunit.Asserts.Compare;

public class HandlerUnitTest
{
    private readonly CommandRepository commandRepository;
    private readonly QueryRepository queryRepository;
    private readonly CacheManipulator cacheManipulator;
    private readonly Handler handler;

    public HandlerUnitTest()
    {
        commandRepository = Substitute.For<CommandRepository>();
        queryRepository = Substitute.For<QueryRepository>();
        cacheManipulator = Substitute.For<CacheManipulator>();
        var currentSession = Substitute.For<INapSession>();

        queryRepository.GetOldMatchIds(Arg.Compat.Any<DateTime>()).Returns([]);
        handler = new Handler(queryRepository, cacheManipulator, commandRepository, currentSession);
    }

    [Fact]
    public async Task Handle_ThereIsAProcessingUser_ReturnsThatUser()
    {
        // Arrange
        var request = new SubmitRequest
        {
            EventDate = DateTime.Parse("05/25/2020"),
            SessionId = "sessionId"
        };

        const string processingUser = "AnyUser";
        var expectedResponse = new SubmitResponse
        {
            IsSuccessful = false,
            Message = "User AnyUser is uploading another assignment file for date  May 25, 2020. Please try again later!"
        };
        cacheManipulator.GetUserWhoIsUploadingAssignment(Arg.Compat.Any<DateTime>()).Returns(processingUser);
        await handler.SaveAssignmentsToDatabase(request);

        // Act
        var actual = await handler.Handle(request, new CancellationToken(false));

        // Assert
        DeepAssert.Equal(expectedResponse, actual);
    }

    [Fact]
    public async Task Handle_NoProcessingUser_SnapshotShouldBeSavedAndDataShouldBeUpdated()
    {
        // Arrange
        var emptyMatchIds = new List<OldMatch>();
        var request = new SubmitRequest
        {
            EventDate = DateTime.Parse("05/25/2020"),
            SessionId = "sessionId"
        };

        var emptyAssignments = new List<AssignmentModel>();
        var emptySnapshot = new AssignmentSnapshot
        {
            Matches = new Dictionary<int, GeneralMatch>(),
            Assignments = new Dictionary<int, List<MatchAssignment>>(),
            MatchSettings = new Dictionary<int, List<MatchSetting>>()
        };

        queryRepository.GetOldMatchIds(Arg.Compat.Any<DateTime>()).Returns(emptyMatchIds);
        queryRepository.Query(request.EventDate).Returns(Task.FromResult(emptySnapshot));
        cacheManipulator.GetAssignments(Arg.Compat.Any<string>())
            .Returns(emptyAssignments);
        cacheManipulator.GetUserWhoIsUploadingAssignment(Arg.Compat.Any<DateTime>()).Returns(string.Empty);
        await handler.SaveAssignmentsToDatabase(request);
        var expectedResponse = new SubmitResponse
        {
            IsSuccessful = true,
            Message = "Event assignment is successfully submitted!"
        };

        // Act
        var actual = await handler.Handle(request, new CancellationToken(false));

        // Assert
        DeepAssert.Equal(expectedResponse, actual);
        await commandRepository.Received().Update(emptyMatchIds, emptyAssignments, request);
        await cacheManipulator.Received().SaveAssignmentSnapshot(request.SessionId, emptySnapshot);
    }

    [Fact]
    public async Task Handle_NoProcessingUser_SnapshotShouldBeSavedAndDataShouldBeUpdated_ReturnEmpty()
    {
        // Arrange
        var emptyMatchIds = new List<OldMatch>();
        var request = new SubmitRequest
        {
            EventDate = DateTime.Parse("05/25/2020"),
            SessionId = "sessionId"
        };

        var emptyAssignments = new List<AssignmentModel>();
        var emptySnapshot = new AssignmentSnapshot
        {
            Matches = new Dictionary<int, GeneralMatch>(),
            Assignments = new Dictionary<int, List<MatchAssignment>>(),
            MatchSettings = new Dictionary<int, List<MatchSetting>>()
        };

        queryRepository.GetOldMatchIds(Arg.Compat.Any<DateTime>()).Returns(emptyMatchIds);
        queryRepository.Query(request.EventDate).Returns(Task.FromResult(emptySnapshot));
        cacheManipulator.GetAssignments(Arg.Compat.Any<string>())
            .Returns(emptyAssignments);
        cacheManipulator.GetUserWhoIsUploadingAssignment(Arg.Compat.Any<DateTime>()).Returns(string.Empty);
        await handler.SaveAssignmentsToDatabase(request);
        var expectedResponse = new SubmitResponse
        {
            IsSuccessful = true,
            Message = "Event assignment is successfully submitted!"
        };

        // Act
        var actual = await handler.Handle(request, new CancellationToken(false));

        // Assert
        DeepAssert.Equal(expectedResponse, actual);
        await commandRepository.Received().Update(emptyMatchIds, emptyAssignments, request);
        await cacheManipulator.Received().SaveAssignmentSnapshot(request.SessionId, emptySnapshot);
    }
}