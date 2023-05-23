using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using Xunit;

namespace OWP.AssigneeSuggestionList.UnitTests.Helper;
public class SortAssigneesHelperUnitTest
{
    [Fact]
    public void SortByRole_SortBySoloMatch_ReturnExpect()
    {
        var workingShifts = MockWorkingSchedules();

        var sortWorkingShifts = OrderAssigneeHelper.SortAssignee(new MatchInfo { IsSoloMatch = true, MinorBettypeId = null, BettypeId = 3 }, workingShifts);

        Assert.Equal("leader", sortWorkingShifts.First().UserInfo.Role);
        Assert.Equal(new DateTime(2023, 4, 22), sortWorkingShifts.First().UserInfo.ShiftStartTime);
        Assert.Null(sortWorkingShifts.Last().UserInfo.LevelLabel);
    }

    [Fact]
    public void SortByRole_SortByMainMatch_ReturnExpect()
    {
        var workingShifts = MockWorkingSchedules();

        var sortWorkingShifts = OrderAssigneeHelper.SortAssignee(new MatchInfo { IsSoloMatch = false, MaxbetHDP = 60_000, MinorBettypeId = null, BettypeId = 3 }, workingShifts);

        Assert.Equal("leader", sortWorkingShifts.First().UserInfo.Role);

    }

    [Fact]
    public void SortByRole_SortByChildMatch_ReturnExpect()
    {
        var workingShifts = MockWorkingSchedules();

        var sortWorkingShifts = OrderAssigneeHelper.SortAssignee(new MatchInfo { IsSoloMatch = false, MinorBettypeId = 123, BettypeId = 3 }, workingShifts);

        Assert.Equal("leader", sortWorkingShifts.First().UserInfo.Role);

    }

    [Fact]
    public void SortByRole_SortByMainMatchWithMaxbetLower35K_ReturnExpect()
    {
        var workingShifts = MockWorkingSchedules();

        var sortWorkingShifts = OrderAssigneeHelper
            .SortAssignee(new MatchInfo { IsSoloMatch = false, MaxbetHDP = 34_000, MinorBettypeId = null, BettypeId = 1 }, workingShifts);

        Assert.Equal("staff", sortWorkingShifts.First().UserInfo.Role);

    }

    [Fact]
    public void SortByRole_SortByMainMatchWithMaxbetUpper80K_ReturnExpect()
    {
        var workingShifts = MockWorkingSchedules();

        var sortWorkingShifts = OrderAssigneeHelper
            .SortAssignee(new MatchInfo { IsSoloMatch = false, MaxbetHDP = 85_000, MinorBettypeId = null, BettypeId = 1 }, workingShifts);

        Assert.Equal("leader", sortWorkingShifts.First().UserInfo.Role);

    }

    public List<WorkingSchedule> MockWorkingSchedules()
        => new()
            {
                new() { UserInfo = new() { LevelLabel = "2nd", Role = "management" ,MainBetType = "", ShiftStartTime = new DateTime(2023,5,22)}},
                new() { UserInfo = new() { LevelLabel = "1st", Role = "leader" , MainBetType = "2", ShiftStartTime = new DateTime(2023,4,22)}},
                new() { UserInfo = new() { LevelLabel = null, Role = "supervisor" , MainBetType = "2[OU]", ShiftStartTime = new DateTime(2023,3,22)}},
                new() { UserInfo = new() { LevelLabel = "1st", Role = "staff" , MainBetType = null, ShiftStartTime = new DateTime(2023,2,22)}},
                new() { UserInfo = new() { LevelLabel = "1st", Role = "supervisor" , MainBetType = null, ShiftStartTime = new DateTime(2023,1,22)}}
            };
}
