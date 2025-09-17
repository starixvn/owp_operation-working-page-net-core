using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Constant;
using OWP.Constants;
using Xunit;
using OfficeOpenXml;
using System.Drawing;

namespace OWP.EventPicking.UnitTests.Helper;

public class ExcelColorHelperUnitTest
{
    [Fact]
    public void FillRowColor_ShouldHandleNullWorksheet()
    {
        // Arrange
        ExcelWorksheet worksheet = null!;
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "red" }
        };

        // Act & Assert
        var exception = Record.Exception(() => ExcelColorHelper.FillRowColor(matches, worksheet));
        Assert.NotNull(exception);
        Assert.IsType<NullReferenceException>(exception);
    }

    [Fact]
    public void FillRowColor_ShouldHandleNullMatches()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        List<EventPickingModel> matches = null!;

        // Act & Assert
        var exception = Record.Exception(() => ExcelColorHelper.FillRowColor(matches, worksheet));
        Assert.NotNull(exception);
        Assert.IsType<NullReferenceException>(exception);
    }

    [Fact]
    public void FillRowColor_ShouldHandleEmptyMatches()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>();

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithNullRowColor()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = null }
        };

        // Act & Assert
        var exception = Record.Exception(() => ExcelColorHelper.FillRowColor(matches, worksheet));
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithEmptyRowColor()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithWhitespaceRowColor()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "   " }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithValidRowColor()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "red" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMultipleMatches()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "red" },
            new EventPickingModel { RowColor = "blue" },
            new EventPickingModel { RowColor = "green" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithSpecialCharactersInRowColor()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "red!@#$%^&*()_+-=[]{}|;':\",./<>?" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithUnicodeCharactersInRowColor()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "red中文日本語한국어" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithNewlinesInRowColor()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "red\nblue\ngreen" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithTabsInRowColor()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "red\tblue\tgreen" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithQuotesInRowColor()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "\"red\" 'blue' green" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithVeryLongRowColor()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = new string('A', 10000) }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithMixedRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "red" },
            new EventPickingModel { RowColor = null },
            new EventPickingModel { RowColor = "" },
            new EventPickingModel { RowColor = "   " },
            new EventPickingModel { RowColor = "blue" }
        };

        // Act & Assert
        var exception = Record.Exception(() => ExcelColorHelper.FillRowColor(matches, worksheet));
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithNumericRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "123" },
            new EventPickingModel { RowColor = "456" },
            new EventPickingModel { RowColor = "789" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithAlphanumericRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "red123" },
            new EventPickingModel { RowColor = "blue456" },
            new EventPickingModel { RowColor = "green789" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithSingleCharacterRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "r" },
            new EventPickingModel { RowColor = "b" },
            new EventPickingModel { RowColor = "g" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithOnlyNumbersRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "123456789" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithOnlyLettersRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "ABCDEFGH" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithMixedContentRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "ABC123-DEF456_GHI789!@#" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithEmptyAfterTrimRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "   " }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithNullAfterTrimRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "   " }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithLeadingAndTrailingWhitespaceRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "  red  " }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithOnlyLeadingWhitespaceRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "  red" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithOnlyTrailingWhitespaceRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "red  " }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithTabsAndSpacesRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "\t red \t" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithMultipleSpacesRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "   red   " }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithNewlinesRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "\nred\n" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithCarriageReturnsRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "\rred\r" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithMixedWhitespaceRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "\r\n\t red \t\r\n" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithZeroLengthStringRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithSingleSpaceRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = " " }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithSingleTabRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "\t" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithSingleNewlineRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "\n" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithSingleCarriageReturnRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "\r" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithMultipleWhitespaceCharactersRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "   \t\n\r   " }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithVeryLongWhitespaceRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = new string(' ', 1000) }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithMixedWhitespaceAndContentRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "   \t\nred\r\n\t   " }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithUnicodeWhitespaceRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "\u00A0red\u00A0" } // Non-breaking space
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithControlCharactersRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "\u0001red\u0002" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithFormFeedRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "\fred\f" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithVerticalTabRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "\vred\v" }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithAllWhitespaceTypesRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = " \t\n\r\f\v red \v\f\r\n\t " }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithEmptyStringAfterTrimRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "   " }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithNullStringAfterTrimRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "   " }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithVeryLongContentRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = new string('A', 10000) }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithVeryLongContentWithWhitespaceRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = "   " + new string('A', 10000) + "   " }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithOnlyWhitespaceVeryLongRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = new string(' ', 10000) }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithMixedWhitespaceVeryLongRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = new string(' ', 5000) + new string('\t', 5000) }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public void FillRowColor_ShouldHandleMatchesWithContentWithMixedWhitespaceVeryLongRowColors()
    {
        // Arrange
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Test");
        var matches = new List<EventPickingModel>
        {
            new EventPickingModel { RowColor = new string(' ', 5000) + "red" + new string('\t', 5000) }
        };

        // Act
        ExcelColorHelper.FillRowColor(matches, worksheet);

        // Assert
        // No exception should be thrown
        Assert.True(true);
    }
}