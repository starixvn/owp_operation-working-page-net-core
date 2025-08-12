using System;
using OWP.EventPicking;
using OWP.EventPicking.Api.Models;
using Xunit;

namespace OWP.EventPicking.UnitTests.Utils;

public class UtilsUnitTests
{
    [Fact]
    public void GetMiscMatchTemplate_ShouldReturnCorrectPath()
    {
        // Arrange
        var currentDirectory = @"C:\TestApp";
        var expectedPath = @"C:\TestApp\App_Data\Templates\EventPickingNewFormat.xlsx";

        // Act
        var result = Utils.GetMiscMatchTemplate(currentDirectory);

        // Assert
        Assert.Equal(expectedPath, result);
    }

    [Fact]
    public void GetMiscMatchTemplate_WithUnixStylePath_ShouldReturnCorrectPath()
    {
        // Arrange
        var currentDirectory = "/home/testapp";
        var expectedPath = "/home/testapp/App_Data/Templates/EventPickingNewFormat.xlsx";

        // Act
        var result = Utils.GetMiscMatchTemplate(currentDirectory);

        // Assert
        Assert.Equal(expectedPath, result);
    }

    [Fact]
    public void CalculateValueTurnover_WithPositiveTurnover_ShouldSetCorrectValues()
    {
        // Arrange
        var turnoverWinLoss = new TurnOverWinLossModel
        {
            Turnover = 1234.56
        };
        var turnoverInfo = new TurnOverInfo();

        // Act
        Utils.CalculateValueTurnover(turnoverWinLoss, turnoverInfo);

        // Assert
        Assert.Equal("1,234.56", turnoverInfo.ValueDisplay);
        Assert.Equal(1234.56, turnoverInfo.Value);
        Assert.Null(turnoverInfo.Color); // No color for positive values
    }

    [Fact]
    public void CalculateValueTurnover_WithNegativeTurnover_ShouldSetCorrectValuesAndColor()
    {
        // Arrange
        var turnoverWinLoss = new TurnOverWinLossModel
        {
            Turnover = -567.89
        };
        var turnoverInfo = new TurnOverInfo();

        // Act
        Utils.CalculateValueTurnover(turnoverWinLoss, turnoverInfo);

        // Assert
        Assert.Equal("567.89", turnoverInfo.ValueDisplay);
        Assert.Equal(-567.89, turnoverInfo.Value);
        Assert.Equal("red", turnoverInfo.Color);
    }

    [Fact]
    public void CalculateValueTurnover_WithZeroTurnover_ShouldSetZeroValues()
    {
        // Arrange
        var turnoverWinLoss = new TurnOverWinLossModel
        {
            Turnover = 0.0
        };
        var turnoverInfo = new TurnOverInfo();

        // Act
        Utils.CalculateValueTurnover(turnoverWinLoss, turnoverInfo);

        // Assert
        Assert.Equal("0.00", turnoverInfo.ValueDisplay);
        Assert.Equal(0.0, turnoverInfo.Value);
        Assert.Null(turnoverInfo.Color);
    }

    [Fact]
    public void CalculateValueTurnover_WithVerySmallTurnover_ShouldSetZeroValues()
    {
        // Arrange
        var turnoverWinLoss = new TurnOverWinLossModel
        {
            Turnover = 0.0001 // Very small value
        };
        var turnoverInfo = new TurnOverInfo();

        // Act
        Utils.CalculateValueTurnover(turnoverWinLoss, turnoverInfo);

        // Assert
        Assert.Equal("0.00", turnoverInfo.ValueDisplay);
        Assert.Equal(0.0, turnoverInfo.Value);
        Assert.Null(turnoverInfo.Color);
    }

    [Fact]
    public void CalculateValueTurnover_WithNullTurnoverWinLoss_ShouldNotModifyTurnoverInfo()
    {
        // Arrange
        TurnOverWinLossModel? turnoverWinLoss = null;
        var turnoverInfo = new TurnOverInfo
        {
            ValueDisplay = "Original",
            Value = 999.0,
            Color = "blue"
        };

        // Act
        Utils.CalculateValueTurnover(turnoverWinLoss, turnoverInfo);

        // Assert
        Assert.Equal("Original", turnoverInfo.ValueDisplay);
        Assert.Equal(999.0, turnoverInfo.Value);
        Assert.Equal("blue", turnoverInfo.Color);
    }

    [Fact]
    public void CalculateValueWinLoss_WithPositiveWinLoss_ShouldSetCorrectValues()
    {
        // Arrange
        var turnoverWinLoss = new TurnOverWinLossModel
        {
            Turnover = 1000.0,
            WinLoss = 100.0
        };
        var marginInfo = new MarginInfo();

        // Act
        Utils.CalculateValueWinLoss(turnoverWinLoss, marginInfo);

        // Assert
        Assert.Equal("0.100", marginInfo.ValueDisplay);
        Assert.Equal(0.1, marginInfo.Value);
        Assert.Null(marginInfo.Color); // No color for positive values
    }

