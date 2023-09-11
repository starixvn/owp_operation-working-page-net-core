using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services;
using Xunit;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;

namespace OWP.AssigneeSuggestionList.UnitTests.Services;

public class CategorizeSabaMatchServicesUnitTests
{
    private readonly CategorizeSabaMatchServicesImp service;

    public CategorizeSabaMatchServicesUnitTests()
    {
        service = new CategorizeSabaMatchServicesImp();
    }

    [Fact]
    public void Categorize()
    {
        var matchInfo = new MatchInfo
        {
            OwpMatchId = 100,
            KOTime = new DateTime(2022, 04, 19, 14, 0, 0),
            EndTime = new DateTime(2022, 04, 19, 15, 0, 0),
            BettypeId = 1,
            LeagueName = "SABA A"
        };

        var schedules = new List<WorkingSchedule>
        {
            new () // Busy (no ws)
            {
                UserInfo = new UserInfo
                {
                    UserId = 111,
                    ShiftStartTime = new DateTime(2022, 04, 19, 18, 0, 0),
                    ShiftEndTime = new DateTime(2022, 04, 19, 23, 0, 0)
                }
            },
            new () // Busy
            {
                UserInfo = new UserInfo
                {
                    UserId = 222,
                    ShiftStartTime = new DateTime(2022, 04, 19, 13, 0, 0),
                    ShiftEndTime = new DateTime(2022, 04, 19, 18, 0, 0)
                },
                CategorizedAssignments = new List<AssignmentModel>
                {
                    new()
                    {
                        LeagueName = "Saba A",
                        EventDateTime = new DateTime(2022, 04, 19, 14, 0, 0),
                        EndTime = new DateTime(2022, 04, 19, 15, 0, 0)
                    },
                    new()
                    {
                        LeagueName = "League B",
                        EventDateTime = new DateTime(2022, 04, 19, 13, 0, 0),
                        EndTime = new DateTime(2022, 04, 19, 16, 0, 0)
                    }
                }
            },
            new () // Available
            {
                UserInfo = new UserInfo
                {
                    UserId = 333,
                    ShiftStartTime = new DateTime(2022, 04, 19, 13, 0, 0),
                    ShiftEndTime = new DateTime(2022, 04, 19, 18, 0, 0)
                },
                CategorizedAssignments = new List<AssignmentModel>
                {
                    new()
                    {
                        LeagueName = "Saba A",
                        EventDateTime = new DateTime(2022, 04, 19, 17, 0, 0),
                        EndTime = new DateTime(2022, 04, 19, 18, 0, 0)
                    }
                }
            },
            new () // busy, cannot link 5 min
            {
                UserInfo = new UserInfo
                {
                    UserId = 444,
                    ShiftStartTime = new DateTime(2022, 04, 19, 13, 0, 0),
                    ShiftEndTime = new DateTime(2022, 04, 19, 18, 0, 0),
                    MainBetType = "HDP"
                },
                CategorizedAssignments = new List<AssignmentModel>
                {
                    new()
                    {
                        LeagueName = "Saba B",
                        EventDateTime = new DateTime(2022, 04, 19, 14, 0, 0),
                        EndTime = new DateTime(2022, 04, 19, 15, 0, 0),
                        MatchType = MatchType.Saba5Mins
                    }
                }
            }
        };

        service.Categorize(schedules, matchInfo);

        Assert.Equal(CategoryType.Busy, schedules[0].CategoryType);
        Assert.Equal(CategoryType.Busy, schedules[1].CategoryType);
        Assert.Equal(CategoryType.FullyAvailable, schedules[2].CategoryType);
        Assert.Equal(CategoryType.Busy, schedules[3].CategoryType);
    }

