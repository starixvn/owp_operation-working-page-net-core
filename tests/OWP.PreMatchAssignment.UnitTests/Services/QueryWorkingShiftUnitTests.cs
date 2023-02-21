using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using NSubstitute;
using OWP.PrematchAssignment.Criteria;
using OWP.PrematchAssignment.Models;
using OWP.PrematchAssignment.Services;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.PreMatchAssignment.UnitTests.Services;

public class QueryWorkingShiftUnitTests
{
    private readonly IDynamicRepository dynamicRepository;

    private readonly QueryWorkingShift queryWorkingShift;

    private readonly INapSession currentSession;

    public QueryWorkingShiftUnitTests()
    {
        dynamicRepository = Substitute.For<IDynamicRepository>();

        currentSession = Substitute.For<INapSession>();

        queryWorkingShift = new QueryWorkingShiftImp(dynamicRepository, currentSession);
    }

    [Fact]
    public async Task QueryWorkingShift_NoData_ReturnEmptyWorkingShift()
    {
        var EventDate = DateTime.Parse("2020-08-16");

        var getWorkingShiftList = new List<WorkingShiftModel>();

        var expected = new List<WorkingShift>();

        dynamicRepository.FetchAsync<WorkingShiftModel>(Arg.Compat.Any<GetWorkingShiftByDateCriteria>()).Returns(getWorkingShiftList);

        // Act
        var actual = await queryWorkingShift.QueryWorkingShift(EventDate);

        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public async Task QueryWorkingShift_AvailableData_ReturnWorkingShift()
    {
        var EventDate = DateTime.Parse("2020-08-16");

        var getWorkingShiftList = ListWorkingShift();
        var expected = new List<WorkingShift>
            {
                new()
                {
                    Shifts = "00 > 05 (REAL)",
                    Usernames = "esther, curry"
                },

                new()
                {
                    Shifts = "OFF",
                    Usernames = "ella"
                }
            };

        dynamicRepository.FetchAsync<WorkingShiftModel>(Arg.Compat.Any<GetWorkingShiftByDateCriteria>()).Returns(getWorkingShiftList);

        // Act
        var actual = await queryWorkingShift.QueryWorkingShift(EventDate);

        DeepAssert.Equal(expected, actual);
    }

    public List<WorkingShiftModel> ListWorkingShift()
    {
        var workingShift = new WorkingShiftModel
        {
            NickName = "esther",
            UserId = 123,
            StartTime = new TimeSpan(0),
            EndTime = new TimeSpan(5, 00, 00),
            Extension = 1,
            IsTimeShift = true
        };

        var workingShift2 = new WorkingShiftModel
        {
            NickName = "curry",
            UserId = 234,
            StartTime = new TimeSpan(0),
            EndTime = new TimeSpan(5, 00, 00),
            Extension = 1,
            IsTimeShift = true
        };

        var workingShift3 = new WorkingShiftModel
        {
            NickName = "ella",
            UserId = 345,
            ShiftCode = "OFF",
            StartTime = null,
            EndTime = null,
            Extension = 0,
            IsTimeShift = false
        };

        var workingShiftList = new List<WorkingShiftModel> { workingShift, workingShift2, workingShift3 };
        return workingShiftList;
    }
}