using System.ComponentModel;
using OWP.Utils;
using Xunit;

namespace OWP.UnitTests.Utils;

public class EnumUtilsUnitTests
{
    [Fact]
    public void GetDescription_WithEnumValueHavingDescription_ShouldReturnDescription()
    {
        // Arrange
        var enumValue = TestEnum.ValueWithDescription;

        // Act
        var result = EnumUtils<TestEnum>.GetDescription(enumValue);

        // Assert
        Assert.Equal("This is a test value", result);
    }

    [Fact]
    public void GetDescription_WithEnumValueWithoutDescription_ShouldReturnEmptyString()
    {
        // Arrange
        var enumValue = TestEnum.ValueWithoutDescription;

        // Act
        var result = EnumUtils<TestEnum>.GetDescription(enumValue);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetDescription_WithEnumValueAndDefaultDescription_ShouldReturnDefaultDescription()
    {
        // Arrange
        var enumValue = TestEnum.ValueWithoutDescription;
        var defaultDesc = "Default Description";

        // Act
        var result = EnumUtils<TestEnum>.GetDescription(enumValue, defaultDesc);

        // Assert
        Assert.Equal(defaultDesc, result);
    }

    [Fact]
    public void GetDescription_WithIntValue_ShouldReturnDescription()
    {
        // Arrange
        var intValue = (int)TestEnum.ValueWithDescription;

        // Act
        var result = EnumUtils<TestEnum>.GetDescription(intValue);

        // Assert
        Assert.Equal("This is a test value", result);
    }

    [Fact]
    public void GetDescription_WithInvalidIntValue_ShouldReturnEmptyString()
    {
        // Arrange
        var intValue = 999; // Invalid enum value

        // Act
        var result = EnumUtils<TestEnum>.GetDescription(intValue);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void FromDescription_WithValidDescription_ShouldReturnEnumValue()
    {
        // Arrange
        var description = "This is a test value";

        // Act
        var result = EnumUtils<TestEnum>.FromDescription(description);

        // Assert
        Assert.Equal(TestEnum.ValueWithDescription, result);
    }

    [Fact]
    public void FromDescription_WithInvalidDescription_ShouldReturnDefaultValue()
    {
        // Arrange
        var description = "Invalid Description";

        // Act
        var result = EnumUtils<TestEnum>.FromDescription(description);

        // Assert
        Assert.Equal(default(TestEnum), result);
    }

    [Fact]
    public void FromDescription_WithEmptyDescription_ShouldReturnDefaultValue()
    {
        // Arrange
        var description = string.Empty;

        // Act
        var result = EnumUtils<TestEnum>.FromDescription(description);

        // Assert
        Assert.Equal(default(TestEnum), result);
    }

    [Fact]
    public void ToEnum_WithValidString_ShouldReturnEnumValue()
    {
        // Arrange
        var stringValue = "ValueWithDescription";

        // Act
        var result = EnumUtils<TestEnum>.ToEnum(stringValue);

        // Assert
        Assert.Equal(TestEnum.ValueWithDescription, result);
    }

    [Fact]
    public void ToEnum_WithInvalidString_ShouldThrowException()
    {
        // Arrange
        var stringValue = "InvalidValue";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => EnumUtils<TestEnum>.ToEnum(stringValue));
    }

    [Fact]
    public void ToEnum_WithValidInt_ShouldReturnEnumValue()
    {
        // Arrange
        var intValue = (int)TestEnum.ValueWithDescription;

        // Act
        var result = EnumUtils<TestEnum>.ToEnum(intValue);

        // Assert
        Assert.Equal(TestEnum.ValueWithDescription, result);
    }

    [Fact]
    public void ToEnum_WithInvalidInt_ShouldReturnEnumValue()
    {
        // Arrange
        var intValue = 999; // Invalid enum value

        // Act
        var result = EnumUtils<TestEnum>.ToEnum(intValue);

        // Assert
        // This should not throw an exception, but return an invalid enum value
        Assert.Equal((TestEnum)999, result);
    }
}

// Test enum for unit testing
public enum TestEnum
{
    [Description("This is a test value")]
    ValueWithDescription = 1,

    ValueWithoutDescription = 2,

    [Description("Another test value")]
    AnotherValue = 3
}
