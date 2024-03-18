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
        SpecialGroupService singleGameGroupService = new SpeacialGroupServiceImp(new WorkingScheduleServiceImp());
        var assignment = new List<AssignmentModel> { new() { OWPMatchId = 1 } };
        var schedule = new WorkingSchedule();
        ScheduleUpdateModel BuildUpdateScheduleFunc(IList<AssignmentModel> _) => new();

        singleGameGroupService.Group(assignment, schedule, BuildUpdateScheduleFunc);

        Assert.Equal(MatchType.RealMatchOther, assignment[0].MatchType);
    }

    [Fact]
    public void Calculate_Group_ReturnExpectMatchType()
    {
        SpecialGroupService singleGameGroupService = new SpeacialGroupServiceImp(new WorkingScheduleServiceImp());
        var assignment = new List<AssignmentModel> { new() { OWPMatchId = 1 } };
        var schedule = new WorkingSchedule();
        ScheduleUpdateModel BuildUpdateScheduleFunc(IList<AssignmentModel> _) => new();

        singleGameGroupService.Group(assignment, schedule, BuildUpdateScheduleFunc, MatchType.RealMatchLink1G);

        Assert.Equal(MatchType.RealMatchLink1G, assignment[0].MatchType);
    }
}