    [Theory]
    [InlineData(MatchType.SabaLink3G, 14, AssigneeSabaCategory.Doing3G)]
    [InlineData(MatchType.SabaLink2G, 14, AssigneeSabaCategory.Doing2G)]
    [InlineData(MatchType.SabaLink4G, 14, AssigneeSabaCategory.Busy)]
    [InlineData(MatchType.SabaLink4G, 15, AssigneeSabaCategory.Busy)]
    [InlineData(MatchType.SabaLink1G, 14, AssigneeSabaCategory.Doing1G)]
    public void LinkSabaGame(MatchType matchType, int startHour, AssigneeSabaCategory expect)
    {
        var matchInfo = new MatchInfo
        {
            OwpMatchId = 100,
            KOTime = new DateTime(2022, 04, 19, 14, 0, 0),
            EndTime = new DateTime(2022, 04, 19, 15, 0, 0),
            BettypeId = 1,
            LeagueName = "SABA A"
        };
        var assignments = new List<AssignmentModel>
        {
            new()
            {
                LeagueName = "Saba A",
                EventDateTime = new DateTime(2022, 04, 19, startHour, 00, 0),
                EndTime = new DateTime(2022, 04, 19, 16, 0, 0),
                MatchType = matchType
            }
        };
        var result = CategorizeSabaMatchServicesImp.LinkSabaGame(assignments, matchInfo);

        Assert.Equal(expect, result);
    }

    [Theory]
    [InlineData(MatchType.Saba5Mins, 14, 0, AssigneeSabaCategory.Doing5Mins1G)]
    [InlineData(MatchType.Saba5Mins, 14, 15, AssigneeSabaCategory.Doing5Mins1G)]
    [InlineData(MatchType.Saba5Mins, 15, 0, AssigneeSabaCategory.Busy)]
    public void Link5Min(MatchType matchType, int startHour, int startMinute, AssigneeSabaCategory expect)
    {
        var matchInfo = new MatchInfo
        {
            OwpMatchId = 100,
            KOTime = new DateTime(2022, 04, 19, 14, 0, 0),
            EndTime = new DateTime(2022, 04, 19, 15, 0, 0),
            BettypeId = 1,
            LeagueName = "SABA A"
        };
        var assignments = new List<AssignmentModel>
        {
            new()
            {
                LeagueName = "Saba A",
                EventDateTime = new DateTime(2022, 04, 19, startHour, startMinute, 0),
                EndTime = new DateTime(2022, 04, 19, 16, 0, 0),
                MatchType = matchType
            }
        };
        var result = CategorizeSabaMatchServicesImp.Link5MinsGame(assignments, matchInfo);

        Assert.Equal(expect, result);
    }

    [Theory]
    [InlineData(AssigneeSabaCategory.Doing1G, CategoryType.FullyAvailable, "doing 1 of 4 games in capacity")]
    [InlineData(AssigneeSabaCategory.Doing2G, CategoryType.FullyAvailable, "doing 2 of 4 games in capacity")]
    [InlineData(AssigneeSabaCategory.Doing3G, CategoryType.FullyAvailable, "doing 3 of 4 games in capacity")]
    [InlineData(AssigneeSabaCategory.Free, CategoryType.FullyAvailable, "100% free from 02:00 PM to 03:00 PM")]
    [InlineData(AssigneeSabaCategory.Doing5Mins1G, CategoryType.FullyAvailable, "doing 1 of 2 games in capacity")]
    [InlineData(AssigneeSabaCategory.Busy, CategoryType.Busy, null)]
    public void GetCategoryTypeAndExplanation(AssigneeSabaCategory type, CategoryType expectedCategoryType, string expectedExplanation)
    {
        var matchInfo = new MatchInfo
        {
            OwpMatchId = 100,
            KOTime = new DateTime(2022, 04, 19, 14, 0, 0),
            EndTime = new DateTime(2022, 04, 19, 15, 0, 0)
        };
        var (actualCategoryType, actualExplanation) = CategorizeSabaMatchServicesImp.GetCategoryTypeAndExplanation(type, matchInfo);

        Assert.Equal(expectedCategoryType, actualCategoryType);
        Assert.Equal(expectedExplanation, actualExplanation);
    }
}
