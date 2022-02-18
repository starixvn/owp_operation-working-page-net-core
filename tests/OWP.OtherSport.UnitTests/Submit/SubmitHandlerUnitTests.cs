//namespace OWP.OtherSport.UnitTests.Submit
//{
//    
//    
//    
//    
//    using Fanex.NAP.Client.Helpers.Web;
//    using Microsoft.AspNetCore.SignalR;
//    using NSubstitute;
//    using OWP.OtherSport.Hubs;
//    using OWP.OtherSport.Submit;
//    using OWP.OtherSport.Submit.Models;
//    using OWP.OtherSport.Submit.Services;
//    using OWP.OtherSport.Validation.Models;
//    using Xunit;
//    using Xunit.Asserts.Compare;

//    public class SubmitHandlerUnitTests
//    {
//        private readonly CommandRepository commandRepository;
//        private readonly CacheManipulator cacheManipulator;
//        private readonly INapSession currentSession;
//        private readonly SubmitHandler handler;
//        private readonly IHubContext<NewDataUploadNotifyHubImp, NewDataUploadNotifyHub> notifyHub;

//        public SubmitHandlerUnitTests()
//        {
//            commandRepository = Substitute.For<CommandRepository>();
//            cacheManipulator = Substitute.For<CacheManipulator>();
//            currentSession = Substitute.For<INapSession>();
//            notifyHub = Substitute.For<IHubContext<NewDataUploadNotifyHubImp, NewDataUploadNotifyHub>>();

//            handler = new SubmitHandler(cacheManipulator, commandRepository, currentSession, notifyHub);
//        }

//        [Fact]
//        public async Task Handle_ThereIsAProcessingUser_ReturnsThatUser()
//        {
//            // Arrange
//            const string sessionId = "sessionABC";
//            var request = new SubmitRequest { SessionId = sessionId };

//            var emptyAssignments = new List<AssignmentModel>
//            {
//                new() { DateTime=new DateTime(2020,1,2) },
//                new() { DateTime=new DateTime(2020,1,3) }
//            };
//            const string processingUser = "anyUser";
//            var response = new SubmitResponse
//            {
//                IsSuccessful = false,
//                Message = $"User {processingUser} is uploading another assignment file for date Jan 02, 2020. Please try again later!"
//            };

//            cacheManipulator.GetUserWhoIsUploadingAssignment(Arg.Compat.Any<DateTime>()).Returns(processingUser);
//            await handler.SaveAssignmentsToDatabase(emptyAssignments, string.Empty);
//            cacheManipulator.GetNewAssignments(sessionId).Returns(emptyAssignments);

//            // Act
//            var actual = await handler.Handle(request, new CancellationToken(false));

//            // Assert
//            DeepAssert.Equal(response, actual);
//        }

//        [Fact]
//        public async Task Handle_NoProcessingUser_SnapshotShouldBeSavedAndDataShouldBeUpdated()
//        {
//            // Arrange
//            const string sessionId = "sessionABC";
//            var request = new SubmitRequest { SessionId = sessionId };

//            var assignments = new List<AssignmentModel>
//            {
//                new AssignmentModel{ Home = "ABC", DateTime=new DateTime(2020,1,2) }
//            };
//            var snapShot = new AssignmentSnapshot
//            {
//                MaxBet = new Dictionary<int, List<MatchMaxBet>>
//                {
//                    { 1, new List<MatchMaxBet> { new MatchMaxBet { BetTypeName = "bet type" } } }
//                }
//            };
//            const string processingUser = "";
//            var response = new SubmitResponse
//            {
//                IsSuccessful = true,
//                Message = "Other sport assignment is successfully submitted!"
//            };

//            cacheManipulator.GetUserWhoIsUploadingAssignment(Arg.Compat.Any<DateTime>()).Returns(processingUser);
//            await handler.SaveAssignmentsToDatabase(assignments, string.Empty);
//            cacheManipulator.GetNewAssignments(sessionId).Returns(assignments);
//            await cacheManipulator.SaveUserWhoIsUploadingAssignment(new List<DateTime>(), "sa");
//            await cacheManipulator.SaveOldAssignmentSnapshot(snapShot, sessionId);
//            await cacheManipulator.DeleteUserWhoIsUploadingAssignment(new List<DateTime>());

//            // Act
//            var actual = await handler.Handle(request, new CancellationToken(false));

//            // Assert
//            DeepAssert.Equal(response, actual);
//            await notifyHub.Received().Clients.All.NotifyUploadedNewData();
//            await commandRepository.Received().Update(string.Empty, assignments);
//            await cacheManipulator.Received().SaveOldAssignmentSnapshot(snapShot,sessionId);
//        }
//    }
//}
