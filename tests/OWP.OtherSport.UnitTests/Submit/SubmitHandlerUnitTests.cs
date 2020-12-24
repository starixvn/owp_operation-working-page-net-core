namespace OWP.OtherSport.UnitTests.Submit
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.NAP.Client.Helpers.Web;
    using NSubstitute;
    using OWP.OtherSport.Submit;
    using OWP.OtherSport.Submit.Models;
    using OWP.OtherSport.Submit.Services;
    using OWP.OtherSport.Validation.Models;
    using Xunit;

    public class SubmitHandlerUnitTests
    {
        private readonly CommandRepository commandRepository;
        private readonly CacheManipulator cacheManipulator;
        private readonly INapSession currentSession;
        private readonly SubmitHandler handler;

        public SubmitHandlerUnitTests()
        {
            commandRepository = Substitute.For<CommandRepository>();
            cacheManipulator = Substitute.For<CacheManipulator>();
            currentSession = Substitute.For<INapSession>();

            handler = new SubmitHandler(cacheManipulator, commandRepository, currentSession);
        }

        [Fact]
        public async Task Handle_ThereIsAProcessingUser_ReturnsThatUser()
        {
            // Arrange
            var request = new SubmitRequest();

            var emptyAssignments = new List<AssignmentModel>();
            const string processingUser = "AnyUser";
            const string sessionId = "sessionABC";

            cacheManipulator.GetUserWhoIsUploadingAssignment(Arg.Compat.Any<List<DateTime>>()).Returns(processingUser);
            await handler.SaveAssignmentsToDatabase(emptyAssignments, string.Empty);
            cacheManipulator.GetNewAssignments(sessionId).Returns(emptyAssignments);

            // Act
            var actual = await handler.Handle(request, new CancellationToken(false));

            // Assert
            Assert.Equal(processingUser, actual);
        }

        [Fact]
        public async Task Handle_NoProcessingUser_SnapshotShouldBeSavedAndDataShouldBeUpdated()
        {
            // Arrange
            var request = new SubmitRequest();

            var assignments = new List<AssignmentModel>
            {
                new AssignmentModel{ Home = "ABC"}
            };
            var snapShot = new AssignmentSnapshot
            {
                MaxBet = new Dictionary<int, List<MatchMaxBet>>
                {
                    { 1, new List<MatchMaxBet> { new MatchMaxBet { BetTypeName = "bet type" } } }
                }
            };
            const string processingUser = "";
            const string sessionId = "sessionABC";

            cacheManipulator.GetUserWhoIsUploadingAssignment(Arg.Compat.Any<List<DateTime>>()).Returns(processingUser);
            await handler.SaveAssignmentsToDatabase(assignments, string.Empty);
            cacheManipulator.GetNewAssignments(sessionId).Returns(assignments);
            await cacheManipulator.SaveUserWhoIsUploadingAssignment(new List<DateTime>(), "sa");
            await cacheManipulator.SaveOldAssignmentSnapshot(snapShot, sessionId);

            // Act
            var actual = await handler.Handle(request, new CancellationToken(false));

            // Assert
            Assert.Equal(string.Empty, actual);
            await commandRepository.Received().Update(string.Empty, assignments);
            await cacheManipulator.Received().SaveOldAssignmentSnapshot(snapShot,sessionId);
        }
    }
}
