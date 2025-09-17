using OWP.EventPicking.Api.Helper;
using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Constant;
using OWP.Constants;
using Xunit;

namespace OWP.EventPicking.UnitTests.Helper;

public class SpecialProductHelperUnitTest
{
    [Fact]
    public void BuildPostFixSpecialProduct_ShouldReturnEmptyString_WhenMinorBettypeIdIsNull()
    {
        // Arrange
        int? minorBettypeId = null;

        // Act
        var result = SpecialProductHelper.BuildPostFixSpecialProduct(minorBettypeId);

        // Assert
        Assert.Equal(string.Empty, result.leaguePostfix);
        Assert.Equal(string.Empty, result.teamPostfix);
    }

    [Fact]
    public void BuildPostFixSpecialProduct_ShouldReturnCornersPostfix_WhenMinorBettypeIdIsNoOfCorner()
    {
        // Arrange
        int? minorBettypeId = Bettype.NoOfCorner;

        // Act
        var result = SpecialProductHelper.BuildPostFixSpecialProduct(minorBettypeId);

        // Assert
        Assert.Equal("- CORNERS", result.leaguePostfix);
        Assert.Equal("- No.of Corners", result.teamPostfix);
    }

    [Fact]
    public void BuildPostFixSpecialProduct_ShouldReturnNextCornersPostfix_WhenMinorBettypeIdIsNextCorner()
    {
        // Arrange
        int? minorBettypeId = Bettype.NextCorner;

        // Act
        var result = SpecialProductHelper.BuildPostFixSpecialProduct(minorBettypeId);

        // Assert
        Assert.Equal("- CORNERS", result.leaguePostfix);
        Assert.Equal("- Next Corners", result.teamPostfix);
    }

    [Fact]
    public void BuildPostFixSpecialProduct_ShouldReturnBookingPostfix_WhenMinorBettypeIdIsTotalBooking()
    {
        // Arrange
        int? minorBettypeId = Bettype.TotalBooking;

        // Act
        var result = SpecialProductHelper.BuildPostFixSpecialProduct(minorBettypeId);

        // Assert
        Assert.Equal("- BOOKING", result.leaguePostfix);
        Assert.Equal("- Total Bookings", result.teamPostfix);
    }

    [Fact]
    public void BuildPostFixSpecialProduct_ShouldReturnNextBookingPostfix_WhenMinorBettypeIdIsNextBooking()
    {
        // Arrange
        int? minorBettypeId = Bettype.NextBooking;

        // Act
        var result = SpecialProductHelper.BuildPostFixSpecialProduct(minorBettypeId);

        // Assert
        Assert.Equal("- BOOKING", result.leaguePostfix);
        Assert.Equal("- Next Booking", result.teamPostfix);
    }

    [Fact]
    public void BuildPostFixSpecialProduct_ShouldReturnEmptyString_WhenMinorBettypeIdIsUnknown()
    {
        // Arrange
        int? minorBettypeId = 999; // Unknown bettype

        // Act
        var result = SpecialProductHelper.BuildPostFixSpecialProduct(minorBettypeId);

        // Assert
        Assert.Equal(string.Empty, result.leaguePostfix);
        Assert.Equal(string.Empty, result.teamPostfix);
    }


    [Fact]
    public void CombineMainAndChildMatchAndAddMatchNo_ShouldHandleEmptyLists()
    {
        // Arrange
        var mainMatches = new List<EventPickingModel>();
        var childMatches = new List<EventPickingModel>();

        // Act
        var result = SpecialProductHelper.CombineMainAndChildMatchAndAddMatchNo(mainMatches, childMatches);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void CombineMainAndChildMatchAndAddMatchNo_ShouldHandleNullLists()
    {
        // Arrange
        List<EventPickingModel> mainMatches = null!;
        List<EventPickingModel> childMatches = null!;

        // Act & Assert
        var exception = Record.Exception(() => SpecialProductHelper.CombineMainAndChildMatchAndAddMatchNo(mainMatches, childMatches));
        Assert.NotNull(exception);
    }

    [Fact]
    public void CheckMatchHaveBeenExclude_ShouldReturnTrue_WhenHomeNameContainsET()
    {
        // Arrange
        var homeName = "Team A (ET)";

        // Act
        var result = SpecialProductHelper.CheckMatchHaveBeenExclude(homeName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CheckMatchHaveBeenExclude_ShouldReturnTrue_WhenHomeNameContainsPEN()
    {
        // Arrange
        var homeName = "Team A (PEN)";

        // Act
        var result = SpecialProductHelper.CheckMatchHaveBeenExclude(homeName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CheckMatchHaveBeenExclude_ShouldReturnFalse_WhenHomeNameDoesNotContainETOrPEN()
    {
        // Arrange
        var homeName = "Team A";

        // Act
        var result = SpecialProductHelper.CheckMatchHaveBeenExclude(homeName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CheckMatchHaveBeenExclude_ShouldReturnFalse_WhenHomeNameIsNull()
    {
        // Arrange
        string homeName = null!;

        // Act
        var result = SpecialProductHelper.CheckMatchHaveBeenExclude(homeName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CheckMatchHaveBeenExclude_ShouldReturnFalse_WhenHomeNameIsEmpty()
    {
        // Arrange
        var homeName = string.Empty;

        // Act
        var result = SpecialProductHelper.CheckMatchHaveBeenExclude(homeName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CheckMatchHaveBeenExclude_ShouldReturnTrue_WhenHomeNameContainsBothETAndPEN()
    {
        // Arrange
        var homeName = "Team A (ET) (PEN)";

        // Act
        var result = SpecialProductHelper.CheckMatchHaveBeenExclude(homeName);

        // Assert
        Assert.True(result);
    }

}