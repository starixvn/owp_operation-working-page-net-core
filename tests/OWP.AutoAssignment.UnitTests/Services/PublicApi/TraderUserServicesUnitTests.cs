using NSubstitute;
using OWP.AutoAssignment.Models.PublicApi;
using OWP.AutoAssignment.Services.PublicApi;
using Xunit;

namespace OWP.AutoAssignment.UnitTests.Services.PublicApi;

using Xunit.Asserts.Compare;

public class TraderUserServicesUnitTests
{
    private readonly Repository repository;

    public TraderUserServicesUnitTests()
    {
        repository = Substitute.For<Repository>();
    }

    [Fact]
    public async Task GetLAUsersInfo_EventDate_ReturnUserWorkingShift()
    {
        MockRepository();

        // Act
        var service = new TraderUserServiceImp(repository);
        var actual = await service.GetTradersInfo(DateTime.Parse("2021-12-08"));

        var expected = new List<SchedulingTraderInfo>
        {
            new()
            {
                Level=1,
                BettypesPreferences="Booking",
                MaxGames=1,
                MultipleGames="1G",
                Role="Employee",
                TraderId=8,
                WorkingShifts=
                [
                    new SchedulingWorkingShift
                    {
                        InTime= "2021-12-08T00:00:00",
                        OutTime="2021-12-09T00:00:00"
                    }
                ]
            }
        };
        // Assert
        DeepAssert.Equal(expected, actual);
    }

    private void MockRepository()
    {
        // Arrange
        var traderInfo = new List<SchedulingTraderInfo>
        {
            new()
            {
                TraderId =8,
                Level =1,
                Role = "Employee",
                BettypesPreferences ="Booking",
                MultipleGames="1G",
                MaxGames =3
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
        var userInfo = new ValueTuple<IEnumerable<SchedulingTraderInfo>, IEnumerable<UserWorkingShift>>(traderInfo, workingShift);

        repository.GetTraders(Arg.Compat.Any<DateTime>()).ReturnsForAnyArgs(_ => userInfo);
        repository.GetUserInfoFromNAPUserControl().ReturnsForAnyArgs(_ => new List<int> { 8 });
        repository.GetCustInfoFromDB02().ReturnsForAnyArgs(_ => new List<int> { 8 });
    }
}