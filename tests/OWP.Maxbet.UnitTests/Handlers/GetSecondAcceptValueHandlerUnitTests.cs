namespace OWP.Maxbet.UnitTests.Handlers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using NSubstitute;
    using OWP.Maxbet.Criteria;
    using OWP.Maxbet.Handlers;
    using OWP.Maxbet.Models;
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
        [InlineData((int)EnumMatchOffer.Accept6s, 6)]
        [InlineData((int)EnumMatchOffer.FastMarket, null)]
        public async Task Handler_MatchWith5sAcceptMatchOffer_Return5(int matchOfferId, int? expected)
        {
            Setup();
            IEnumerable<MatchResource> matchOffer = new List<MatchResource> { new MatchResource { ResourceId = matchOfferId } };
            dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchOfferGetCriteria>()).Returns(Task.FromResult(matchOffer));

            var actual = await handler.Handle(new GetSecondAcceptValueHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Handler_MatchWith5sAnd10sAcceptMatchOffer_ReturnNull()
        {
            Setup();
            IEnumerable<MatchResource> matchOffer = new List<MatchResource> {
                new MatchResource { ResourceId = (int)EnumMatchOffer.Accept10s },
                new MatchResource { ResourceId = (int)EnumMatchOffer.Accept5s }
            };
            dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchOfferGetCriteria>()).Returns(Task.FromResult(matchOffer));

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
            dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchOfferGetCriteria>()).Returns(Task.FromResult(matchOffer));

            var actual = await handler.Handle(new GetSecondAcceptValueHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            Assert.Equal(8, actual);
        }

        [Fact]
        public async Task Handler_MatchWithNoMatchOffer_ReturnNull()
        {
            Setup();
            IEnumerable<MatchResource> matchOffer = new List<MatchResource>() { };
            dynamicRepository.FetchAsync<MatchResource>(Arg.Compat.Any<MatchOfferGetCriteria>()).Returns(Task.FromResult(matchOffer));

            var actual = await handler.Handle(new GetSecondAcceptValueHandler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            Assert.Null(actual);
        }

        private void Setup()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            handler = new GetSecondAcceptValueHandler.Handler(dynamicRepository);
        }
    }
}