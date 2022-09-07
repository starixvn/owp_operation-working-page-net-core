namespace OWP.Maxbet.UnitTests.Handlers
{
    using Fanex.Data.Repository;
    using NSubstitute;
    using OWP.Enums;
    using OWP.Maxbet.Criteria;
    using OWP.Maxbet.Handlers;
    using OWP.Maxbet.Models;
    using Xunit;

    public class GetSecondAcceptValueHandlerUnitTests
    {
        private MaxbetRepository dynamicRepository;

        private GetSecondAcceptValueHandler.Handler handler;

        [Theory]
        [InlineData((int)EnumMatchOffer.Accept5s, 5)]
        [InlineData((int)EnumMatchOffer.Accept10s, 10)]
        [InlineData((int)EnumMatchOffer.Accept20s, 20)]
        [InlineData((int)EnumMatchOffer.Accept30s, 30)]
        [InlineData((int)EnumMatchOffer.Accept15s, 15)]
        [InlineData((int)EnumMatchOffer.Accept6s, 6)]
        [InlineData((int)EnumMatchOffer.FastMarket, null)]
        public async Task Handler_MatchWith5sAcceptMatchOffer_Return5(int matchOfferId, int? expected)
        {
            Setup();
            IEnumerable<MatchResource> matchOffer = new List<MatchResource> { new MatchResource { ResourceId = matchOfferId } };
            dynamicRepository.GetMatchOfferId(Arg.Compat.Any<int>()).Returns(matchOfferId);

            var actual = await handler.Handle(new GetSecondAcceptValueHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Handler_MatchWith5sAnd10sAcceptMatchOffer_ReturnNull()
        {
            Setup();

            dynamicRepository.GetMatchOfferId(Arg.Compat.Any<int>()).Returns(2);

            var actual = await handler.Handle(new GetSecondAcceptValueHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            Assert.Null(actual);
        }

        [Fact]
        public async Task Handler_MatchWith8sAcceptMatchOffer_Return8()
        {
            Setup();
            IEnumerable<MatchResource> matchOffer = new List<MatchResource> {
                new MatchResource { ResourceId = (int)EnumMatchOffer.Accept8s }
            };
            dynamicRepository.GetMatchOfferId(Arg.Compat.Any<int>()).Returns(3);

            var actual = await handler.Handle(new GetSecondAcceptValueHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            Assert.Equal(10, actual);
        }

        [Fact]
        public async Task Handler_MatchWithNoMatchOffer_ReturnNull()
        {
            Setup();
            IEnumerable<MatchResource> matchOffer = new List<MatchResource>() { };
            dynamicRepository.GetMatchOfferId(Arg.Compat.Any<int>()).Returns(0);

            var actual = await handler.Handle(new GetSecondAcceptValueHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            Assert.Null(actual);
        }

        private void Setup()
        {
            dynamicRepository = Substitute.For<MaxbetRepository>();
            handler = new GetSecondAcceptValueHandler.Handler(dynamicRepository);
        }
    }
}