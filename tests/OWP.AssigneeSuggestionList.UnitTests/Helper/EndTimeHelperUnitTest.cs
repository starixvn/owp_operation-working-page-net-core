using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using Xunit;

namespace OWP.AssigneeSuggestionList.UnitTests.Helper;

public class EndTimeHelperUnitTest
{

    [Fact]
    public void Update_ForSaBaMatch_ReturnExpect()
    {
        var assignment = new List<AssignmentModel>
        {
            new () { IsMainMatch = true, EventDateTime = new DateTime(2023, 4, 14, 23, 0, 0),LeagueName = "saba "},

        };

        var actual = EndTimeHelper.Update(assignment);

        Assert.Equal(new DateTime(2023, 4, 15, 0, 0, 0), actual[0].EndTime);
    }

    [Fact]
    public void Update_ForFtMatch_ReturnExpect()
    {
        var assignment = new List<AssignmentModel>
        {
            new () { IsMainMatch = true, EventDateTime = new DateTime(2023, 4, 14, 10, 0, 0),BettypeId = 1},
            new () { IsMainMatch = true, EventDateTime = new DateTime(2023, 4, 14, 10, 0, 0),BettypeId = 1,Remark = "aPKaa"},
            new () { IsMainMatch = true, EventDateTime = new DateTime(2023, 4, 14, 10, 0, 0),BettypeId = 1,Remark = "aPKETaa"},
            new () { IsMainMatch = true, EventDateTime = new DateTime(2023, 4, 14, 10, 0, 0),BettypeId = 7},
        };

        var actual = EndTimeHelper.Update(assignment);

        Assert.Equal(new DateTime(2023, 4, 14, 12, 0, 0), actual[0].EndTime);
        Assert.Equal(new DateTime(2023, 4, 14, 12, 30, 0), actual[1].EndTime);
        Assert.Equal(new DateTime(2023, 4, 14, 13, 00, 0), actual[2].EndTime);
        Assert.Equal(new DateTime(2023, 4, 14, 11, 00, 0), actual[3].EndTime);
    }

    [Fact]
    public void Update_ForSpecialMatch_ReturnExpect()
    {
        var assignment = new List<AssignmentModel>
        {
            new () { IsMainMatch = false, EventDateTime = new DateTime(2023, 4, 14, 10, 0, 0), BettypeId = 1, MinorBettypeId = -3, Remark = "ETPK"},
            new () { IsMainMatch = false, EventDateTime = new DateTime(2023, 4, 14, 10, 0, 0), BettypeId = -100},
        };

        var actual = EndTimeHelper.Update(assignment);

        Assert.Equal(new DateTime(2023, 4, 14, 13, 0, 0), actual[0].EndTime);
        Assert.Equal(new DateTime(2023, 4, 14, 12, 0, 0), actual[1].EndTime);
    }
}
