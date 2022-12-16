namespace OWP.TaskList.UnitTests.Builder;

using Builders;
using Models;
using Xunit;
using Xunit.Asserts.Compare;

public class AssignmentBuilderUnitTests
{
    [Fact]
    public void Build_Specific15MinCorner_ReturnExpect()
    {
        var assignment = MockAssignment();
        var expected = new AssignmentModel
        {
            Handicap = new List<Assignment>
            {
                new()
                {
                    BettypeId = 1, MinorBettypeId = -5, AssignmentType = 1, OWPMatchId = 1117, ParentMatchId = 117
                },
                new()
                {
                    BettypeId = 1, MinorBettypeId = -8, AssignmentType = 1, OWPMatchId = 1117, ParentMatchId = 117
                }
            },
            Specific15Mins = new List<Assignment>
            {
                new() { BettypeId = 1, MinorBettypeId = -5, AssignmentType = 1, OWPMatchId = 1117, ParentMatchId = 117 }
            }
        };
        var result =
            AssignmentBuilder.Build(new MatchInfo { OWPMatchId = 1117, MinorBettypeId = -3, ParentMatchId = 117 },
                assignment);

        DeepAssert.Equal(expected.Handicap, result.Handicap.ToList());
        DeepAssert.Equal(expected.Specific15Mins, result.Specific15Mins.ToList());
    }

    [Fact]
    public void Build_Specific15MinBooking_ReturnExpect()
    {
        var assignment = MockAssignment();
        var expected = new AssignmentModel
        {
            Handicap = new List<Assignment>
            {
                new()
                {
                    BettypeId = 1, MinorBettypeId = -5, AssignmentType = 1, OWPMatchId = 1117, ParentMatchId = 117
                },
                new()
                {
                    BettypeId = 1, MinorBettypeId = -8, AssignmentType = 1, OWPMatchId = 1117, ParentMatchId = 117
                }
            },
            Specific15Mins = new List<Assignment>
            {
                new() { BettypeId = 1, MinorBettypeId = -8, AssignmentType = 1, OWPMatchId = 1117, ParentMatchId = 117 }
            }
        };
        var result =
            AssignmentBuilder.Build(new MatchInfo { OWPMatchId = 1117, MinorBettypeId = -6, ParentMatchId = 117 },
                assignment);

        DeepAssert.Equal(expected.Handicap, result.Handicap.ToList());
        DeepAssert.Equal(expected.Specific15Mins, result.Specific15Mins.ToList());
    }

    public List<Assignment> MockAssignment()
    {
        return new List<Assignment>
        {
            new()
            {
                BettypeId = 1, MinorBettypeId = 1, AssignmentType = 1, OWPMatchId = 117
            },
            new()
            {
                BettypeId = 1, MinorBettypeId = -5, AssignmentType = 1, OWPMatchId = 1117, ParentMatchId = 117
            },
            new()
            {
                BettypeId = 1, MinorBettypeId = -8, AssignmentType = 1, OWPMatchId = 1117, ParentMatchId = 117
            }
        };
    }
}