using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using Xunit;

namespace OWP.AssigneeSuggestionList.UnitTests.Helper;

public class CategorizeHelperUnitTests
{
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
            }
        };
        var kOTime = new DateTime(2023, 4, 22, 8, 0, 0);
        var endTime = new DateTime(2023, 4, 22, 10, 0, 0);

        // Act
        var isFree = assignments.IsFreeInRangeTime(kOTime, endTime);

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
        var isFree = assignments.IsFreeInRangeTime(kOTime, endTime);

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
            }
        };
        var kOTime = new DateTime(2023, 4, 20, 10, 30, 0);
        var endTime = new DateTime(2023, 4, 21, 15, 0, 0);

        // Act
        var isFree = assignments.IsFreeInRangeTime(kOTime, endTime);

        // Assert
        Assert.False(isFree);
    }
}
