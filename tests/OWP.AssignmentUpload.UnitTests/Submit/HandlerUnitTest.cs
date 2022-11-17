namespace OWP.AssignmentUpload.UnitTests.Submit
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AssignmentUpload.Submit;
    using AssignmentUpload.Submit.Models;
    using AssignmentUpload.Submit.Services;
    using AssignmentUpload.Validation.Models;
    using Fanex.NAP.Client.Helpers.Web;
    using NSubstitute;
    using Xunit;

    public class HandlerUnitTest
    {
        private readonly CommandRepository commandRepository;
        private readonly QueryRepository queryRepository;
        private readonly CacheManipulator cacheManipulator;
        private readonly INapSession currentSession;
        private readonly Handler handler;

        public HandlerUnitTest()
        {
            commandRepository = Substitute.For<CommandRepository>();
            queryRepository = Substitute.For<QueryRepository>();
            cacheManipulator = Substitute.For<CacheManipulator>();
            currentSession = Substitute.For<INapSession>();

            queryRepository.GetOldMatchIds(Arg.Compat.Any<DateTime>()).Returns(new List<OldMatch>());
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

            var emptyAssignments = new List<AssignmentModel>();
            var emptySnapshot = new AssignmentSnapshot
            {
                Matches = new Dictionary<int, GeneralMatch>(),
                Assignments = new Dictionary<int, List<MatchAssignment>>(),
                MatchSettings = new Dictionary<int, List<MatchSetting>>()
            };
            const string processingUser = "AnyUser";

            cacheManipulator.GetUserWhoIsUploadingAssignment(Arg.Compat.Any<DateTime>()).Returns(processingUser);
            await handler.SaveAssignmentsToDatabase(request);

            // Act
            var actual = await handler.Handle(request, new CancellationToken(false));

            // Assert
            Assert.Equal(processingUser, actual);
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

            // Act
            var actual = await handler.Handle(request, new CancellationToken(false));

            // Assert
            Assert.Equal(string.Empty, actual);
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

            // Act
            var actual = await handler.Handle(request, new CancellationToken(false));

            // Assert
            Assert.Equal(string.Empty, actual);
            await commandRepository.Received().Update(emptyMatchIds, emptyAssignments, request);
            await cacheManipulator.Received().SaveAssignmentSnapshot(request.SessionId, emptySnapshot);
        }
    }
}
