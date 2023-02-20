namespace OWP.Maxbet.UnitTests.Handlers
{
    using NSubstitute;

    using OWP.Maxbet.Handlers;
    using OWP.Maxbet.Models;
    using Xunit;

    public class GetSecondAcceptValueHandlerUnitTests
    {
        private MaxbetRepository dynamicRepository;

        private GetSecondAcceptValueHandler.Handler handler;

        [Theory]
        [InlineData("5s Accept", 5)]
        [InlineData("10s Accept", 10)]
        [InlineData("20s Accept", 20)]
        [InlineData("30s Accept", 30)]
        [InlineData("15s Accept", 15)]
        [InlineData("6s ACCEPT", 6)]
        public async Task Handler_MatchWith5sAcceptMatchOffer_Return5(string matchOfferId, int? expected)
        {
            Setup();
            IEnumerable<MatchResource> matchOffer = new List<MatchResource> { new() { Value = matchOfferId } };
            dynamicRepository.GetMatchOfferId(Arg.Compat.Any<int>()).Returns(matchOffer);

            var actual = await handler.Handle(new GetSecondAcceptValueHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Handler_MatchWith2MatchOffer_ReturnNull()
        {
            Setup();
            IEnumerable<MatchResource> matchOffer = new List<MatchResource>()
            {
                new() {Value = "1s Accept"},
                new() {Value = "10s Accept"},
            };
            dynamicRepository.GetMatchOfferId(Arg.Compat.Any<int>()).Returns(matchOffer);
            dynamicRepository.GetSecondAcceptValueDefaultInfo(Arg.Compat.Any<decimal?>(), Arg.Compat.Any<int>())
                .Returns((new SecondAcceptDefault(), "hello"));

            var actual = await handler.Handle(new GetSecondAcceptValueHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            Assert.Null(actual);
        }

        [Theory]
        [InlineData("FastMarket", "Fantasy", 0, 15)]
        [InlineData("KO+15", "", -3, 3)]
        [InlineData("", "", -4, 3)]
        [InlineData("", "", -6, null)]
        [InlineData("", "", 0, 10)]
        [InlineData("", "", -5, null)]
        public async Task Handler_MatchWithNoMatchOffer_ReturnOfferLineDefault(string matchOfferValue, string leagueName, int betType, int? expected)
        {
            Setup();
            IEnumerable<MatchResource> matchOffer = new List<MatchResource>() { new() { Value = matchOfferValue } };
            dynamicRepository.GetMatchOfferId(Arg.Compat.Any<int>()).Returns(matchOffer);
            dynamicRepository.GetSecondAcceptValueDefaultInfo(Arg.Any<decimal?>(), Arg.Any<int>())
                .Returns((new SecondAcceptDefault
                {
                    Main = "10s Accept",
                    Corner = "3s Accept",
                    Fantasy = "15s accept",
                    TotalBooking = "0"
                }, leagueName));

            var actual = await handler.Handle(new GetSecondAcceptValueHandler.Request { OWPMatchId = 123, Bettype = betType, MaxbetHdp = 5000 }, new CancellationToken(false));

            Assert.Equal(expected, actual);
        }

        private void Setup()
        {
            dynamicRepository = Substitute.For<MaxbetRepository>();
            handler = new GetSecondAcceptValueHandler.Handler(dynamicRepository);
        }
    }
}