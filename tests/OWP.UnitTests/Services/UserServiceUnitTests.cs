using Fanex.Data.Repository;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Services;
using NSubstitute;
using Newtonsoft.Json;
using OWP.Criteria;
using OWP.Models;
using OWP.Services;
using Xunit;

namespace OWP.UnitTests.Services;

public class UserServiceUnitTests
{
    private readonly IDynamicRepository repository;
    private readonly IUserService userService;
    private readonly INapSettings napSettings;
    private readonly UserRepository userRepository;

    public UserServiceUnitTests()
    {
        repository = Substitute.For<IDynamicRepository>();
        userService = Substitute.For<IUserService>();
        napSettings = Substitute.For<INapSettings>();
        userRepository = new UserRepositoryImp(repository, userService, napSettings);
    }

    [Fact]
    public async Task GetUserInfoById_ShouldCallRepositoryWithCorrectCriteria()
    {
        // Arrange
        var userId = 123;
        var expectedUser = new User { UserId = userId, UserName = "Test User" };

        repository.GetAsync<User>(Arg.Any<GetUserInfoByIdCriteria>())
            .Returns(expectedUser);

        // Act
        var result = await userRepository.GetUserInfoById(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUser, result);
        Assert.Equal(userId, result.UserId);

        await repository.Received(1).GetAsync<User>(
            Arg.Is<GetUserInfoByIdCriteria>(x => x.UserId == userId));
    }

    [Fact]
    public async Task GetUserInfoById_WithZeroUserId_ShouldCallRepositoryWithZeroUserId()
    {
        // Arrange
        var userId = 0;
        var expectedUser = new User { UserId = userId, UserName = "Test User" };

        repository.GetAsync<User>(Arg.Any<GetUserInfoByIdCriteria>())
            .Returns(expectedUser);

        // Act
        var result = await userRepository.GetUserInfoById(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUser, result);

        await repository.Received(1).GetAsync<User>(
            Arg.Is<GetUserInfoByIdCriteria>(x => x.UserId == userId));
    }

    [Fact]
    public async Task GetAllUsers_ShouldCallUserServiceAndRepositoryWithCorrectParameters()
    {
        // Arrange
        var siteId = 1;
        var activeUsers = new List<Fanex.NAP.Client.Models.User>
        {
            new() { Id = 1, Name = "User 1" },
            new() { Id = 2, Name = "User 2" }
        };

        var expectedWeeklyUsers = new List<WeeklyWorkingShiftUser>
        {
            new() { UserId = 1, IsSecondAccount = true },
            new() { UserId = 2, IsSecondAccount = true }
        };

        napSettings.SiteId.Returns(siteId);
        userService.GetBySiteAsync(siteId, true).Returns(activeUsers);
        repository.FetchAsync<WeeklyWorkingShiftUser>(Arg.Any<GetAllUserCriteria>())
            .Returns(expectedWeeklyUsers);

        // Act
        var result = await userRepository.GetAllUsers();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(expectedWeeklyUsers, result);

        await userService.Received(1).GetBySiteAsync(siteId, true);
        await repository.Received(1).FetchAsync<WeeklyWorkingShiftUser>(
            Arg.Is<GetAllUserCriteria>(x =>
                x.AllUserJson.Contains("UserId") &&
                x.AllUserJson.Contains("IsActive")));
    }

    [Fact]
    public async Task GetAllUsers_WithEmptyUserList_ShouldReturnEmptyList()
    {
        // Arrange
        var siteId = 1;
        var activeUsers = new List<Fanex.NAP.Client.Models.User>();

        napSettings.SiteId.Returns(siteId);
        userService.GetBySiteAsync(siteId, true).Returns(activeUsers);
        repository.FetchAsync<WeeklyWorkingShiftUser>(Arg.Any<GetAllUserCriteria>())
            .Returns(new List<WeeklyWorkingShiftUser>());

        // Act
        var result = await userRepository.GetAllUsers();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        await userService.Received(1).GetBySiteAsync(siteId, true);
        await repository.Received(1).FetchAsync<WeeklyWorkingShiftUser>(
            Arg.Is<GetAllUserCriteria>(x => x.AllUserJson == "[]"));
    }

    [Fact]
    public async Task GetAllUsers_ShouldSerializeUserDataCorrectly()
    {
        // Arrange
        var siteId = 1;
        var activeUsers = new List<Fanex.NAP.Client.Models.User>
        {
            new() { Id = 1, Name = "User 1" },
            new() { Id = 2, Name = "User 2" }
        };

        var expectedWeeklyUsers = new List<WeeklyWorkingShiftUser>();

        napSettings.SiteId.Returns(siteId);
        userService.GetBySiteAsync(siteId, true).Returns(activeUsers);
        repository.FetchAsync<WeeklyWorkingShiftUser>(Arg.Any<GetAllUserCriteria>())
            .Returns(expectedWeeklyUsers);

        // Act
        await userRepository.GetAllUsers();

        // Assert
        await repository.Received(1).FetchAsync<WeeklyWorkingShiftUser>(
            Arg.Is<GetAllUserCriteria>(x =>
                x.AllUserJson.Contains("\"UserId\":1") &&
                x.AllUserJson.Contains("\"UserId\":2") &&
                x.AllUserJson.Contains("\"IsActive\":true")));
    }

    [Fact]
    public async Task GetAllUsers_WithNullUserService_ShouldThrowException()
    {
        // Arrange
        var userRepositoryWithNullService = new UserRepositoryImp(repository, null, napSettings);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() =>
            userRepositoryWithNullService.GetAllUsers());
    }

    [Fact]
    public async Task GetAllUsers_WithNullNapSettings_ShouldThrowException()
    {
        // Arrange
        var userRepositoryWithNullSettings = new UserRepositoryImp(repository, userService, null);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() =>
            userRepositoryWithNullSettings.GetAllUsers());
    }
}
