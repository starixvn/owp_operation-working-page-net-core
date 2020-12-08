namespace OWP.API.SyncMaxbet.UnitTests.Handlers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using NSubstitute;
    using OWP.API.SyncMaxbet.Criterias;
    using OWP.API.SyncMaxbet.Handlers;
    using OWP.API.SyncMaxbet.Models;
    using OWP.Enums;
    using Xunit;

    public class GetSecondAcceptValueHandlerUnitTests
    {
        private IDynamicRepository dynamicRepository;

        private GetSecondAcceptValueHandler.Handler handler;

        [Theory]
        [InlineData((int)EnumMatchOffer.Accept5s, 5)]
        [InlineData((int)EnumMatchOffer.Accept10s, 10)]
        [InlineData((int)EnumMatchOffer.Accept20s, 20)]
        [InlineData((int)EnumMatchOffer.Accept30s, 30)]
        [InlineData((int)EnumMatchOffer.Accept15s, 15)]
        [InlineData((int)EnumMatchOffer.FastMarket, 0)]
        public async Task Handler_MatchWith5sAcceptMatchOffer_Return5(int matchOfferId, int expected)
        {
            Setup();
            IEnumerable<MatchResource> matchOffer = new List<MatchResource> { new MatchResource { ResourceId = matchOfferId } };
            dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchOfferGetCriteria>()).Returns(Task.FromResult(matchOffer));

            var actual = await handler.Handle(new GetSecondAcceptValueHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Handler_MatchWith5sAnd10sAcceptMatchOffer_Return10()
        {
            Setup();
            IEnumerable<MatchResource> matchOffer = new List<MatchResource> {
                new MatchResource { ResourceId = (int)EnumMatchOffer.Accept10s },
                new MatchResource { ResourceId = (int)EnumMatchOffer.Accept5s }
            };
            dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchOfferGetCriteria>()).Returns(Task.FromResult(matchOffer));

            var actual = await handler.Handle(new GetSecondAcceptValueHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            Assert.Equal(10, actual);
        }

        private void Setup()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            handler = new GetSecondAcceptValueHandler.Handler(dynamicRepository);
        }
    }
}