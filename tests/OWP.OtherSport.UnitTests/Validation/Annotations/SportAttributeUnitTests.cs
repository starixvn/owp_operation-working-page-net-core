namespace OWP.OtherSport.UnitTests.Validation.Annotations;

using System.ComponentModel.DataAnnotations;
using OtherSport.Validation.Annotations;
using Xunit;

public class SportAttributeUnitTests
{
    private class ValidationTarget
    {
        [Sport]
        public string Sport { get; set; }
    }

    [Theory]
    [InlineData("Cricket", true)]
    [InlineData("Basketball", true)]
    [InlineData("Football", true)]
    [InlineData("Field Hockey", true)]
    [InlineData("Ice Hockey", true)]
    [InlineData("Darts", true)]
    [InlineData("Pool", true)]
    [InlineData("Table Tennis", true)]
    [InlineData("Volleyball", true)]
    [InlineData("Handball", true)]
    [InlineData("Netball", true)]
    [InlineData("Badminton", true)]
    [InlineData("Snooker", true)]
    [InlineData("Baseball", true)]
    [InlineData("Tennis", true)]
    [InlineData("Other Sports", true)]
    public void IsValid_PropWithValidSport_ReturnTrue(string sport, bool expected)
    {
        var target = new ValidationTarget { Sport = sport };
        var context = new ValidationContext(target);
        var results = new List<ValidationResult>();

        var actual = Validator.TryValidateObject(target, context, results, true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void IsValid_PropWithInvalidSport_ReturnFalse()
    {
        var target = new ValidationTarget { Sport = "Sport" };
        var context = new ValidationContext(target);
        var results = new List<ValidationResult>();

        var actual = Validator.TryValidateObject(target, context, results, true);

        Assert.False(actual);
    }
}