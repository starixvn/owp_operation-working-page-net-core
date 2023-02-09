namespace OWP.AutoAssignment.UnitTests.Services.PublicApi;

using AutoAssignment.Services.PublicApi;
using Models.PublicApi;
using NSubstitute;
using Xunit;
using Xunit.Asserts.Compare;

public class LAUserServicesUnitTests
{
    private readonly Repository repository;

    public LAUserServicesUnitTests()
    {
        repository = Substitute.For<Repository>();
    }

    [Fact]
    public async Task GetLAUsersInfo_EventDate_ReturnUserWorkingShift()
    {
        // Arrange
        var lAUserInfo = new List<LAUserInfo>
        {
            new()
            {
                UserId =8,
                Level =1,

                SpeakChinese = true,

                SpeakEnglish = true,

                Location ="SMS"
            }
        };

        var workingShift = new List<UserWorkingShift>
        {
            new()
            {
                UserId=8,
                ShiftStartTime= DateTime.Parse("2021-12-08"),
                ShiftEndTime= DateTime.Parse("2021-12-09")
            }
        };

        var userInfo = new ValueTuple<IEnumerable<LAUserInfo>, IEnumerable<UserWorkingShift>>(lAUserInfo, workingShift);

        repository
            .GetLAInfoByDate(Arg.Compat.Any<DateTime>())
            .ReturnsForAnyArgs(r => userInfo);

        // Act
        var service = new LAUserServiceImp(repository);
        var actual = await service.GetLAUsersInfo(DateTime.Parse("2021-12-08"));

        var expected = new List<SchedulingLAUser>
        {
            new()
            {
                Level=1,
                Location="SMS",
                SpeakChinese=true,
                SpeakEnglish=true,
                UserId=8,
                WorkingShifts= new[]
                {
                    new SchedulingWorkingShift
                    {
                        InTime= "2021-12-08T00:00:00",
                        OutTime="2021-12-09T00:00:00"
                    }
                }
            }
        };
        // Assert
        DeepAssert.Equal(expected, actual);
    }
}