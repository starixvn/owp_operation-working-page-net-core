using OWP.LiveGameList.Builder;
using OWP.LiveGameList.Models;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.LiveGameList.UnitTests.Builder;

public class AssignmentBuilderUnitTest
{
    [Fact]
    public void Build_Specific15MinCorner_ReturnExpect()
    {
        var assignment = MockAssignment();
        var expected = new AssignmentModel
        {
            Handicap = new List<Assignment> { new Assignment { BettypeId = 1, MinorBettypeId = 1, AssignmentType = 1, OWPMatchId = 117 } },
            Specific15Mins = new List<Assignment> { new Assignment { BettypeId = 1, MinorBettypeId = -5, AssignmentType = 1, OWPMatchId = 1117, ParentMatchId = 117 } }
        };
        var result = AssignmentBuilder.Build(new MatchInfo { OWPMatchId = 117, IsMinorMatch = true, MinorBettypeId = -3, ParentMatchId = 117 }, assignment);

        DeepAssert.Equal(expected.Handicap, result.Handicap.ToList());
        DeepAssert.Equal(expected.Specific15Mins, result.Specific15Mins.ToList());
    }

    [Fact]
    public void Build_Specific15MinBooking_ReturnExpect()
    {
        var assignment = MockAssignment();
        var expected = new AssignmentModel
        {
            Handicap = new List<Assignment> { new Assignment { BettypeId = 1, MinorBettypeId = 1, AssignmentType = 1, OWPMatchId = 117 } },
            Specific15Mins = new List<Assignment> { new Assignment { BettypeId = 1, MinorBettypeId = -8, AssignmentType = 1, OWPMatchId = 1117, ParentMatchId = 117 } }
        };
        var result = AssignmentBuilder.Build(new MatchInfo { OWPMatchId = 117, IsMinorMatch = true, MinorBettypeId = -6, ParentMatchId = 117 }, assignment);

        DeepAssert.Equal(expected.Handicap, result.Handicap.ToList());
        DeepAssert.Equal(expected.Specific15Mins, result.Specific15Mins.ToList());
    }

    public List<Assignment> MockAssignment()
        => new()
        {
            new Assignment
            {
                BettypeId = 1, MinorBettypeId = 1, AssignmentType = 1,OWPMatchId = 117,
            },
            new Assignment
            {
                BettypeId = 1, MinorBettypeId = -5, AssignmentType = 1,OWPMatchId = 1117, ParentMatchId = 117
            },
            new Assignment
            {
                BettypeId = 1, MinorBettypeId = -8, AssignmentType = 1,OWPMatchId = 1117, ParentMatchId = 117
            }
        };
}

