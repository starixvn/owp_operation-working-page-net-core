namespace OWP.OtherSport.UnitTests.Notification.Services
{
    using NSubstitute;
    using OtherSport.Notification.Models;
    using OtherSport.Notification.Services;
    using OtherSport.Submit.Models;
    using Xunit;
    using Submit = OtherSport.Submit.Services;

    public class ChangedValueByCacheAndDatabaseUnitTests
    {
        private readonly ChangedValueByCacheAndDatabaseImp services;
        private readonly CacheManipulator cacheManipulator;
        private readonly Submit.CommandRepository commandRepo;
        private readonly ChangedValueBySnapshot changedValueBySnapshot;

        public ChangedValueByCacheAndDatabaseUnitTests()
        {
            cacheManipulator = Substitute.For<CacheManipulator>();
            commandRepo = Substitute.For<Submit.CommandRepository>();
            changedValueBySnapshot = Substitute.For<ChangedValueBySnapshot>();
            services = new ChangedValueByCacheAndDatabaseImp(cacheManipulator, commandRepo, changedValueBySnapshot);
        }

        [Fact]
        public void Create_Always_ReturnChangedValue()
        {
            // Arrange
            var changedValues = new List<ChangedValue>
            {
                new ChangedValue
                {
                    UserId = 10
                }
            };
            cacheManipulator.GetAssignmentSnapshot(Arg.Compat.Any<string>(), Arg.Compat.Any<string>()).Returns(new AssignmentSnapshot());
            commandRepo.Query(Arg.Compat.Any<string>()).Returns(new AssignmentSnapshot());
            cacheManipulator.DeleteAssignmentSnapshot(Arg.Compat.Any<string>(), Arg.Compat.Any<string>());
            changedValueBySnapshot.Create(Arg.Compat.Any<AssignmentSnapshot>(), Arg.Compat.Any<AssignmentSnapshot>()).Returns(changedValues);

            // Act
            var actual = services.Create("abc", "2021-05-13");

            // Assert
            Equals(changedValues, actual);
        }
    }
}