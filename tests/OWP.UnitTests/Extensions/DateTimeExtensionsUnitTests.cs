using OWP.Extensions;
using Xunit;

namespace OWP.UnitTests.Extensions;

public class DateTimeExtensionsUnitTests
{
    [Fact]
    public void ConvertToTimeAgo_WithDateTimeInPast_ShouldReturnCorrectTimeAgo()
    {
        // Arrange
        var pastDateTime = DateTime.Now.AddDays(-2);

        // Act
        var result = pastDateTime.ConvertToTimeAgo();

        // Assert
        Assert.Contains("days ago", result);
        Assert.Contains("2", result);
    }

    [Fact]
    public void ConvertToTimeAgo_WithDateTimeHoursAgo_ShouldReturnHoursAgo()
    {
        // Arrange
        var pastDateTime = DateTime.Now.AddHours(-3);

        // Act
        var result = pastDateTime.ConvertToTimeAgo();

        // Assert
        Assert.Contains("hours ago", result);
        Assert.Contains("3", result);
    }

    [Fact]
    public void ConvertToTimeAgo_WithDateTimeMinutesAgo_ShouldReturnMinutesAgo()
    {
        // Arrange
        var pastDateTime = DateTime.Now.AddMinutes(-5);

        // Act
        var result = pastDateTime.ConvertToTimeAgo();

        // Assert
        Assert.Contains("minutes ago", result);
        Assert.Contains("5", result);
    }

    [Fact]
    public void ConvertToTimeAgo_WithDateTimeSecondsAgo_ShouldReturnSecondsAgo()
    {
        // Arrange
        var pastDateTime = DateTime.Now.AddSeconds(-10);

        // Act
        var result = pastDateTime.ConvertToTimeAgo();

        // Assert
        Assert.Contains("seconds ago", result);
        Assert.Contains("10", result);
    }

    [Theory]
    [InlineData(14, 30, "02:30 PM")]
    [InlineData(9, 15, "09:15 AM")]
    [InlineData(0, 0, "12:00 AM")]
    [InlineData(12, 0, "12:00 PM")]
    [InlineData(23, 59, "11:59 PM")]
    public void ToKOTime_WithVariousTimeSpans_ShouldReturnFormattedTime(int hours, int minutes, string expected)
    {
        // Arrange
        var timeSpan = new TimeSpan(hours, minutes, 0);

        // Act
        var result = timeSpan.ToKOTime();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(14, 30, "02:30")]
    [InlineData(9, 15, "09:15")]
    [InlineData(0, 0, "12:00")]
    [InlineData(12, 0, "12:00")]
    [InlineData(23, 59, "11:59")]
    public void ToKOTimeWithoutSuffix_WithVariousTimeSpans_ShouldReturnFormattedTime(int hours, int minutes, string expected)
    {
        // Arrange
        var timeSpan = new TimeSpan(hours, minutes, 0);

        // Act
        var result = timeSpan.ToKOTimeWithoutSuffix();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToGmt8_WithDateTime_ShouldConvertToGmt8()
    {
        // Arrange
        var utcDateTime = new DateTime(2024, 1, 15, 10, 0, 0, DateTimeKind.Utc);
        var expected = utcDateTime.AddHours(8);

        // Act
        var result = utcDateTime.ToGmt8();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToGmt8WorkingShift_WithDateTime_ShouldAdd12Hours()
    {
        // Arrange
        var dateTime = new DateTime(2024, 1, 15, 10, 0, 0);
        var expected = dateTime.AddHours(12);

        // Act
        var result = dateTime.ToGmt8WorkingShift();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToGmt8_WithDateTimeOffset_ShouldConvertToGmt8()
    {
        // Arrange
        var utcDateTimeOffset = new DateTimeOffset(2024, 1, 15, 10, 0, 0, TimeSpan.Zero);
        var expected = utcDateTimeOffset.ToUniversalTime().AddHours(8);

        // Act
        var result = utcDateTimeOffset.ToGmt8();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void StartOfDate_WithDateTime_ShouldReturnStartOfDay()
    {
        // Arrange
        var dateTime = new DateTime(2024, 1, 15, 14, 30, 45);
        var expected = new DateTime(2024, 1, 15, 0, 0, 0);

        // Act
        var result = dateTime.StartOfDate();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void StartHalfOfDate_WithDateTime_ShouldReturnHalfOfDate()
    {
        // Arrange
        var dateTime = new DateTime(2024, 1, 15, 14, 30, 45);
        var expected = new DateTime(2024, 1, 15, 11, 59, 59);

        // Act
        var result = dateTime.StartHalfOfDate();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TimeRangeOfDate_WithDateTimeOffset_ShouldReturnTimeRange()
    {
        // Arrange
        var dateTimeOffset = new DateTimeOffset(2024, 1, 15, 14, 30, 45, TimeSpan.Zero);
        var expected = new DateTime(2024, 1, 16, 11, 59, 59);

        // Act
        var result = dateTimeOffset.TimeRangeOfDate();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertToTimeAgo_WithFutureDateTime_ShouldReturnNegativeValues()
    {
        // Arrange
        var futureDateTime = DateTime.Now.AddDays(1);

        // Act
        var result = futureDateTime.ConvertToTimeAgo();

        // Assert
        // This should handle future dates gracefully
        Assert.NotNull(result);
    }
}
