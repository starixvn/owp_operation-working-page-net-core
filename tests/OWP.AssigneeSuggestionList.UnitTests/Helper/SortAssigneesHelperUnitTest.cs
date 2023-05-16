using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using Xunit;

namespace OWP.AssigneeSuggestionList.UnitTests.Helper;
public class SortAssigneesHelperUnitTest
{
    [Fact]
    public void SortLevelByType_Always_ReturnExpect()
    {
        var workingShifts = new List<WorkingSchedule>
        {
            new ()
            {
                UserInfo = new (){ LevelLabel = "2nd"}
            },
            new ()
            {
                UserInfo = new (){ LevelLabel = "1st"}
            },
            new ()
            {
                UserInfo = new (){ LevelLabel = "none"}
            }
        };

        SortAssigneesHelper.SortByLevel(workingShifts, true);

        Assert.Equal("1st", workingShifts.First().UserInfo.LevelLabel);
    }

    [Fact]
    public void SortByRole_Always_ReturnExpect()
    {
        var workingShifts = new List<WorkingSchedule>
        {
            new ()
            {
                UserInfo = new (){ Role = "management"}
            },
            new ()
            {
                UserInfo = new (){ Role = "staff"}
            },
            new ()
            {
                UserInfo = new (){ Role = "supervisor"}
            }
        };

        SortAssigneesHelper.SortByRole(workingShifts, OrderConstant.RoleOrder);

        Assert.Equal("staff", workingShifts.First().UserInfo.Role);
    }

}
