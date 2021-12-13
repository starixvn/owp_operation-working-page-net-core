using OWP.AutoAssignment.Services.PublicApi;
using Xunit;

namespace OWP.AutoAssignment.UnitTests.Services.PublicApi
{
    public class ProviderDetectorServiceUnitTests
    {
        [Theory]
        [InlineData(new object[] { "BG/RB2/BET/RTS", new int[] { 1 } })]
        [InlineData(new object[] { "RB2/BG/BET/RTS", new int[] { 2 } })]
        [InlineData(new object[] { "RTS/RB2/BET/BET", new int[] { 3 } })]
        [InlineData(new object[] { "BET/RTS", new int[] { 4 } })]
        [InlineData(new object[] { "betf", new int[] { 0 } })]
        [InlineData(new object[] { "abc/BET", new int[] { 4 } })]
        public void DetectIndexAndOrderProviderSpecial_ReturnProviderCorrect(string provider, int[] expected)
        {
            var service = new ProviderDetectorServiceImp();
            var actual = service.DetectProviderType(provider);
            Assert.Equal(expected, actual);
        }
    }
}