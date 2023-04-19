using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services;
using Xunit;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.UnitTests.Services;

public class CategorizeRealMatchServicesUnitTests
{
    private readonly CategorizeRealMatchServicesImp service;

    public CategorizeRealMatchServicesUnitTests()
    {
        service = new CategorizeRealMatchServicesImp();
    }

    [Fact]
    public void Categorize()
    {
        var matchInfo = new MatchInfo
        {
            OwpMatchId = 100,
            KOTime = new DateTime(2022, 04, 19, 14, 0, 0),
            EndTime = new DateTime(2022, 04, 19, 16, 0, 0),
        };

        var schedules = new List<WorkingSchedule>
        {
            new () // Busy (no ws)
            {
                UserInfo = new UserInfo
                {
                    UserId = 111,
                    ShiftStartTime = new DateTime(2022, 04, 19, 18, 0, 0),
                    ShiftEndTime = new DateTime(2022, 04, 19, 23, 0, 0),
                },
            },
            new () // Busy
            {
                UserInfo = new UserInfo
                {
                    UserId = 222,
                    ShiftStartTime = new DateTime(2022, 04, 19, 13, 0, 0),
                    ShiftEndTime = new DateTime(2022, 04, 19, 18, 0, 0),
                },
                CategorizedAssignments = new List<AssignmentModel>
                {
                    new()
                    {
                        LeagueName = "Saba A",
                        EventDateTime = new DateTime(2022, 04, 19, 14, 0, 0),
                        EndTime = new DateTime(2022, 04, 19, 15, 0, 0),
                    },
                    new()
                    {
                        LeagueName = "League B",
                        EventDateTime = new DateTime(2022, 04, 19, 13, 0, 0),
                        EndTime = new DateTime(2022, 04, 19, 16, 0, 0),
                    }
                }
            },
            new () // Available
            {
                UserInfo = new UserInfo
                {
                    UserId = 333,
                    ShiftStartTime = new DateTime(2022, 04, 19, 13, 0, 0),
                    ShiftEndTime = new DateTime(2022, 04, 19, 18, 0, 0),
                },
                CategorizedAssignments = new List<AssignmentModel>
                {
                    new()
                    {
                        LeagueName = "Saba A",
                        EventDateTime = new DateTime(2022, 04, 19, 17, 0, 0),
                        EndTime = new DateTime(2022, 04, 19, 18, 0, 0),
                    }
                }
            },
            new () // Available multiple
            {
                UserInfo = new UserInfo
                {
                    UserId = 333,
                    ShiftStartTime = new DateTime(2022, 04, 19, 13, 0, 0),
                    ShiftEndTime = new DateTime(2022, 04, 19, 18, 0, 0),
                    MaxMultipleGames = "2G"
                },
                CategorizedAssignments = new List<AssignmentModel>
                {
                    new()
                    {
                        LeagueName = "League B",
                        EventDateTime = new DateTime(2022, 04, 19, 14, 0, 0),
                        EndTime = new DateTime(2022, 04, 19, 16, 0, 0),
                        MatchType = MatchType.RealMatchLink1G
                    },
                    new()
                    {
                        LeagueName = "Saba B",
                        EventDateTime = new DateTime(2022, 04, 19, 14, 0, 0),
                        EndTime = new DateTime(2022, 04, 19, 15, 0, 0),
                        MatchType = MatchType.SabaLink1G,
                    }
                }
            }
        };

        service.Categorize(schedules, matchInfo);
    }
}
