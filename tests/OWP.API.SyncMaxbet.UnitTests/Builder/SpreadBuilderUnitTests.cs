using NSubstitute;
using OWP.API.SyncMaxbet.Builder;
using OWP.API.SyncMaxbet.Models;
using OWP.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.API.SyncMaxbet.UnitTests.Builder
{
    public class SpreadBuilderUnitTests
    {
        private readonly SyncMaxbetReposiory syncMaxbetReposiory;

        public SpreadBuilderUnitTests()
        {
            syncMaxbetReposiory = Substitute.For<SyncMaxbetReposiory>();
        }

        [Fact]
        public async Task BuildSpread_NotFound_HDP_OU_1HHDP_1HOU_ReturnDefaultMaxbetSpread()
        {
            var spreadTupple = new ValueTuple<IEnumerable<SpreadModel>, IEnumerable<int>>();
            spreadTupple.Item1 = new List<SpreadModel>();

            var spreadBuilder = new SpreadBuilderImp(syncMaxbetReposiory);
            syncMaxbetReposiory
                .GetSpreadByOwpmatchId(123)
                .ReturnsForAnyArgs(Task.FromResult(spreadTupple));

            var expectedResult = await spreadBuilder.BuildSpread(123);

            DeepAssert.Equal(expectedResult, new MaxbetSpread());
        }

        [Fact]
        public async Task BuildSpread_Found_HDP_OU_1HHDP_1HOU_ReturnMaxbetSpreadCorrect()
        {
            var spreadTupple = new ValueTuple<IEnumerable<SpreadModel>, IEnumerable<int>>();
            spreadTupple.Item1 = new List<SpreadModel>
            {
                new SpreadModel
                {
                    BettypeId= (int)Bettype.Handicap,
                    Spread=(decimal)0.1
                },
                new SpreadModel
                {
                    BettypeId= (int)Bettype.OverUnder,
                    Spread=(decimal)0.2
                },
                 new SpreadModel
                {
                    BettypeId= (int)Bettype.FirstHalfHandicap,
                    Spread=(decimal)0.3
                },
                new SpreadModel
                {
                    BettypeId= (int)Bettype.FirstHalfOverUnder,
                    Spread=(decimal)0.4
                }
            };



            var spreadBuilder = new SpreadBuilderImp(syncMaxbetReposiory);
            syncMaxbetReposiory
                .GetSpreadByOwpmatchId(123)
                .ReturnsForAnyArgs(Task.FromResult(spreadTupple));

            var result = await spreadBuilder.BuildSpread(123);

            var expected = new MaxbetSpread
            {
                Handicap = (decimal)0.1,
                OverUnder = (decimal)0.2,
                FirstHalfHandicap = (decimal)0.3,
                FirstHalfOverUnder = (decimal)0.4
            };

            DeepAssert.Equal(expected, result);
        }

        [Fact]
        public async Task BuildSpread_ForGroupMJ_NotExists_1HHDP_1HOU_ReturnSpreadHasSpread_1HHDP_1HOU_Follow_HDP_OU()
        {
            var spreadTupple = new ValueTuple<IEnumerable<SpreadModel>, IEnumerable<int>>();
            spreadTupple.Item1 = new List<SpreadModel>
            {
                new SpreadModel
                {
                    BettypeId= (int)Bettype.Handicap,
                    Spread=(decimal)0.1
                },
                new SpreadModel
                {
                    BettypeId= (int)Bettype.OverUnder,
                    Spread=(decimal)0.2
                }
            };

            spreadTupple.Item2 = new List<int> { 1 };

            var spreadBuilder = new SpreadBuilderImp(syncMaxbetReposiory);
            syncMaxbetReposiory
                .GetSpreadByOwpmatchId(123)
                .ReturnsForAnyArgs(Task.FromResult(spreadTupple));

            var result = await spreadBuilder.BuildSpread(123);
            var expected = new MaxbetSpread
            {
                Handicap = (decimal)0.1,
                OverUnder = (decimal)0.2,
                FirstHalfHandicap = (decimal)0.1,
                FirstHalfOverUnder = (decimal)0.2
            };

            DeepAssert.Equal(expected, result);
        }

        [Fact]
        public async Task BuildSpread_ForGroupNotMJ_NotExists_1HHDP_1HOU_ReturnSpreadHasSpread_1HHDP_1HOU_Follow_HDP_OU()
        {
            var spreadTupple = new ValueTuple<IEnumerable<SpreadModel>, IEnumerable<int>>();
            spreadTupple.Item1 = new List<SpreadModel>
            {
                new SpreadModel
                {
                    BettypeId= (int)Bettype.Handicap,
                    Spread=(decimal)0.1
                },
                new SpreadModel
                {
                    BettypeId= (int)Bettype.OverUnder,
                    Spread=(decimal)0.1
                }
            };

            spreadTupple.Item2 = new List<int> { 2 };

            var spreadBuilder = new SpreadBuilderImp(syncMaxbetReposiory);
            syncMaxbetReposiory
                .GetSpreadByOwpmatchId(123)
                .ReturnsForAnyArgs(Task.FromResult(spreadTupple));

            var result = await spreadBuilder.BuildSpread(123);
            var expected = new MaxbetSpread
            {
                Handicap = (decimal)0.1,
                OverUnder = (decimal)0.1,
                FirstHalfHandicap = (decimal)0.1,
                FirstHalfOverUnder = (decimal)0.1
            };

            DeepAssert.Equal(expected, result);
        }

        [Fact]
        public void BuildMaxbetSpread_NotExistsHDP_OU_1HHDP_1HOU_ReturnEmptySpread()
        {
            var spreadBuilder = new SpreadBuilderImp(syncMaxbetReposiory);
            var spread = new List<SpreadModel>
            {
                new SpreadModel()
            };

            var actual = spreadBuilder.BuildSpreadSender(spread);
            var expected = new MaxbetSpread();
            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void BuildMaxbetSpread_ExistsHDP_OU_1HHDP_1HOU_ReturnSpread()
        {
            var spreadBuilder = new SpreadBuilderImp(syncMaxbetReposiory);
            var spread = new List<SpreadModel>
            {
                new SpreadModel
                {
                    BettypeId= (int)Bettype.Handicap,
                    Spread=(decimal)0.1
                },
                new SpreadModel
                {
                    BettypeId= (int)Bettype.OverUnder,
                    Spread=(decimal)0.2
                },
                 new SpreadModel
                {
                    BettypeId= (int)Bettype.FirstHalfHandicap,
                    Spread=(decimal)0.3
                },
                new SpreadModel
                {
                    BettypeId= (int)Bettype.FirstHalfOverUnder,
                    Spread=(decimal)0.4
                }
            };

            var actual = spreadBuilder.BuildSpreadSender(spread);
            var expected = new MaxbetSpread
            {
                Handicap = (decimal)0.1,
                OverUnder = (decimal)0.2,
                FirstHalfHandicap = (decimal)0.3,
                FirstHalfOverUnder = (decimal)0.4
            };
            DeepAssert.Equal(expected, actual);
        }
    }
}
