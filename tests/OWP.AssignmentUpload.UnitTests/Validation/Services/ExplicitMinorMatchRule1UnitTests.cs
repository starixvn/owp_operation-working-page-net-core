using OWP.AssignmentUpload.Validation.Models;
using OWP.AssignmentUpload.Validation.Services;
using Xunit;

namespace OWP.AssignmentUpload.UnitTests.Validation.Services;

public class ExplicitMinorMatchRule1UnitTests
{
    private readonly ExplicitMinorMatchRule1Imp explicitMinorMatchRule1;

    public ExplicitMinorMatchRule1UnitTests()
    {
        explicitMinorMatchRule1 = new ExplicitMinorMatchRule1Imp();
    }

    [Fact]
    public void Detect_DataRule1CORNERS_SetMinorBettypeIdAndParentHashKey()
    {
        var assignments = new List<AssignmentModel>
        {
            new()
            {
                League = "VIETNAM CHAMPIONSHIP U19 QUALIFIERS",
                Home = "Ha Noi FC U19 (N)",
                Away = "Than Quang Ninh U19",
                HashKey = "9832587b812e62f74fd7bcb8c1e141fde2cd01fa3da8d7f3b9b1b6b0074355b8"
            },
            new()
            {
                League = "VIETNAM CHAMPIONSHIP U19 QUALIFIERS - CORNERS",
                Home = "Ha Noi FC U19 (N) Next Corners",
                Away = "Than Quang Ninh U19 Next Corners"
            },
            new()
            {
                League = "VIETNAM CHAMPIONSHIP U19 QUALIFIERS - CORNERS",
                Home = "Ha Noi FC U19 (N) No.of Corners",
                Away = "Than Quang Ninh U19 No.of Corners"
            },
            new()
            {
                League = "VIETNAM CHAMPIONSHIP U19 QUALIFIERS",
                Home = "VIETNAM CHAMPIONSHIP U19 QUALIFIERS",
                Away = "Kos and Shin"
            }
        };

        foreach (var assignment in assignments)
        {
            explicitMinorMatchRule1.DetectMinorBettypeIdRule(assignment, assignments);
        }

        Assert.Null(assignments[0].MinorBettypeId);
        Assert.Equal(-4, assignments[1].MinorBettypeId);
        Assert.Equal(-3, assignments[2].MinorBettypeId);
        Assert.Null(assignments[3].MinorBettypeId);

        Assert.Equal("9832587b812e62f74fd7bcb8c1e141fde2cd01fa3da8d7f3b9b1b6b0074355b8", assignments[1].ParentHashKey);
        Assert.Equal("9832587b812e62f74fd7bcb8c1e141fde2cd01fa3da8d7f3b9b1b6b0074355b8", assignments[2].ParentHashKey);
        Assert.True(string.IsNullOrEmpty(assignments[0].ParentHashKey));
        Assert.True(string.IsNullOrEmpty(assignments[3].ParentHashKey));
    }

    [Fact]
    public void Detect_DataRule1BOOKING_SetMinorBettypeIdAndParentHashKey()
    {
        var assignments = new List<AssignmentModel>
        {
            new()
            {
                League = "leaguemar27",
                Home = "homemar277",
                Away = "awaymar277",
                HashKey = "9832587b812e62f74fd7bcb8c1e141fde2cd01fa3da8d7f3b9b1b6b0074355b8"
            },
            new()
            {
                League = "leaguemar27 - BOOKING",
                Home = "homemar277 Next Booking",
                Away = "awaymar277 Next Booking"
            }
        };

        foreach (var assignment in assignments)
        {
            explicitMinorMatchRule1.DetectMinorBettypeIdRule(assignment, assignments);
        }

        Assert.Null(assignments[0].MinorBettypeId);
        Assert.Equal(-7, assignments[1].MinorBettypeId);

        Assert.True(string.IsNullOrEmpty(assignments[0].ParentHashKey));
        Assert.Equal("9832587b812e62f74fd7bcb8c1e141fde2cd01fa3da8d7f3b9b1b6b0074355b8", assignments[1].ParentHashKey);
    }

