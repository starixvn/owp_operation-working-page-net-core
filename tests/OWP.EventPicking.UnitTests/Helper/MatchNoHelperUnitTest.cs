using OWP.EventPicking.Api.Helper;
using OWP.Constants;
using Xunit;

namespace OWP.EventPicking.UnitTests.Helper;

public class MatchNoHelperUnitTest
{
    [Fact]
    public void BuildMainMatchNo_ShouldReturnCorrectMatchNo()
    {
        // Arrange
        var matchNo = 1;

        // Act
        var result = MatchNoHelper.BuildMainMatchNo(matchNo);

        // Assert
        Assert.Equal("0001", result);
    }

    [Fact]
    public void BuildMainMatchNo_ShouldReturnCorrectMatchNoForLargeNumber()
    {
        // Arrange
        var matchNo = 1234;

        // Act
        var result = MatchNoHelper.BuildMainMatchNo(matchNo);

        // Assert
        Assert.Equal("1234", result);
    }

    [Fact]
    public void BuildMainMatchNo_ShouldReturnCorrectMatchNoForZero()
    {
        // Arrange
        var matchNo = 0;

        // Act
        var result = MatchNoHelper.BuildMainMatchNo(matchNo);

        // Assert
        Assert.Equal("0000", result);
    }

    [Fact]
    public void BuildMainMatchNo_ShouldReturnCorrectMatchNoForNegativeNumber()
    {
        // Arrange
        var matchNo = -1;

        // Act
        var result = MatchNoHelper.BuildMainMatchNo(matchNo);

        // Assert
        Assert.Equal("-0001", result);
    }

    [Fact]
    public void BuildChildMatchNo_ShouldReturnCorrectMatchNoForNoOfCorner()
    {
        // Arrange
        var matchNo = 1;
        var minorBettypeId = Bettype.NoOfCorner;

        // Act
        var result = MatchNoHelper.BuildChildMatchNo(matchNo, minorBettypeId);

        // Assert
        Assert.Equal("0001C", result);
    }

    [Fact]
    public void BuildChildMatchNo_ShouldReturnCorrectMatchNoForNextCorner()
    {
        // Arrange
        var matchNo = 1;
        var minorBettypeId = Bettype.NextCorner;

        // Act
        var result = MatchNoHelper.BuildChildMatchNo(matchNo, minorBettypeId);

        // Assert
        Assert.Equal("0001C1", result);
    }

    [Fact]
    public void BuildChildMatchNo_ShouldReturnCorrectMatchNoForTotalBooking()
    {
        // Arrange
        var matchNo = 1;
        var minorBettypeId = Bettype.TotalBooking;

        // Act
        var result = MatchNoHelper.BuildChildMatchNo(matchNo, minorBettypeId);

        // Assert
        Assert.Equal("0001TB", result);
    }

    [Fact]
    public void BuildChildMatchNo_ShouldReturnCorrectMatchNoForNextBooking()
    {
        // Arrange
        var matchNo = 1;
        var minorBettypeId = Bettype.NextBooking;

        // Act
        var result = MatchNoHelper.BuildChildMatchNo(matchNo, minorBettypeId);

        // Assert
        Assert.Equal("0001TB1", result);
    }

    [Fact]
    public void BuildChildMatchNo_ShouldReturnCorrectMatchNoForUnknownBettype()
    {
        // Arrange
        var matchNo = 1;
        var minorBettypeId = 999; // Unknown bettype

        // Act
        var result = MatchNoHelper.BuildChildMatchNo(matchNo, minorBettypeId);

        // Assert
        Assert.Equal("0001", result);
    }

    [Fact]
    public void BuildChildMatchNo_ShouldReturnCorrectMatchNoForNullMinorBettypeId()
    {
        // Arrange
        var matchNo = 1;
        int? minorBettypeId = null;

        // Act
        var result = MatchNoHelper.BuildChildMatchNo(matchNo, minorBettypeId);

        // Assert
        Assert.Equal("0001", result);
    }

    [Fact]
    public void BuildChildMatchNo_ShouldReturnCorrectMatchNoForZeroMatchNo()
    {
        // Arrange
        var matchNo = 0;
        var minorBettypeId = Bettype.NoOfCorner;

        // Act
        var result = MatchNoHelper.BuildChildMatchNo(matchNo, minorBettypeId);

        // Assert
        Assert.Equal("0000C", result);
    }

    [Fact]
    public void BuildChildMatchNo_ShouldReturnCorrectMatchNoForLargeMatchNo()
    {
        // Arrange
        var matchNo = 1234;
        var minorBettypeId = Bettype.TotalBooking;

        // Act
        var result = MatchNoHelper.BuildChildMatchNo(matchNo, minorBettypeId);

        // Assert
        Assert.Equal("1234TB", result);
    }

    [Fact]
    public void BuildChildMatchNo_ShouldReturnCorrectMatchNoForNegativeMatchNo()
    {
        // Arrange
        var matchNo = -1;
        var minorBettypeId = Bettype.NextCorner;

        // Act
        var result = MatchNoHelper.BuildChildMatchNo(matchNo, minorBettypeId);

        // Assert
        Assert.Equal("-0001C1", result);
    }
}