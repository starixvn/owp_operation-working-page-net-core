using System.ComponentModel.DataAnnotations;
using OWP.OtherSport.Validation.Helpers;
using OWP.OtherSport.Validation.Models;
using Xunit;

namespace OWP.OtherSport.UnitTests.Validation.Helpers;

public class ValidationHelperUnitTests
{
    [Fact]
    public void BuildValidationRowResult_WithValidInputs_ShouldAddValidationResultsToCollection()
    {
        // Arrange
        var validationResults = new List<ValidationResult>
        {
            new ValidationResult("Error 1"),
            new ValidationResult("Error 2"),
            new ValidationResult("Error 3")
        };
        var validationRowResult = new List<AssignmentValidationRowResult>();
        var rowIndex = 5;

        // Act
        ValidationHelper.BuildValidationRowResult(validationResults, validationRowResult, rowIndex);

        // Assert
        Assert.Equal(3, validationRowResult.Count);
        Assert.All(validationRowResult, result => Assert.Equal(rowIndex, result.Row));
        Assert.Equal("Error 1", validationRowResult[0].Message);
        Assert.Equal("Error 2", validationRowResult[1].Message);
        Assert.Equal("Error 3", validationRowResult[2].Message);
    }

    [Fact]
    public void BuildValidationRowResult_WithEmptyValidationResults_ShouldNotAddAnyResults()
    {
        // Arrange
        var validationResults = new List<ValidationResult>();
        var validationRowResult = new List<AssignmentValidationRowResult>();
        var rowIndex = 1;

        // Act
        ValidationHelper.BuildValidationRowResult(validationResults, validationRowResult, rowIndex);

        // Assert
        Assert.Empty(validationRowResult);
    }

    [Fact]
    public void BuildValidationRowResult_WithNullValidationResults_ShouldThrowArgumentNullException()
    {
        // Arrange
        List<ValidationResult> validationResults = null;
        var validationRowResult = new List<AssignmentValidationRowResult>();
        var rowIndex = 1;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            ValidationHelper.BuildValidationRowResult(validationResults, validationRowResult, rowIndex));
    }

    [Fact]
    public void BuildValidationRowResult_WithNullValidationRowResult_ShouldThrowNullReferenceException()
    {
        // Arrange
        var validationResults = new List<ValidationResult>
        {
            new ValidationResult("Error 1")
        };
        List<AssignmentValidationRowResult> validationRowResult = null;
        var rowIndex = 1;

        // Act & Assert
        Assert.Throws<NullReferenceException>(() =>
            ValidationHelper.BuildValidationRowResult(validationResults, validationRowResult, rowIndex));
    }

    [Fact]
    public void BuildValidationRowResult_WithNegativeRowIndex_ShouldStillProcessResults()
    {
        // Arrange
        var validationResults = new List<ValidationResult>
        {
            new ValidationResult("Error 1")
        };
        var validationRowResult = new List<AssignmentValidationRowResult>();
        var rowIndex = -1;

        // Act
        ValidationHelper.BuildValidationRowResult(validationResults, validationRowResult, rowIndex);

        // Assert
        Assert.Single(validationRowResult);
        Assert.Equal(rowIndex, validationRowResult[0].Row);
        Assert.Equal("Error 1", validationRowResult[0].Message);
    }

    [Fact]
    public void BuildValidationRowResult_WithZeroRowIndex_ShouldProcessResultsCorrectly()
    {
        // Arrange
        var validationResults = new List<ValidationResult>
        {
            new ValidationResult("Error 1"),
            new ValidationResult("Error 2")
        };
        var validationRowResult = new List<AssignmentValidationRowResult>();
        var rowIndex = 0;

        // Act
        ValidationHelper.BuildValidationRowResult(validationResults, validationRowResult, rowIndex);

        // Assert
        Assert.Equal(2, validationRowResult.Count);
        Assert.All(validationRowResult, result => Assert.Equal(rowIndex, result.Row));
    }

    [Fact]
    public void BuildValidationRowResult_WithLargeRowIndex_ShouldProcessResultsCorrectly()
    {
        // Arrange
        var validationResults = new List<ValidationResult>
        {
            new ValidationResult("Error 1")
        };
        var validationRowResult = new List<AssignmentValidationRowResult>();
        var rowIndex = int.MaxValue;

        // Act
        ValidationHelper.BuildValidationRowResult(validationResults, validationRowResult, rowIndex);

        // Assert
        Assert.Single(validationRowResult);
        Assert.Equal(rowIndex, validationRowResult[0].Row);
        Assert.Equal("Error 1", validationRowResult[0].Message);
    }

    [Fact]
    public void BuildValidationRowResult_WithExistingResultsInCollection_ShouldAppendNewResults()
    {
        // Arrange
        var existingResults = new List<AssignmentValidationRowResult>
        {
            new AssignmentValidationRowResult { Row = 1, Message = "Existing Error" }
        };
        var validationResults = new List<ValidationResult>
        {
            new ValidationResult("New Error 1"),
            new ValidationResult("New Error 2")
        };
        var rowIndex = 2;

        // Act
        ValidationHelper.BuildValidationRowResult(validationResults, existingResults, rowIndex);

        // Assert
        Assert.Equal(3, existingResults.Count);
        Assert.Equal("Existing Error", existingResults[0].Message);
        Assert.Equal("New Error 1", existingResults[1].Message);
        Assert.Equal("New Error 2", existingResults[2].Message);
        Assert.Equal(1, existingResults[0].Row);
        Assert.Equal(2, existingResults[1].Row);
        Assert.Equal(2, existingResults[2].Row);
    }

    [Fact]
    public void BuildValidationRowResult_WithValidationResultsContainingNullErrorMessage_ShouldHandleNullMessages()
    {
        // Arrange
        var validationResults = new List<ValidationResult>
        {
            new ValidationResult(null),
            new ValidationResult("Valid Error"),
            new ValidationResult("")
        };
        var validationRowResult = new List<AssignmentValidationRowResult>();
        var rowIndex = 1;

        // Act
        ValidationHelper.BuildValidationRowResult(validationResults, validationRowResult, rowIndex);

        // Assert
        Assert.Equal(3, validationRowResult.Count);
        Assert.Null(validationRowResult[0].Message);
        Assert.Equal("Valid Error", validationRowResult[1].Message);
        Assert.Equal("", validationRowResult[2].Message);
        Assert.All(validationRowResult, result => Assert.Equal(rowIndex, result.Row));
    }

    [Fact]
    public void BuildValidationRowResult_WithSingleValidationResult_ShouldAddSingleResult()
    {
        // Arrange
        var validationResults = new List<ValidationResult>
        {
            new ValidationResult("Single Error")
        };
        var validationRowResult = new List<AssignmentValidationRowResult>();
        var rowIndex = 10;

        // Act
        ValidationHelper.BuildValidationRowResult(validationResults, validationRowResult, rowIndex);

        // Assert
        Assert.Single(validationRowResult);
        Assert.Equal(rowIndex, validationRowResult[0].Row);
        Assert.Equal("Single Error", validationRowResult[0].Message);
    }
}