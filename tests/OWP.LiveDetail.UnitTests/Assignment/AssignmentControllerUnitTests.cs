namespace OWP.LiveDetail.UnitTests.Assignment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common;
    using Fanex.Data.Repository;
    using OWP.LiveDetail.Assignment;
    using NSubstitute;
    using Services;
    using Xunit;

    public class AssignmentControllerUnitTests
    {
        private readonly AssignmentRepository repository;
        //private readonly NotificationService notificationService;
        private readonly AssignmentController controller;

        public AssignmentControllerUnitTests()
        {
            repository = Substitute.For<AssignmentRepository>();
            Substitute.For<IDynamicRepository>();
            // notificationService = Substitute.For<NotificationService>();
            controller = new AssignmentController(repository);
        }

        [Fact]
        public async Task Query_Always_ReturnsData()
        {
            var assignments = new List<Assignment>();
            repository.Query(Arg.Compat.Any<QueryModel>()).Returns(assignments);

            // Act
            var queryModel = new QueryModel
            {
                OWPMatchId = 1
            };
            var result = await controller.Query(queryModel);

            // Assert
            Assert.Equal(assignments, result);
        }

        [Fact]
        public async Task Validate_WhenCalled_ReturnValidationResult()
        {
            // Arrange
            var expected = new ValidateResult
            {
                Results = new List<ValidateEntity>
                {
                    new ValidateEntity {ResultType = 2, Message= "Invalid" }
                }
            };

            repository.Validate(Arg.Compat.Any<ValidateModel>()).Returns(expected);

            // Act
            var actual = await controller.Validate(new ValidateModel());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Create_NotifyMessage_WhenResultIsSuccessful()
        {
            // Arrange
            var model = new CreateModel
            {
                AssignmentNote = "",
                AssignmentType = 1,
                BetType = 1,
                OWPMatchId = 1,
                UserId = 1,
                Username = "sa"
            };

            var commandResult = new CommandResult
            {
                Code = 0,
                SessionId = "123"
            };

            repository.Create(Arg.Compat.Any<CreateModel>()).Returns(Task.FromResult(commandResult));

            // Act
            var result = await controller.Create(model);

            // Assert
            Assert.Equal(commandResult, result);
            //await notificationService.Received().Notify(Arg.Compat.Is("123"));
        }

        [Fact]
        public async Task Create_DoNotNotifyMessage_WhenResultIsFailure()
        {
            // Arrange
            var model = new CreateModel();
            var commandResult = new CommandResult
            {
                Code = 1,
                Message = "Failure"
            };

            repository.Create(Arg.Compat.Any<CreateModel>()).Returns(Task.FromResult(commandResult));

            // Act
            var result = await controller.Create(model);

            // Assert
            Assert.Equal(commandResult.IsSuccessful, result.IsSuccessful);
            //await notificationService.DidNotReceive().Notify(Arg.Compat.Any<string>());
        }

        [Fact]
        public async Task Update_NotifyMessage_WhenResultIsSuccessful()
        {
            // Arrange
            var model = new UpdateModel
            {
                AssignmentNote = "",
                AssignmentType = 1,
                BetType = 1,
                OWPMatchId = 1,
                UserId = 1,
                Username = "sa",
                AssignmentId = 1
            };

            var commandResult = new CommandResult
            {
                Code = 0,
                SessionId = "123"
            };

            repository.Update(Arg.Compat.Any<UpdateModel>()).Returns(Task.FromResult(commandResult));

            // Act
            var result = await controller.Update(model);

            // Assert
            Assert.Equal(commandResult, result);
            //await notificationService.Received().Notify(Arg.Compat.Is("123"));
        }

        [Fact]
        public async Task Update_DoNotNotifyMessage_WhenResultIsFailure()
        {
            // Arrange
            var model = new UpdateModel();
            var commandResult = new CommandResult
            {
                Code = 1
            };

            repository.Update(Arg.Compat.Any<UpdateModel>()).Returns(Task.FromResult(commandResult));

            // Act
            var result = await controller.Update(model);

            // Assert
            Assert.Equal(commandResult.IsSuccessful, result.IsSuccessful);
            //await notificationService.DidNotReceive().Notify(Arg.Compat.Any<string>());
        }

        [Fact]
        public async Task Delete_NotifyMessage_WhenResultIsSuccessful()
        {
            // Arrange
            var model = new DeleteModel
            {
                AssignmentType = 1,
                BetType = 1,
                OWPMatchId = 1,
                AssignmentId = 1
            };

            var commandResult = new CommandResult
            {
                Code = 0,
                SessionId = "123"
            };

            repository.Delete(Arg.Compat.Any<DeleteModel>()).Returns(Task.FromResult(commandResult));

            // Act
            var result = await controller.Delete(model);

            // Assert
            Assert.Equal(commandResult, result);
            //await notificationService.Received().Notify(Arg.Compat.Is("123"));
        }

        [Fact]
        public async Task Delete_DoNotNotifyMessage_WhenResultIsFailure()
        {
            // Arrange
            var model = new DeleteModel();
            var commandResult = new CommandResult
            {
                Code = 1
            };

            repository.Delete(Arg.Compat.Any<DeleteModel>()).Returns(Task.FromResult(commandResult));

            // Act
            var result = await controller.Delete(model);

            // Assert
            Assert.Equal(commandResult.IsSuccessful, result.IsSuccessful);
            //await notificationService.DidNotReceive().Notify(Arg.Compat.Any<string>());
        }
    }
}