using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services;
using OWP.AssigneeSuggestionList.Services.RealMatch;
using Xunit;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.UnitTests.Services;
public class SingleGameGroupServiceUnitTests
{

    [Fact]
    public void Calculate_Group_ReturnMatchTypeDefault()
    {
        SingleGameGroupService singleGameGroupService = new SingleGameGroupServiceImp(new WorkingScheduleServiceImp());
        var assignment = new List<AssignmentModel> { new() { OWPMatchId = 1 } };
        var schedule = new WorkingSchedule();
        Func<IList<AssignmentModel>, ScheduleUpdateModel> buildUpdateScheduleFunc = (list) => new ScheduleUpdateModel();

        singleGameGroupService.Group(assignment, schedule, buildUpdateScheduleFunc);

        Assert.Equal(MatchType.RealMatchOther, assignment[0].MatchType);
    }

    [Fact]
    public void Calculate_Group_ReturnExpectMatchType()
    {
        SingleGameGroupService singleGameGroupService = new SingleGameGroupServiceImp(new WorkingScheduleServiceImp());
        var assignment = new List<AssignmentModel> { new() { OWPMatchId = 1 } };
        var schedule = new WorkingSchedule();
        Func<IList<AssignmentModel>, ScheduleUpdateModel> buildUpdateScheduleFunc = (list) => new ScheduleUpdateModel();

        singleGameGroupService.Group(assignment, schedule, buildUpdateScheduleFunc, MatchType.RealMatchLink1G);

        Assert.Equal(MatchType.RealMatchLink1G, assignment[0].MatchType);
    }
}