    [Fact]
    public void Detect_MultipleDataRule1_SetMinorBettypeIdAndParentHashKey()
    {
        var assignments = new List<AssignmentModel>
        {
            new()
            {
                League = "VietNam League",
                Home = "A",
                Away = "B",
                HashKey = "123456"
            },
            new() //-7
            {
                League = "VietNam League - BOOKING",
                Home = "A Next Booking",
                Away = "B Next Booking"
            },
            new() //-6
            {
                League = "VietNam League - BOOKING",
                Home = "A Total Bookings",
                Away = "B Total Bookings"
            },
            new() //-4
            {
                League = "VietNam League - CORNERS",
                Home = "A Next Corners",
                Away = "B Next Corners"
            },
            new() //-3
            {
                League = "VietNam League - CORNERS",
                Home = "A No.of Corners",
                Away = "B No.of Corners"
            },
            new()
            {
                League = "league test",
                Home = "C",
                Away = "D"
            }
        };

        foreach (var assignment in assignments)
        {
            explicitMinorMatchRule1.DetectMinorBettypeIdRule(assignment, assignments);
        }

        Assert.Null(assignments[0].MinorBettypeId);
        Assert.Equal(-7, assignments[1].MinorBettypeId);
        Assert.Equal(-6, assignments[2].MinorBettypeId);
        Assert.Equal(-4, assignments[3].MinorBettypeId);
        Assert.Equal(-3, assignments[4].MinorBettypeId);
        Assert.Null(assignments[5].MinorBettypeId);

        Assert.True(string.IsNullOrEmpty(assignments[0].ParentHashKey));
        Assert.Equal("123456", assignments[1].ParentHashKey);
        Assert.Equal("123456", assignments[2].ParentHashKey);
        Assert.Equal("123456", assignments[3].ParentHashKey);
        Assert.Equal("123456", assignments[4].ParentHashKey);
        Assert.True(string.IsNullOrEmpty(assignments[5].ParentHashKey));
    }

    [Fact]
    public void Detect_ParentHashKeyIsNotNullButMinorBettyeIdIsNull_ParentHashKeyIsEmpty()
    {
        var assignments = new List<AssignmentModel>
        {
            new()
            {
                League = "VietNam League",
                Home = "A",
                Away = "B",
                HashKey = "123456"
            },
            new() //-7
            {
                League = "VietNam League - BOOKING",
                Home = "A Booking",
                Away = "B Next Booking"
            }
        };

        foreach (var assignment in assignments)
        {
            explicitMinorMatchRule1.DetectMinorBettypeIdRule(assignment, assignments);
        }

        Assert.Null(assignments[1].MinorBettypeId);
        Assert.Empty(assignments[1].ParentHashKey);
    }

    [Fact]
    public void Detect_MatchHasNotParentHashKey_MinorBettypeIsNull()
    {
        var assignments = new List<AssignmentModel>
        {
            new() //-7
            {
                League = "VietNam League - BOOKING",
                Home = "A Next Booking",
                Away = "B Next Booking"
            }
        };

        foreach (var assignment in assignments)
        {
            explicitMinorMatchRule1.DetectMinorBettypeIdRule(assignment, assignments);
        }

        Assert.Null(assignments[0].MinorBettypeId);
        Assert.Empty(assignments[0].ParentHashKey);
    }

    [Fact]
    public void FindMainMatch_LeagueLengthIsInvalid_ReturnNull()
    {
        var assignments = new List<AssignmentModel>
        {
            new() //-7
            {
                League = "VietNam",
                Home = "A Next Booking",
                Away = "B Next Booking"
            }
        };
        var minorMatch = new MinorMatch
        {
            MinorBettypeId = -7,
            LeagueLike = "BOOKING",
            LeagueNotLike = "",
            HomeLike = "Next Booking",
            HomeNotLike = "Total Bookings",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 9,
            HomeChars = 12,
            AwayChars = 12
        };
        var result = ExplicitMinorMatchRule1Imp.FindMainMatch(assignments[0], assignments, minorMatch);

        Assert.Null(result);
    }
}