namespace OWP.OtherSport.UnitTests.Validation.Helpers;

using OWP.OtherSport.Validation.Helpers;
using System.ComponentModel.DataAnnotations;
using Xunit;

public class PropertyHelperUnitTests
{
    private class MockClass
    {
        [Display(Name = "Sport")]
        public string Sport { get; set; }
    }

    private class EmptyMockClass
    {
        public string Sport { get; set; }
    }

    [Fact]
    public void GetDisplayName_NotEmptyName_ReturnName()
    {
        var type = typeof(MockClass);
        var property = type.GetProperties().FirstOrDefault();

        var actual = PropertyHelper.GetDisplayName(property);

        Assert.Equal("Sport", actual);
    }

    [Fact]
    public void GetDisplayName_EmptyName_ReturnEmptyString()
    {
        var type = typeof(EmptyMockClass);
        var property = type.GetProperties().FirstOrDefault();

        var actual = PropertyHelper.GetDisplayName(property);

        Assert.Equal(string.Empty, actual);
    }
}