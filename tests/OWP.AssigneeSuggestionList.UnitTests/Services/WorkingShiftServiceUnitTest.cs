using NSubstitute;
using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services;
using Xunit;

namespace OWP.AssigneeSuggestionList.UnitTests.Services;
public class WorkingShiftServiceUnitTest
{
    private readonly Repository repository;

    public WorkingShiftServiceUnitTest()
    {
        repository = Substitute.For<Repository>();
    }

    [Fact]
    public async Task Calculate_Always_ReturnCorrectWorkingSchedules()
    {
        var service = new WorkingShiftServiceImp(repository);
        repository.QueryTraderInfo(Arg.Any<DateTime>())
            .Returns(MockUserInfo());
        repository.GetCustInfoFromDB02().Returns(new List<int> { 1 });
        repository.GetUserInfoFromNAPUserControl().Returns(new List<int> { 1 });

        var actual = await service.Calculate(new DateTime(2023, 3, 28, 15, 00, 00));

        Assert.NotNull(actual);
        Assert.Equal("#BFBFBF", actual[0].Today_12h[0].Color);
        Assert.Equal("#BFBFBF", actual[0].Yesterday_10h[0].Color);
        Assert.Equal("#BFBFBF", actual[0].Yesterday_11h[0].Color);
    }

    public List<UserInfo> MockUserInfo()
        =>
        [
            new UserInfo
            {
                UserId = 1, ShiftStartTime = new DateTime(2023, 3, 28, 10, 00, 00),
                ShiftEndTime = new DateTime(2023, 3, 28, 12, 00, 00)
            }
        ];
}
