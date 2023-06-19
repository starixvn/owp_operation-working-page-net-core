using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using Xunit;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.UnitTests.Helper;
public class SortAssigneesHelperUnitTest
{
    [Fact]
    public void SortByRole_SortBySoloMatch_ReturnExpect()
    {
        var workingShifts = MockWorkingSchedules();

        var sortWorkingShifts = OrderAssigneeHelper.SortAssignee(new MatchInfo { IsSoloMatch = true, MinorBettypeId = null, BettypeId = 3 }, workingShifts);

        Assert.Equal("leader", sortWorkingShifts.First().UserInfo.Role);
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

        Assert.Equal("leader", sortWorkingShifts.First().UserInfo.Role);

    }

    [Fact]
    public void SortByRole_SortByMainMatchWithMaxbetUpper80K_ReturnExpect()
    {
        var workingShifts = MockWorkingSchedules();

        var sortWorkingShifts = OrderAssigneeHelper
            .SortAssignee(new MatchInfo { IsSoloMatch = false, MaxbetHDP = 85_000, MinorBettypeId = null, BettypeId = 1 }, workingShifts);

        Assert.Equal("leader", sortWorkingShifts.First().UserInfo.Role);

    }

    [Fact]
    public void SortByWorkLoad_SortByMainMatchWithMaxbetUpper80K_ReturnExpect()
    {
        var workingShifts = MockWorkingSchedulesForWorkLoad();

        var sortWorkingShifts = OrderAssigneeHelper
            .SortAssignee(new MatchInfo { IsSoloMatch = false, KOTime = new DateTime(2023, 5, 22, 15, 0, 0), MaxbetHDP = 85_000, MinorBettypeId = null, BettypeId = 1 }, workingShifts);

        Assert.Equal(1, sortWorkingShifts.First().UserInfo.UserId);
        Assert.Equal(5, sortWorkingShifts.Last().UserInfo.UserId);
        Assert.Equal("1,2,3,4,5", string.Join(",", sortWorkingShifts.Select(x => x.UserInfo.UserId)));
    }

    public List<WorkingSchedule> MockWorkingSchedules()
        => new()
            {
                new() { UserInfo = new() { LevelLabel = "2nd", Role = "management" ,MainBetType = "", ShiftStartTime = new DateTime(2023,5,22)}},
                new() { UserInfo = new() { LevelLabel = "1st", Role = "leader" , MainBetType = "2", ShiftStartTime = new DateTime(2023,5,22)}},
                new() { UserInfo = new() { LevelLabel = null, Role = "supervisor" , MainBetType = "2[OU]", ShiftStartTime = new DateTime(2023,5,22)}},
                new() { UserInfo = new() { LevelLabel = "1st", Role = "staff" , MainBetType = null, ShiftStartTime = new DateTime(2023,5,22)}},
                new() { UserInfo = new() { LevelLabel = "1st", Role = "supervisor" , MainBetType = null, ShiftStartTime = new DateTime(2023,5,22)}}
            };

    public List<WorkingSchedule> MockWorkingSchedulesForWorkLoad()
        => new()
        {
            new() {
                UserInfo = new() { LevelLabel = "2nd",UserId = 4,Role = "management" ,MainBetType = "", ShiftStartTime = new DateTime(2023,5,22), ShiftEndTime =new DateTime(2023,5,23) },
                CategorizedAssignments = new List<AssignmentModel>{new ()
                {
                    MatchType = MatchType.RealMatchLink3G,
                    EventDateTime = new DateTime(2023,5,22,14,0,0),
                    EndTime = new DateTime(2023,5,22,16,0,0),
                } }},
            new() {
                UserInfo = new() { LevelLabel = "2nd",UserId = 5,Role = "management" ,MainBetType = "", ShiftStartTime = new DateTime(2023,5,24), ShiftEndTime =new DateTime(2023,5,25) },
                CategorizedAssignments = new List<AssignmentModel>{new ()
                {
                    MatchType = MatchType.RealMatchLinkError,
                    EventDateTime = new DateTime(2023,5,22,14,0,0),
                    EndTime = new DateTime(2023,5,22,16,0,0),
                } }},
            new() {
                UserInfo = new() { LevelLabel = "2nd",UserId = 2,Role = "management" ,MainBetType = "", ShiftStartTime = new DateTime(2023,5,22), ShiftEndTime =new DateTime(2023,5,23) },
                CategorizedAssignments = new List<AssignmentModel>{new ()
                {
                    MatchType = MatchType.RealMatchLink1G,
                    EventDateTime = new DateTime(2023,5,22,14,0,0),
                    EndTime = new DateTime(2023,5,22,16,0,0),
                } }},
            new() {
                UserInfo = new() { LevelLabel = "2nd",UserId = 1,Role = "management" ,MainBetType = "", ShiftStartTime = new DateTime(2023,5,22), ShiftEndTime =new DateTime(2023,5,23) },
                CategorizedAssignments = new List<AssignmentModel>{new ()
                {
                    MatchType = MatchType.RealMatchLink1G,
                    EventDateTime = new DateTime(2023,5,22,12,0,0),
                    EndTime = new DateTime(2023,5,22,13,0,0),
                } }},

            new() {
                UserInfo = new() { LevelLabel = "2nd",UserId = 3,Role = "management" ,MainBetType = "", ShiftStartTime = new DateTime(2023,5,22), ShiftEndTime =new DateTime(2023,5,23) },
                CategorizedAssignments = new List<AssignmentModel>{new ()
                {
                    MatchType = MatchType.RealMatchLink2G,
                    EventDateTime = new DateTime(2023,5,22,14,0,0),
                    EndTime = new DateTime(2023,5,22,16,0,0),
                } }}
        };
}
