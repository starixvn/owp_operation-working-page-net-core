using OWP.AutoAssignment.Services.PublicApi;
using Xunit;

namespace OWP.AutoAssignment.UnitTests.Services.PublicApi
{
    public class ProviderDetectorServiceUnitTests
    {
        [Theory]
        [InlineData(new object[] { "ipsd", new int[] { 1 } })]
        [InlineData(new object[] { "bg", new int[] { 1 } })]
        [InlineData(new object[] { "rb2", new int[] { 2 } })]
        [InlineData(new object[] { "rts", new int[] { 3 } })]
        [InlineData(new object[] { "bet", new int[] { 4 } })]
        [InlineData(new object[] { "abc", new int[] { 0 } })]
        [InlineData(new object[] { "ipsd,bg,rb2,rts,bet", new int[] { 1, 2, 3, 4 } })]
        public void DetectIndexAndOrderProviderSpecial_ReturnProviderCorrect(string provider, int[] expected)
        {
            var service = new ProviderDetectorServiceImp();
            var actual = service.DetectProviderType(provider);
            Assert.Equal(expected, actual);
        }
    }
}