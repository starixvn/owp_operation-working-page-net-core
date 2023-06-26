using OWP.Extensions;
using Xunit;

namespace OWP.PreMatchAssignment.UnitTests.Misc;

public class StringExtensionUnitTests
{
    [Fact]
    public void IsContains_WithEmptyToBeSearchedString_ReturnFalse()
    {
        var toBeSearchedString = string.Empty;
        const string searchString = "abc";

        var actual = toBeSearchedString.IsContains(searchString);

        Assert.False(actual);
    }

    [Fact]
    public void IsContains_WithEmptySearchString_ReturnTrue()
    {
        const string toBeSearchedString = "abc";
        var searchString = string.Empty;

        var actual = toBeSearchedString.IsContains(searchString);

        Assert.True(actual);
    }

    [Fact]
    public void IsContains_WithNullSearchString_ReturnTrue()
    {
        const string toBeSearchedString = "abc";

        var actual = toBeSearchedString.IsContains(null);

        Assert.True(actual);
    }

    [Theory]
    [InlineData("starix14", "starix", true)]
    [InlineData("starix14", "STARIX", true)]
    [InlineData("starix14", "Rix", true)]
    [InlineData("starix14", "sa", false)]
    public void IsContains_WithNonEmpty_ReturnTrue(string toBeSearchedString, string searchString, bool expected)
    {
        var actual = toBeSearchedString.IsContains(searchString);

        Assert.Equal(expected, actual);
    }
}