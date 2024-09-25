using Fanex.Data.Repository;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Client.Services;
using NSubstitute;
using OWP.Models;
using OWP.Services;
using OWP.WorkingShift.Common.Models;
using OWP.WorkingShift.Common.Repositories;
using OWP.WorkingShift.WeeklyWorkingShift.Criteria;
using OWP.WorkingShift.WeeklyWorkingShift.Models;
using OWP.WorkingShift.WeeklyWorkingShift.Repositories;
using Xunit;

namespace OWP.WorkingShift.UnitTests.WeeklyWorkingShift.Repositories;

public class RepositoryUnitTests
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSession currentSession;
    private readonly Repository repository;
    private readonly WorkingShiftColorRepository workingShiftColorRepository;
    private readonly WorkingShiftPublishRepository workingShiftPublishRepository;
    private readonly WorkingShiftRepository workingShiftRepository;
    private readonly UserRepository userRepository;

    public RepositoryUnitTests()
    {
        dynamicRepository = Substitute.For<IDynamicRepository>();
        currentSession = Substitute.For<INapSession>();
        workingShiftColorRepository = Substitute.For<WorkingShiftColorRepository>();
        workingShiftPublishRepository = Substitute.For<WorkingShiftPublishRepository>();
        var workingShiftDefaultRepository = Substitute.For<WorkingShiftDefaultRepository>();
        workingShiftRepository = Substitute.For<WorkingShiftRepository>();
        userRepository = Substitute.For<UserRepository>();
        var settings = Substitute.For<INapSettings>();
        var userService = Substitute.For<IUserService>();
        currentSession.UserId.Returns(99);
        repository = new RepositoryImp(dynamicRepository,
            workingShiftColorRepository,
            workingShiftDefaultRepository,
            workingShiftPublishRepository,
            workingShiftRepository,
            currentSession,
            userRepository,
            settings,
            userService);
    }

    [Fact]
    public async Task SearchWorkingShiftUser_WhenCalled_ReturnListWorkingShiftUserModel()
    {
        var fromDate = new DateTime(2019, 06, 10);
        var toDate = new DateTime(2019, 06, 16);

        var model = new QueryModel
        {
            FromDate = fromDate,
            ToDate = toDate,
            User = "",
            Section = "1",
            Category = "",
            Location = "",
            ExcludeClosedAccount = true,
            OrderUserId = 1,
            IsFrame = false,
            TimeZoneOffset = 0,
            Permission = new WorkingShiftPermission
            {
                WorkingShiftManagementAllSections = new PermissionModel(false, false)
            }
        };

        // Arrange
        var expected = new List<WorkingShiftUserModel>
        {
            new()
            {
                UserId =currentSession.UserId,
                SectionId =0,
                Username ="sa",
                Nickname ="Sa",
                Address ="sa",
                Phone ="123",
                OrderId=1,
                WorkingShifts = [new  WorkingShiftModel { UserId=1, ShiftId=1 }]
            }
        };

        dynamicRepository.FetchAsync<WorkingShiftUserModel>(Arg.Compat.Any<SearchEmployeeWorkingShiftCriteria>()).Returns(expected);

        var mockUser = new User
        {
            SectionId = 0,
            UserId = 99
        };

        userRepository.GetUserInfoById(currentSession.UserId).Returns(mockUser);
        // Act
        var actual = await repository.SearchWorkingShiftUser(model);

        // Assert
        Assert.True(actual.Count > 0);
    }

    [Fact]
    public async Task SearchWorkingShiftWeekly_IsIFrame_WhenCalled_ReturnQueryResult()
    {
        var fromDate = new DateTime(2019, 06, 10);
        var toDate = new DateTime(2019, 06, 16);

        var model = new QueryModel
        {
            FromDate = fromDate,
            ToDate = toDate,
            User = "",
            Section = "1",
            Category = "",
            Location = "",
            ExcludeClosedAccount = true,
            OrderUserId = 1,
            IsFrame = true,
            TimeZoneOffset = 0,
            Permission = new WorkingShiftPermission
            {
                WorkingShiftManagementAllSections = new PermissionModel(true, true)
            }
        };

        // Arrange
        var mockListUserModel = new List<WorkingShiftUserModel>
        {
            new()
            {
                UserId =currentSession.UserId, SectionId=3, Nickname="Sa", Address="sa", Phone="123", Username="sa",
                SectionName="est", OrderId=1,
                WorkingShifts = [new  WorkingShiftModel { UserId=1, ShiftId=1 }]
            }
        };
        var mockSectionPubslishs = new List<SectionPublishWorkingShiftModel>
        {
            new() {SectionId=3, FromDate=model.FromDate, ToDate=model.ToDate, IsPublished=true}
        };

        workingShiftPublishRepository.GetSectionPublishWorkingShift(model.FromDate, model.ToDate, model.Section).Returns(mockSectionPubslishs);

        dynamicRepository.FetchAsync<WorkingShiftUserModel>(Arg.Compat.Any<SearchEmployeeWorkingShiftCriteria>()).Returns(mockListUserModel);

        var mockUser = new User
        {
            SectionId = 3,
            UserId = 99
        };

        userRepository.GetUserInfoById(currentSession.UserId).Returns(mockUser);
        // Act
        var mockWorkingShiftColorModel = new List<WorkingShiftColorModel>
        {
            new() { ShiftId=1}
        };
        workingShiftColorRepository.GetAllByTimeZone(model.TimeZoneOffset).Returns(mockWorkingShiftColorModel);

        var mockWokingShiftModel = new List<WorkingShiftModel>();
        workingShiftRepository.QueryByUser(Arg.Compat.Any<WorkingShiftQueryByUserModel>()).Returns(mockWokingShiftModel);

        await repository.SearchWorkingShiftWeekly(model);

        // Assert
    }

    [Fact]
    public async Task SearchWorkingShiftWeekly_NoneIFrame_WhenCalled_ReturnQueryResult()
    {
        var fromDate = new DateTime(2019, 06, 10);
        var toDate = new DateTime(2019, 06, 16);

        var model = new QueryModel
        {
            FromDate = fromDate,
            ToDate = toDate,
            User = "",
            Section = "1",
            Category = "",
            Location = "",
            ExcludeClosedAccount = true,
            OrderUserId = 1,
            IsFrame = false,
            TimeZoneOffset = 0,
            Permission = new WorkingShiftPermission
            {
                WorkingShiftManagementAllSections = new PermissionModel(true, true)
            }
        };

        // Arrange
        var mockListUserModel = new List<WorkingShiftUserModel>
        {
            new()
            {
                UserId =currentSession.UserId, SectionId=3, Nickname="Sa", Address="sa", Phone="123", Username="sa",
                SectionName="est", OrderId=1,
                WorkingShifts = [new  WorkingShiftModel { UserId=1, ShiftId=1 }]
            }
        };
        var mockSectionPubslishs = new List<SectionPublishWorkingShiftModel>
        {
            new() {SectionId=3, FromDate=model.FromDate, ToDate=model.ToDate, IsPublished=true}
        };

        workingShiftPublishRepository.GetSectionPublishWorkingShift(model.FromDate, model.ToDate, model.Section).Returns(mockSectionPubslishs);

        dynamicRepository.FetchAsync<WorkingShiftUserModel>(Arg.Compat.Any<SearchEmployeeWorkingShiftCriteria>()).Returns(mockListUserModel);

        var mockUser = new User
        {
            SectionId = 3,
            UserId = 99
        };

        userRepository.GetUserInfoById(currentSession.UserId).Returns(mockUser);
        // Act
        var mockWorkingShiftColorModel = new List<WorkingShiftColorModel>
        {
            new() { ShiftId=1}
        };
        workingShiftColorRepository.GetAllByTimeZone(model.TimeZoneOffset).Returns(mockWorkingShiftColorModel);

        var mockWokingShiftModel = new List<WorkingShiftModel>();
        workingShiftRepository.QueryByUser(Arg.Compat.Any<WorkingShiftQueryByUserModel>()).Returns(mockWokingShiftModel);

        await repository.SearchWorkingShiftWeekly(model);

    }
}