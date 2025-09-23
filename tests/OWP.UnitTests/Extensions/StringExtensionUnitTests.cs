using OWP.Extensions;
using Xunit;

namespace OWP.UnitTests.Extensions;

public class StringExtensionUnitTests
{
    [Theory]
    [InlineData("Hello World", "world", true)]
    [InlineData("Hello World", "WORLD", true)]
    [InlineData("Hello World", "hello", true)]
    [InlineData("Hello World", "World", true)]
    [InlineData("Hello World", "xyz", false)]
    [InlineData("Hello World", "", true)]
    [InlineData("", "test", false)]
    [InlineData(null, "test", false)]
    [InlineData("Hello World", null, true)]
    public void IsContains_WithVariousInputs_ShouldReturnExpectedResult(string toBeSearchedString, string searchString, bool expected)
    {
        // Act
        var result = toBeSearchedString.IsContains(searchString);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(0, "-")]
    [InlineData(1, "1")]
    [InlineData(100, "100")]
    [InlineData(-5, "-5")]
    public void ToDashes_WithVariousInputs_ShouldReturnExpectedResult(int input, string expected)
    {
        // Act
        var result = input.ToDashes();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToFormatVn_WithValidDateTime_ShouldReturnFormattedString()
    {
        // Arrange
        var dateTime = new DateTime(2024, 1, 15);

        // Act
        var result = dateTime.ToFormatVn();

        // Assert
        Assert.Equal("15/01/2024", result);
    }

    [Fact]
    public void ToFormatDDMMyyyy_WithValidDateTime_ShouldReturnFormattedString()
    {
        // Arrange
        var dateTime = new DateTime(2024, 1, 15);

        // Act
        var result = dateTime.ToFormatDDMMyyyy();

        // Assert
        Assert.Equal("15012024", result);
    }

    [Theory]
    [InlineData("Hello World", @"\s", "_", "Hello_World")]
    [InlineData("Test123", @"\d", "", "Test")]
    [InlineData("abc", @"[a-z]", "X", "XXX")]
    [InlineData("", @"\w", "X", "")]
    public void ReplaceWith_WithVariousInputs_ShouldReturnExpectedResult(string text, string regex, string replacement, string expected)
    {
        // Act
        var result = text.ReplaceWith(regex, replacement);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Hello World Test", new[] { "Hello", "World" }, true)]
    [InlineData("Hello World Test", new[] { "Hello", "Test" }, true)]
    [InlineData("Hello World Test", new[] { "Hello", "World", "Test" }, true)]
    [InlineData("Hello World Test", new[] { "Hello", "XYZ" }, false)]
    [InlineData("Hello World Test", new[] { "XYZ", "ABC" }, false)]
    [InlineData("", new[] { "test" }, false)]
    [InlineData(null, new[] { "test" }, false)]
    public void IsContainsAll_WithVariousInputs_ShouldReturnExpectedResult(string toBeSearchedString, string[] searchStrings, bool expected)
    {
        // Act
        var result = toBeSearchedString.IsContainsAll(searchStrings);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("123", 123)]
    [InlineData("0", 0)]
    [InlineData("-456", -456)]
    [InlineData("abc", null)]
    [InlineData("", null)]
    [InlineData(null, null)]
    [InlineData("12.34", null)]
    public void ConvertNullableInt_WithVariousInputs_ShouldReturnExpectedResult(string text, int? expected)
    {
        // Act
        var result = text.ConvertNullableInt();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Hello123World", "HelloWorld")]
    [InlineData("Test!@#456", "Test")]
    [InlineData("ABC", "ABC")]
    [InlineData("123456", "")]
    [InlineData("", "")]
    [InlineData(null, "")]
    public void RemoveNonAlphaCharacters_WithVariousInputs_ShouldReturnExpectedResult(string source, string expected)
    {
        // Act
        var result = StringExtension.RemoveNonAlphaCharacters(source);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("  test  ", "test")]
    [InlineData("test", "test")]
    [InlineData("", "")]
    [InlineData(null, "")]
    [InlineData("   ", "")]
    public void ConvertNullToEmpty_WithVariousInputs_ShouldReturnExpectedResult(string value, string expected)
    {
        // Act
        var result = StringExtension.ConvertNullToEmpty(value);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ParseXml_WithValidXml_ShouldReturnDeserializedObject()
    {
        // Arrange
        var xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><TestClass><Name>Test</Name><Value>123</Value></TestClass>";

        // Act
        var result = xml.ParseXml<TestClass>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.Name);
        Assert.Equal(123, result.Value);
    }

    [Fact]
    public void ParseXml_WithInvalidXml_ShouldReturnNull()
    {
        // Arrange
        var invalidXml = "invalid xml content";

        // Act
        var result = invalidXml.ParseXml<TestClass>();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ParseXml_WithEmptyXml_ShouldReturnNull()
    {
        // Arrange
        var emptyXml = "";

        // Act
        var result = emptyXml.ParseXml<TestClass>();

        // Assert
        Assert.Null(result);
    }
}

// Test class for XML parsing tests
public class TestClass
{
    public string Name { get; set; }
    public int Value { get; set; }
}