    [Fact]
    public void CalculateValueWinLoss_WithNegativeWinLoss_ShouldSetCorrectValuesAndColor()
    {
        // Arrange
        var turnoverWinLoss = new TurnOverWinLossModel
        {
            Turnover = 1000.0,
            WinLoss = -150.0
        };
        var marginInfo = new MarginInfo();

        // Act
        Utils.CalculateValueWinLoss(turnoverWinLoss, marginInfo);

        // Assert
        Assert.Equal("-0.150", marginInfo.ValueDisplay);
        Assert.Equal(-0.15, marginInfo.Value);
        Assert.Equal("red", marginInfo.Color);
    }

    [Fact]
    public void CalculateValueWinLoss_WithZeroWinLoss_ShouldSetDashDisplay()
    {
        // Arrange
        var turnoverWinLoss = new TurnOverWinLossModel
        {
            Turnover = 1000.0,
            WinLoss = 0.0
        };
        var marginInfo = new MarginInfo();

        // Act
        Utils.CalculateValueWinLoss(turnoverWinLoss, marginInfo);

        // Assert
        Assert.Equal("-", marginInfo.ValueDisplay);
        Assert.Equal(0.0, marginInfo.Value);
        Assert.Null(marginInfo.Color);
    }

    [Fact]
    public void CalculateValueWinLoss_WithVerySmallWinLoss_ShouldSetZeroValues()
    {
        // Arrange
        var turnoverWinLoss = new TurnOverWinLossModel
        {
            Turnover = 1000.0,
            WinLoss = 0.0001 // Very small value
        };
        var marginInfo = new MarginInfo();

        // Act
        Utils.CalculateValueWinLoss(turnoverWinLoss, marginInfo);

        // Assert
        Assert.Equal("0.000", marginInfo.ValueDisplay);
        Assert.Equal(0.0, marginInfo.Value);
        Assert.Null(marginInfo.Color);
    }

    [Fact]
    public void CalculateValueWinLoss_WithNullTurnoverWinLoss_ShouldNotModifyMarginInfo()
    {
        // Arrange
        TurnOverWinLossModel? turnoverWinLoss = null;
        var marginInfo = new MarginInfo
        {
            ValueDisplay = "Original",
            Value = 0.5,
            Color = "green"
        };

        // Act
        Utils.CalculateValueWinLoss(turnoverWinLoss, marginInfo);

        // Assert
        Assert.Equal("Original", marginInfo.ValueDisplay);
        Assert.Equal(0.5, marginInfo.Value);
        Assert.Equal("green", marginInfo.Color);
    }

    [Fact]
    public void CalculateValueWinLoss_WithZeroTurnover_ShouldHandleDivisionByZero()
    {
        // Arrange
        var turnoverWinLoss = new TurnOverWinLossModel
        {
            Turnover = 0.0,
            WinLoss = 100.0
        };
        var marginInfo = new MarginInfo();

        // Act
        Utils.CalculateValueWinLoss(turnoverWinLoss, marginInfo);

        // Assert
        // Should handle division by zero gracefully
        Assert.Equal("0.000", marginInfo.ValueDisplay);
        Assert.Equal(0.0, marginInfo.Value);
        Assert.Null(marginInfo.Color);
    }

    [Fact]
    public void CalculateValueTurnover_WithRoundingEdgeCases_ShouldHandleCorrectly()
    {
        // Arrange
        var turnoverWinLoss = new TurnOverWinLossModel
        {
            Turnover = 123.456789
        };
        var turnoverInfo = new TurnOverInfo();

        // Act
        Utils.CalculateValueTurnover(turnoverWinLoss, turnoverInfo);

        // Assert
        Assert.Equal("123.46", turnoverInfo.ValueDisplay); // Should round to 2 decimal places
        Assert.Equal(123.46, turnoverInfo.Value);
    }

    [Fact]
    public void CalculateValueWinLoss_WithRoundingEdgeCases_ShouldHandleCorrectly()
    {
        // Arrange
        var turnoverWinLoss = new TurnOverWinLossModel
        {
            Turnover = 1000.0,
            WinLoss = 123.456789
        };
        var marginInfo = new MarginInfo();

        // Act
        Utils.CalculateValueWinLoss(turnoverWinLoss, marginInfo);

        // Assert
        Assert.Equal("0.123", marginInfo.ValueDisplay); // Should round to 3 decimal places
        Assert.Equal(0.123, marginInfo.Value);
    }
}
