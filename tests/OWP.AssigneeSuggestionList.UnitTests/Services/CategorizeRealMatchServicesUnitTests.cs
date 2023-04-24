using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services;
using OWP.Constants;
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
            BettypeId = 1,
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
            },
            new () // PartiallyFree
            {
                UserInfo = new UserInfo
                {
                    UserId = 444,
                    ShiftStartTime = new DateTime(2022, 04, 19, 13, 0, 0),
                    ShiftEndTime = new DateTime(2022, 04, 19, 18, 0, 0),
                    MaxMultipleGames = "2G"
                },
                CategorizedAssignments = new List<AssignmentModel>
                {
                    new()
                    {
                        LeagueName = "League B",
                        EventDateTime = new DateTime(2022, 04, 19, 15, 0, 0),
                        EndTime = new DateTime(2022, 04, 19, 16, 0, 0),
                        MatchType = MatchType.RealMatchLink1G
                    }
                }
            },
            new () // PartiallyFree - free in ws
            {
                UserInfo = new UserInfo
                {
                    UserId = 444,
                    ShiftStartTime = new DateTime(2022, 04, 19, 13, 0, 0),
                    ShiftEndTime = new DateTime(2022, 04, 19, 15, 0, 0),
                    MaxMultipleGames = "2G"
                }
            }
        };

        service.Categorize(schedules, matchInfo);

        Assert.Equal(CategoryType.Busy, schedules[0].CategoryType);
        Assert.Equal(CategoryType.Busy, schedules[1].CategoryType);
        Assert.Equal(CategoryType.FullyAvailable, schedules[2].CategoryType);
        Assert.Equal(CategoryType.FullyAvailable, schedules[3].CategoryType);
        Assert.Equal(CategoryType.PartiallyAvailable, schedules[4].CategoryType);
        Assert.Equal(CategoryType.PartiallyAvailable, schedules[5].CategoryType);
        Assert.Equal(CategoryType.PartiallyAvailable, schedules[5].CategoryType);
    }

    [Fact]
    public void IsFreeInRangeTime_NoAssignmentsInRange_ReturnsTrue()
    {
        // Arrange
        var assignments = new List<AssignmentModel>
        {
            new()
            {
                EventDateTime = new DateTime(2023, 4, 20, 10, 0, 0),
                EndTime = new DateTime(2023, 4, 20, 12, 0, 0)
            },
            new()
            {
                EventDateTime = new DateTime(2023, 4, 21, 6, 0, 0),
                EndTime = new DateTime(2023, 4, 21, 8, 0, 0)
            },
        };
        var kOTime = new DateTime(2023, 4, 22, 8, 0, 0);
        var endTime = new DateTime(2023, 4, 22, 10, 0, 0);

        // Act
        var isFree = CategorizeRealMatchServicesImp.IsFreeInRangeTime(assignments, kOTime, endTime);

        // Assert
        Assert.True(isFree);
    }

    [Fact]
    public void IsFreeInRangeTime_OneAssignmentInRange_ReturnsFalse()
    {
        // Arrange
        var assignments = new List<AssignmentModel>
        {
            new()
            {
                EventDateTime = new DateTime(2023, 4, 20, 10, 0, 0),
                EndTime = new DateTime(2023, 4, 20, 12, 0, 0)
            },
            new()
            {
                EventDateTime = new DateTime(2023, 4, 21, 14, 0, 0),
                EndTime = new DateTime(2023, 4, 21, 16, 0, 0)
            }
        };
        var kOTime = new DateTime(2023, 4, 20, 11, 0, 0);
        var endTime = new DateTime(2023, 4, 20, 13, 0, 0);

        // Act
        var isFree = CategorizeRealMatchServicesImp.IsFreeInRangeTime(assignments, kOTime, endTime);

        // Assert
        Assert.False(isFree);
    }

    [Fact]
    public void IsFreeInRangeTime_AllAssignmentsInRange_ReturnsFalse()
    {
        // Arrange
        var assignments = new List<AssignmentModel>
        {
            new()
            {
                EventDateTime = new DateTime(2023, 4, 20, 10, 0, 0),
                EndTime = new DateTime(2023, 4, 20, 16, 0, 0)
            },
            new()
            {
                EventDateTime = new DateTime(2023, 4, 21, 14, 0, 0),
                EndTime = new DateTime(2023, 4, 21, 16, 0, 0)
            },
        };
        var kOTime = new DateTime(2023, 4, 20, 10, 30, 0);
        var endTime = new DateTime(2023, 4, 21, 15, 0, 0);

        // Act
        var isFree = CategorizeRealMatchServicesImp.IsFreeInRangeTime(assignments, kOTime, endTime);

        // Assert
        Assert.False(isFree);
    }

    [Theory]
    [InlineData(AssigneeCategory.Free, CategoryType.FullyAvailable, "100% free from 02:00 PM to 04:00 PM")]
    [InlineData(AssigneeCategory.Capacity2GDoing1G, CategoryType.FullyAvailable, ExplanationConstants.CAPACITY_2G_DOING_1G)]
    [InlineData(AssigneeCategory.Capacity3GDoing2G, CategoryType.FullyAvailable, ExplanationConstants.CAPACITY_3G_DOING_2G)]
    [InlineData(AssigneeCategory.Capacity3GDoing1G, CategoryType.FullyAvailable, ExplanationConstants.CAPACITY_3G_DOING_1G)]
    [InlineData(AssigneeCategory.PartiallyFree, CategoryType.PartiallyAvailable, "free in the first half of the match from 02:00 PM to 04:00 PM")]
    [InlineData(AssigneeCategory.Busy, CategoryType.Busy, ExplanationConstants.BUSY)]
    public void GetCategoryTypeAndExplanation_ShouldReturnCorrectValues(
        AssigneeCategory category, CategoryType expectedCategoryType, string expectedExplanation)
    {
        var matchInfo = new MatchInfo
        {
            BettypeId = Bettype.NextCorner,
            IsSingleMatch = true,
            KOTime = new DateTime(2022, 04, 19, 14, 0, 0),
            EndTime = new DateTime(2022, 04, 19, 16, 0, 0)
        };

        var (actualCategoryType, actualExplanation) = CategorizeRealMatchServicesImp.GetCategoryTypeAndExplanation(category, matchInfo);

        Assert.Equal(expectedCategoryType, actualCategoryType);
        Assert.Equal(expectedExplanation, actualExplanation);
    }

    [Theory]
    [InlineData("2G-1LA", true, false, AssigneeCategory.Capacity2GDoing1G)]
    [InlineData("3G", true, false, AssigneeCategory.Capacity3GDoing1G)]
    [InlineData("3G", false, true, AssigneeCategory.Capacity3GDoing2G)]
    [InlineData("3G", false, false, AssigneeCategory.Busy)]
    [InlineData("1G", false, false, AssigneeCategory.Busy)]
    public void HasAssignmentLinkNGame_Returns_Correct_Category(
        string mixMatchCapacity,
        bool hasLink1G,
        bool hasLink2G,
        AssigneeCategory expectedCategory)
    {
        var matchInfo = new MatchInfo
        {
            OwpMatchId = 100,
            KOTime = new DateTime(2022, 04, 19, 14, 0, 0),
            EndTime = new DateTime(2022, 04, 19, 16, 0, 0),
            BettypeId = 1,
            MaxbetHDP = CategoryConstants.MAX_BET_5K
        };

        var userInfo = new UserInfo
        {
            UserId = 111,
            MaxMultipleGames = mixMatchCapacity,
            ShiftStartTime = new DateTime(2022, 04, 19, 18, 0, 0),
            ShiftEndTime = new DateTime(2022, 04, 19, 23, 0, 0),
        };

        var assignments = new List<AssignmentModel>
        {
            new()
            {
                LeagueName = "League A",
                EventDateTime = new DateTime(2022, 04, 19, 14, 0, 0),
                EndTime = new DateTime(2022, 04, 19, 15, 0, 0),
                MatchType = hasLink1G ? MatchType.RealMatchLink1G : MatchType.Other
            },
            new()
            {
                LeagueName = "League B",
                EventDateTime = new DateTime(2022, 04, 19, 14, 0, 0),
                EndTime = new DateTime(2022, 04, 19, 16, 0, 0),
                MatchType = hasLink2G ? MatchType.RealMatchLink2G : MatchType.Other
            }
        };

        var result = CategorizeRealMatchServicesImp.HasAssignmentLinkNGame(userInfo, assignments, matchInfo);

        Assert.Equal(expectedCategory, result);
    }
}
