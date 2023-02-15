namespace OWP.AutoAssignment.UnitTests.Services.PublicApi;

using AutoAssignment.Services.PublicApi;
using Xunit;

public class ProviderDetectorServiceUnitTests
{
    [Theory]
    [InlineData("BGTV/RB2TV/BETTV/RTSTV", new[] { 1 })]
    [InlineData("BGTV", new[] { 1 })]
    [InlineData("BG/RB2/BET/RTS", new[] { 1 })]
    [InlineData("RB2/BG/BET/RTS", new[] { 2 })]
    [InlineData("RTS/RB2/BET/BET", new[] { 3 })]
    [InlineData("BET/RTS", new[] { 4 })]
    [InlineData("betf", new[] { 0 })]
    [InlineData("abc/BET", new[] { 4 })]
    [InlineData("", new[] { 0 })]
    public void DetectIndexAndOrderProviderSpecial_ReturnProviderCorrect(string provider, int[] expected)
    {
        var service = new ProviderDetectorServiceImp();
        var actual = service.DetectProviderType(provider);
        Assert.Equal(expected, actual);
    }
}