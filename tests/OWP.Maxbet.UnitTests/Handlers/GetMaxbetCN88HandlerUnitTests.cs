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
    using Xunit.Asserts.Compare;

    public class GetMaxbetCN88HandlerUnitTests
    {
        private IDynamicRepository dynamicRepository;

        private GetMaxbetCN88Handler.Handler handler;

        [Fact]
        public async Task Handler_WithAllCN88MaxbetExisted_ReturnData()
        {
            Setup();
            IEnumerable<MaxbetModel> maxbet = new List<MaxbetModel> {
                new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 400000 },
                new MaxbetModel { BettypeId = (int)EnumBettype.OverUnder, Value = 100000 },
                new MaxbetModel { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 200000 },
                new MaxbetModel { BettypeId = (int)EnumBettype.FirstHalfOverUnder, Value = 300000 }
            };
            dynamicRepository.FetchAsync<MaxbetModel>(Arg.Compat.Is<MaxbetGetCriteria>(x => x.IsCN88)).Returns(Task.FromResult(maxbet));
            var expected = new MaxbetSpread { Handicap = 400000, OverUnder = 100000, FirstHalfHandicap = 200000, FirstHalfOverUnder = 300000 };

            var actual = await handler.Handle(new GetMaxbetCN88Handler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public async Task Handler_WithNoCN88MaxbetExistedAndNovaMaxbetIsGreaterThan50000_ReturnData()
        {
            Setup();
            IEnumerable<MaxbetModel> cn88Maxbet = new List<MaxbetModel>();
            IEnumerable<MaxbetModel> novaMaxbet = new List<MaxbetModel> {
                new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 400000 },
            };
            dynamicRepository.FetchAsync<MaxbetModel>(Arg.Compat.Is<MaxbetGetCriteria>(x => x.IsCN88)).Returns(Task.FromResult(cn88Maxbet));
            dynamicRepository.FetchAsync<MaxbetModel>(Arg.Compat.Is<MaxbetGetCriteria>(x => !x.IsCN88)).Returns(Task.FromResult(novaMaxbet));
            var expected = new MaxbetSpread { Handicap = 200000, OverUnder = 200000, FirstHalfHandicap = 60000, FirstHalfOverUnder = 60000 };

            var actual = await handler.Handle(new GetMaxbetCN88Handler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public async Task Handler_WithNoCN88MaxbetExistedAndNovaMaxbetIsGreaterThan50000ButNotExistsInDict_ReturnData()
        {
            Setup();
            IEnumerable<MaxbetModel> cn88Maxbet = new List<MaxbetModel>();
            IEnumerable<MaxbetModel> novaMaxbet = new List<MaxbetModel> {
                new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 300000 },
            };
            dynamicRepository.FetchAsync<MaxbetModel>(Arg.Compat.Is<MaxbetGetCriteria>(x => x.IsCN88)).Returns(Task.FromResult(cn88Maxbet));
            dynamicRepository.FetchAsync<MaxbetModel>(Arg.Compat.Is<MaxbetGetCriteria>(x => !x.IsCN88)).Returns(Task.FromResult(novaMaxbet));
            var expected = new MaxbetSpread { Handicap = 300000, OverUnder = 300000, FirstHalfHandicap = 300000, FirstHalfOverUnder = 300000 };

            var actual = await handler.Handle(new GetMaxbetCN88Handler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public async Task Handler_WithNoCN88MaxbetExistedAndNovaMaxbetIsLowerThan50000_ReturnData()
        {
            Setup();
            IEnumerable<MaxbetModel> cn88Maxbet = new List<MaxbetModel>();
            IEnumerable<MaxbetModel> novaMaxbet = new List<MaxbetModel> {
                new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 40000 },
            };
            dynamicRepository.FetchAsync<MaxbetModel>(Arg.Compat.Is<MaxbetGetCriteria>(x => x.IsCN88)).Returns(Task.FromResult(cn88Maxbet));
            dynamicRepository.FetchAsync<MaxbetModel>(Arg.Compat.Is<MaxbetGetCriteria>(x => !x.IsCN88)).Returns(Task.FromResult(novaMaxbet));
            var expected = new MaxbetSpread { Handicap = 40000, OverUnder = 40000, FirstHalfHandicap = 25000, FirstHalfOverUnder = 25000 };

            var actual = await handler.Handle(new GetMaxbetCN88Handler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public async Task Handler_WithNoCN88MaxbetExistedAndNovaMaxbetIsLowerThan50000ButNotExistsInDict_ReturnData()
        {
            Setup();
            IEnumerable<MaxbetModel> cn88Maxbet = new List<MaxbetModel>();
            IEnumerable<MaxbetModel> novaMaxbet = new List<MaxbetModel> {
                new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 45000 },
            };
            dynamicRepository.FetchAsync<MaxbetModel>(Arg.Compat.Is<MaxbetGetCriteria>(x => x.IsCN88)).Returns(Task.FromResult(cn88Maxbet));
            dynamicRepository.FetchAsync<MaxbetModel>(Arg.Compat.Is<MaxbetGetCriteria>(x => !x.IsCN88)).Returns(Task.FromResult(novaMaxbet));
            var expected = new MaxbetSpread { Handicap = 45000, OverUnder = 45000, FirstHalfHandicap = 45000, FirstHalfOverUnder = 45000 };

            var actual = await handler.Handle(new GetMaxbetCN88Handler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public async Task Handler_WithNoCN88MaxbetAndNoNovaMaxbetExisted_ReturnData()
        {
            Setup();
            IEnumerable<MaxbetModel> cn88Maxbet = new List<MaxbetModel>();
            IEnumerable<MaxbetModel> novaMaxbet = new List<MaxbetModel>();
            dynamicRepository.FetchAsync<MaxbetModel>(Arg.Compat.Is<MaxbetGetCriteria>(x => x.IsCN88)).Returns(Task.FromResult(cn88Maxbet));
            dynamicRepository.FetchAsync<MaxbetModel>(Arg.Compat.Is<MaxbetGetCriteria>(x => !x.IsCN88)).Returns(Task.FromResult(novaMaxbet));
            var expected = new MaxbetSpread();

            var actual = await handler.Handle(new GetMaxbetCN88Handler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public async Task Handler_WithNoCN88MaxbetHDPExistedAndNovaMaxbetIsGreaterThan40000_ReturnData()
        {
            Setup();
            IEnumerable<MaxbetModel> cn88Maxbet = new List<MaxbetModel>()  {
                new MaxbetModel { BettypeId = (int)EnumBettype.OverUnder, Value = 100000 },
                new MaxbetModel { BettypeId = (int)EnumBettype.FirstHalfHandicap, Value = 200000 },
                new MaxbetModel { BettypeId = (int)EnumBettype.FirstHalfOverUnder, Value = 300000 }
            };
            IEnumerable<MaxbetModel> novaMaxbet = new List<MaxbetModel>() {
                new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 100000 },
            };
            dynamicRepository.FetchAsync<MaxbetModel>(Arg.Compat.Is<MaxbetGetCriteria>(x => x.IsCN88)).Returns(Task.FromResult(cn88Maxbet));
            dynamicRepository.FetchAsync<MaxbetModel>(Arg.Compat.Is<MaxbetGetCriteria>(x => !x.IsCN88)).Returns(Task.FromResult(novaMaxbet));
            var expected = new MaxbetSpread() { Handicap = 80000, OverUnder = 100000, FirstHalfHandicap = 200000, FirstHalfOverUnder = 300000 };

            var actual = await handler.Handle(new GetMaxbetCN88Handler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public async Task Handler_WithNoCN88Maxbet1HHDPExistedAndNovaMaxbetIsGreaterThan40000_ReturnData()
        {
            Setup();
            IEnumerable<MaxbetModel> cn88Maxbet = new List<MaxbetModel>()  {
                new MaxbetModel { BettypeId = (int)EnumBettype.OverUnder, Value = 100000 },
            };
            IEnumerable<MaxbetModel> novaMaxbet = new List<MaxbetModel>() {
                new MaxbetModel { BettypeId = (int)EnumBettype.Handicap, Value = 100000 },
            };
            dynamicRepository.FetchAsync<MaxbetModel>(Arg.Compat.Is<MaxbetGetCriteria>(x => x.IsCN88)).Returns(Task.FromResult(cn88Maxbet));
            dynamicRepository.FetchAsync<MaxbetModel>(Arg.Compat.Is<MaxbetGetCriteria>(x => !x.IsCN88)).Returns(Task.FromResult(novaMaxbet));
            var expected = new MaxbetSpread() { Handicap = 80000, OverUnder = 100000, FirstHalfHandicap = 30000, FirstHalfOverUnder = 30000 };

            var actual = await handler.Handle(new GetMaxbetCN88Handler.Request { OWPMatchId = 123 }, new CancellationToken(false));

            DeepAssert.Equal(expected, actual);
        }

        private void Setup()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            handler = new GetMaxbetCN88Handler.Handler(dynamicRepository);
        }
    }
}