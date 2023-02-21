using OWP.AssignmentUpload.Validation.Constants;
using OWP.AssignmentUpload.Validation.Models;
using OWP.AssignmentUpload.Validation.Services;
using Xunit;

namespace OWP.AssignmentUpload.UnitTests.Validation.Services;

public class ExplicitMinorMatchRule2UnitTests
{
    private readonly ExplicitMinorMatchRule2Imp explicitMinorMatchRule2;

    public ExplicitMinorMatchRule2UnitTests()
    {
        explicitMinorMatchRule2 = new ExplicitMinorMatchRule2Imp();
    }


    [Fact]
    public void DetectMinorBettype_NotFoundMainMatch_ReturnFalse()
    {
        var assignment = new AssignmentModel
        {
            MatchNo = "1",
            MatchStatus = "Completed",
            KickOffTime = new TimeSpan(),
            League = " FCPENALTY SHOOT-OUT",
            Home = "FC Okzhetpes PENALTY SHOOT-OUT",
            Away = "FC Astana PENALTY SHOOT-OUT"
        };

        var assignments = new List<AssignmentModel>
        {
            new()
            {
                MatchNo = "1",
                League=" FCPENALTY SHOOT-OUT",
                MatchStatus="Completed",
                KickOffTime= new TimeSpan(),
                Home="FC PENALTY SHOOT-OUT",
                Away="FC PENALTY SHOOT-OUT"
            }
        };

        var result = ExplicitMinorMatchRule2Imp.DetectMinorBettypeIdRuleHome(assignment, assignments);
        Assert.False(result);
    }

    [Fact]
    public void Detect_DataRule2_SetMinorBettypeIdAndParentHashKey()
    {
        var assignments = new List<AssignmentModel>
        {
            new()
            {
                League = "leaguemar27",
                Home = "homemar27",
                Away = "awaymar27",
                HashKey = "9832587b812e62f74fd7bcb8c1e141fde2cd01fa3da8d7f3b9b1b6b0074355b8"
            },
            new()
            {
                League = "leaguemar27 - CORNERS",
                Home = "awaymar27 No.of Corners - Over",
                Away = "awaymar27 No.of Corners - Under"
            },

            new()
            {
                League = "leaguemar27 - CORNERS",
                Home = "homemar27 No.of Corners - Over",
                Away = "homemar27 No.of Corners - Under"
            }
        };

        foreach (var assignment in assignments)
        {
            explicitMinorMatchRule2.DetectMinorBettypeIdRule(assignment, assignments);
        }

        Assert.Null(assignments[0].MinorBettypeId);
        Assert.Equal(-95, assignments[1].MinorBettypeId);
        Assert.Equal(-94, assignments[2].MinorBettypeId);

        Assert.True(string.IsNullOrEmpty(assignments[0].ParentHashKey));
        Assert.Equal("9832587b812e62f74fd7bcb8c1e141fde2cd01fa3da8d7f3b9b1b6b0074355b8", assignments[1].ParentHashKey);
        Assert.Equal("9832587b812e62f74fd7bcb8c1e141fde2cd01fa3da8d7f3b9b1b6b0074355b8", assignments[2].ParentHashKey);
    }

    [Fact]
    public void Detect_Match2HasSpace_Match2HasNotParent()
    {
        var assignments = new List<AssignmentModel>
        {
            new()
            {
                League = "leaguemar27",
                Home = "homemar27(N)",
                Away = "awaymar27",
                HashKey = "9832587b812e62f74fd7bcb8c1e141fde2cd01fa3da8d7f3b9b1b6b0074355b8"
            },
            new()
            {
                League = "leaguemar27 - CORNERS",
                Home = "awaymar27 (N) No.of Corners - Over",
                Away = "awaymar27 (N) No.of Corners - Under"
            }
        };

        foreach (var assignment in assignments)
        {
            explicitMinorMatchRule2.DetectMinorBettypeIdRule(assignment, assignments);
        }

        Assert.Null(assignments[0].MinorBettypeId);
        Assert.Null(assignments[1].MinorBettypeId);

        Assert.True(string.IsNullOrEmpty(assignments[0].ParentHashKey));
        Assert.True(string.IsNullOrEmpty(assignments[1].ParentHashKey));
    }

    [Fact]
    public void Detect_Match2HasSpace()
    {
        var assignments = new List<AssignmentModel>
        {
            new()
            {
                MatchNo = "005",
                League = "LeagueTest-LT",
                Home = "LT0A",
                Away = "LT0B",
                HashKey = "hash123"
            },
            new() // -99
            {
                MatchNo = "024",
                League = "LeagueTest-LT - PENALTY SHOOT-OUT",
                Home = "LT0B PEN Shootout",
                Away = "LT0B PEN Shootout"
            },
            new() // -98
            {
                MatchNo = "025",
                League = "LeagueTest-LT - PENALTY SHOOT-OUT",
                Home = "LT0A PEN Shootout",
                Away = "LT0A PEN Shootout"
            }
        };

        foreach (var assignment in assignments)
        {
            explicitMinorMatchRule2.DetectMinorBettypeIdRule(assignment, assignments);
        }

        Assert.Null(assignments[0].MinorBettypeId);
        Assert.Equal(-99, assignments[1].MinorBettypeId);
        Assert.Equal(-98, assignments[2].MinorBettypeId);

        Assert.True(string.IsNullOrEmpty(assignments[0].ParentHashKey));
        Assert.Equal("hash123", assignments[1].ParentHashKey);
        Assert.Equal("hash123", assignments[2].ParentHashKey);
    }

    [Fact]
    public void FindMainMatchByHomeOrAway_LeagueLengthIsInvalid_ReturnNull()
    {
        var assignments = new List<AssignmentModel>
        {
            new() // -99
            {
                MatchNo = "024",
                League = "LeagueTest",
                Home = "LT0B PEN Shootout",
                Away = "LT0B PEN Shootout"
            }
        };

        var minorMatch = new MinorMatch
        {
            MinorBettypeId = -99,
            LeagueLike = "PENALTY SHOOT-OUT",
            LeagueNotLike = "WHICH TEAM TO TAKE THE FIRST PENALTY",
            HomeLike = "",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 19,
            HomeChars = 12,
            AwayChars = 12
        };

        var actual = ExplicitMinorMatchRule2Imp.FindMainMatchByHomeOrAway(assignments[0], assignments, minorMatch, TargetFinder.Home);

        Assert.Null(actual);
    }

    [Fact]
    public void FindMainMatchByHomeOrAway_AssignmentsIsEmpty_ReturnEmpty()
    {
        var assignments = new List<AssignmentModel>();
        var actual = ExplicitMinorMatchRule2Imp.FindMainMatchByHomeOrAway(assignments, "", "", "", TargetFinder.Home);

        Assert.Null(actual);
    }

    [Fact]
    public void FindMainMatchByHomeOrAway_AssignmentsIsNull_Home_ReturnEmpty()
    {
        var actual = ExplicitMinorMatchRule2Imp.FindMainMatchByHomeOrAway(null, "", "", "", TargetFinder.Home);

        Assert.Null(actual);
    }

    [Fact]
    public void FindMainMatchByHomeOrAway_AssignmentsIsNull_Away_ReturnEmpty()
    {
        var actual = ExplicitMinorMatchRule2Imp.FindMainMatchByHomeOrAway(null, "", "", "", TargetFinder.Away);

        Assert.Null(actual);
    }
}