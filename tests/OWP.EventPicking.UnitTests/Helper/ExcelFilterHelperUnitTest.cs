using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Constant;
using OWP.Constants;
using Xunit;

namespace OWP.EventPicking.UnitTests.Helper;

public class ExcelFilterHelperUnitTest
{
    [Fact]
    public void ExcludeChildMatch_ShouldRemoveChildMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel { MinorBettypeId = 1, LeagueName = "League 1" },
                new EventPickingModel { MinorBettypeId = null, LeagueName = "League 2" },
                new EventPickingModel { MinorBettypeId = 1, LeagueName = "SABA League" }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.False(result[0].IsChildMatch);
    }


    [Fact]
    public void ExcludeChildMatch_ShouldRemove10MinsMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "SPECIFIC 10 MINS League",
                    HomeName = "Team A"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team B"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }


    [Fact]
    public void ExcludeChildMatch_ShouldRemoveNextCornersMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "CORNERS League",
                    HomeName = "Next Corner"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team A"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemove15MinsCornersMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "SPECIFIC 15 MINS NUMBER OF CORNERS League",
                    HomeName = "Team A"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team B"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemoveBookingsMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "BOOKING League",
                    HomeName = "Total Bookings"
                },
                new EventPickingModel
                {
                    LeagueName = "SPECIFIC 15 MINS TOTAL BOOKINGS League",
                    HomeName = "Total Bookings"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team A"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemoveNextBookingsMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "BOOKING League",
                    HomeName = "Next Booking"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team A"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemove15MinsBookingsMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "SPECIFIC 15 MINS TOTAL BOOKINGS League",
                    HomeName = "Team A"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team B"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemoveFirstPenaltyMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "PENALTY SHOOT-OUT - WHICH TEAM TO TAKE THE FIRST PENALTY League",
                    HomeName = "Team A"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team B"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemoveBronzeMedalMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "BRONZE MEDAL WINNER League",
                    HomeName = "Team A"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team B"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemoveGoldMedalMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "GOLD MEDAL WINNER League",
                    HomeName = "Team A"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team B"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemoveWinnerMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "WINNER League",
                    HomeName = "Team A"
                },
                new EventPickingModel
                {
                    LeagueName = "MEDAL WINNER League",
                    HomeName = "Team B"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team C"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, m => m.LeagueName == "MEDAL WINNER League");
        Assert.Contains(result, m => m.LeagueName == "Regular League");
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemoveAdvanceNextRoundMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "WHICH TEAM WILL ADVANCE TO NEXT ROUND League",
                    HomeName = "Team A"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team B"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemoveLastCornersMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "CORNERS League",
                    HomeName = "Last Corner"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team A"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemoveLastBookingsMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "BOOKING League",
                    HomeName = "Last Booking"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team A"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemoveSingleTeamOUMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "SINGLE TEAM OVER/UNDER League",
                    HomeName = "Team A"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team B"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemoveHomeSingleTeamOUOfCornersMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "CORNERS League",
                    HomeName = "No.of Corners - Over"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team A"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemove1stPenaltyScoreMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "PENALTY League",
                    AwayName = "Score"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team A"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemove1stPenaltyAwardedMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "PENALTY League",
                    AwayName = "Awarded"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team A"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }


    [Fact]
    public void ExcludeChildMatch_ShouldRemoveNextGoalKickMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "Goal Kick League",
                    HomeName = "Next"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team A"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemoveLastGoalKickMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "Goal Kick League",
                    HomeName = "Last"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team A"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemove2H1stGoalKickMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "Goal Kick League",
                    HomeName = "2nd"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team A"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemoveLastOffsideMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "Offside League",
                    HomeName = "Last"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team A"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemove2H1stOffsideMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "Offside League",
                    HomeName = "2nd"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team A"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemoveLastThrowInMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "Throw In League",
                    HomeName = "Last"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team A"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemove2H1stThrowInMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "Throw In League",
                    HomeName = "2nd"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team A"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemoveLastFreeKickMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "Free Kick League",
                    HomeName = "Last"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team A"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemove2H1stFreeKickMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "Free Kick League",
                    HomeName = "2nd"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team A"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldRemoveSpecialGoalMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "Specials Goal League",
                    HomeName = "Team A"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team B"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Single(result);
        Assert.Equal("Regular League", result[0].LeagueName);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldNotRemoveSabaMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches =
            [
                new EventPickingModel
                {
                    LeagueName = "SABA League - 15 Mins Play",
                    HomeName = "Team A"
                },
                new EventPickingModel
                {
                    LeagueName = "SABA League - CORNERS",
                    HomeName = "No.of Corners"
                },
                new EventPickingModel
                {
                    LeagueName = "Regular League",
                    HomeName = "Team B"
                }
            ]
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains(result, m => m.LeagueName == "SABA League - 15 Mins Play");
        Assert.Contains(result, m => m.LeagueName == "SABA League - CORNERS");
        Assert.Contains(result, m => m.LeagueName == "Regular League");
    }

    [Fact]
    public void ExcludeChildMatch_ShouldHandleEmptyMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches = new List<EventPickingModel>()
        };

        // Act
        var result = ExcelFilterHelper.ExcludeChildMatch(matches);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ExcludeChildMatch_ShouldHandleNullMatches()
    {
        // Arrange
        var matches = new EventPickingResult
        {
            FilteredMatches = null
        };

        // Act & Assert
        var exception = Record.Exception(() => ExcelFilterHelper.ExcludeChildMatch(matches));
        Assert.NotNull(exception);
        Assert.IsType<NullReferenceException>(exception);
    }

    [Fact]
    public void FilterLeagueName_ShouldExcludeMatchesWithSpecificText()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { LeagueName = "Premier League" },
            new EventPickingModel { LeagueName = "Championship" },
            new EventPickingModel { LeagueName = "La Liga" }
        };

        // Act
        var result = ExcelFilterHelper.FilterLeagueName(matches, "Premier");

        // Assert
        Assert.Equal(2, result.Count);
        Assert.DoesNotContain(result, m => m.LeagueName.Contains("Premier"));
    }

    [Fact]
    public void FilterLeagueName_ShouldBeCaseInsensitive()
    {
        // Arrange
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { LeagueName = "Premier League" },
            new EventPickingModel { LeagueName = "PREMIER League" },
            new EventPickingModel { LeagueName = "premier League" },
            new EventPickingModel { LeagueName = "Championship" }
        };

        // Act
        var result = ExcelFilterHelper.FilterLeagueName(matches, "premier");

        // Assert
        Assert.Single(result);
        Assert.Equal("Championship", result[0].LeagueName);
    }

    [Fact]
    public void FilterLeagueName_ShouldHandleEmptyMatches()
    {
        // Arrange
        var matches = new List<EventPickingModel>();

        // Act
        var result = ExcelFilterHelper.FilterLeagueName(matches, "Premier");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void FilterLeagueName_ShouldHandleNullMatches()
    {
        // Arrange
        List<EventPickingModel> matches = null;

        // Act & Assert
        var exception = Record.Exception(() => ExcelFilterHelper.FilterLeagueName(matches, "Premier"));
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
    }



}
