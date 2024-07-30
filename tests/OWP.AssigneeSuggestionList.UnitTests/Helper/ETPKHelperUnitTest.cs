using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using Xunit;

namespace OWP.AssigneeSuggestionList.UnitTests.Helper;

public class ETPKHelperUnitTest
{
    [Theory]
    [InlineData("remark ET PK", null, -4, "ET")]
    [InlineData("remark with PK", null, null, "PK")]
    [InlineData("remark with PK", "note with PK", -4, "")]
    [InlineData("remark with ET PK", "with ET PK", null, "ET/PK")]
    [InlineData("remark with ET", "note with ET PK", null, "")]
    [InlineData("remark with ET", "note with No PK", null, "")]
    [InlineData("", "", null, "")]
    public void DetectExtraTime_ShouldReturnExpectedResult(string remark, string? assignmentNote, int? minorBettypeId, string expected)
    {
        // Act
        var result = ETPKHelper.DetectExtraTime(remark, assignmentNote, minorBettypeId);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void BuildEtPkTextDisplay_ShouldReturnExpectedText()
    {
        // Arrange
        var assignments = new List<AssignmentModel>
        {
            new AssignmentModel { Remark = "remark with ET PK", AssignmentNote = "note with ET PK", MinorBettypeId = null },
            new AssignmentModel { Remark = "remark with PK", AssignmentNote = "note with PK", MinorBettypeId = null },
            new AssignmentModel { Remark = "remark with ET", AssignmentNote = "note with ET PK", MinorBettypeId = null }
        };

        var expectedText = "";

        // Act
        var result = ETPKHelper.BuildEtPkTextDisplay(assignments);

        // Assert
        Assert.Equal(expectedText, result);
    }
}