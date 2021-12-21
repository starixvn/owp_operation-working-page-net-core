namespace OWP.AutoAssignment.UnitTests.Services.SaveResponse
{
    using AutoAssignment.Services.InternalApi;
    using Models.InternalApi;
    using Xunit;

    public class ProviderServiceUnitTests
    {
        [Fact]
        public void Build_HaveTvAndPcCode_ReturnProvider()
        {
            var service = new ProviderServiceImp();
            var providerInfo = new ProviderInfo
            {
                OtherProvider = "TV/BGTV/RB2TV/BETTV/RTSTV",
                Provider = "helloWOO",
                ProviderTv = "TV"
            };
            var pcCode = "IH9962_PC28";
            var expected = "TV/BGTV/(WOO)/IH9962_PC28";

            var actual = service.Build(providerInfo, pcCode);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Build_NoTvAndPcCode_ReturnProvider()
        {
            var service = new ProviderServiceImp();
            var providerInfo = new ProviderInfo
            {
                OtherProvider = "TV/BGTV/RB2TV/BET/RTSTV",
                Provider = "hiAL",
                ProviderTv = null
            };
            var pcCode = string.Empty;
            var expected = "BET/(AL)";

            var actual = service.Build(providerInfo, pcCode);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "BET")]
        [InlineData("SLRAL", "BET/(AL)(SLR)")]
        public void Build_ProviderIsEmptyOrSLR_ReturnProvider(string provider, string expected)
        {
            var service = new ProviderServiceImp();
            var providerInfo = new ProviderInfo
            {
                OtherProvider = "TV/BET/RTSTV",
                Provider = provider,
                ProviderTv = string.Empty,
            };
            var pcCode = string.Empty;


            var actual = service.Build(providerInfo, pcCode);

            Assert.Equal(expected, actual);
        }

    }
}
