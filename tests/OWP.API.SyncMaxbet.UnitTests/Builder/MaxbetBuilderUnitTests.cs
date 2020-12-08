using NSubstitute;
using OWP.API.SyncMaxbet.Builder;
using OWP.API.SyncMaxbet.Models;
using OWP.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.API.SyncMaxbet.UnitTests.Builder
{
    public class MaxbetBuilderUnitTests
    {
        private readonly SyncMaxbetReposiory syncMaxbetReposiory;

        private readonly MaxbetBuilderImp maxbetBuilder;


        public MaxbetBuilderUnitTests()
        {
            syncMaxbetReposiory = Substitute.For<SyncMaxbetReposiory>();
            maxbetBuilder = new MaxbetBuilderImp(syncMaxbetReposiory);
        }

        [Fact]
        public async Task BuildMaxbet_OwpmatchId_NotFoundMaxbet_ReturnDefaultmaxbetInfo()
        {
            var matchId = 123;
            IEnumerable<Maxbet> maxbet = new List<Maxbet>();
            syncMaxbetReposiory
              .GetMaxbetByOwpMatchId(matchId)
              .ReturnsForAnyArgs(Task.FromResult(maxbet));

            var result = await maxbetBuilder.BuildMaxbet(matchId);
            DeepAssert.Equal(result, new MaxbetSpread());
        }

        [Fact]
        public async Task BuildMaxbet_Found_HDP_OU_1HHDP_1HOU_ReturnMaxbetSpreadCorrect()
        {
            IEnumerable<Maxbet> maxbet = new List<Maxbet>
            {
                new Maxbet
                {
                    BettypeId= (int)EnumBettype.Handicap,
                    Value=1000
                },
                new Maxbet
                {
                    BettypeId= (int)EnumBettype.OverUnder,
                    Value=1000
                },
                 new Maxbet
                {
                    BettypeId= (int)EnumBettype.FirstHalfHandicap,
                    Value=1000
                },
                new Maxbet
                {
                    BettypeId= (int)EnumBettype.FirstHalfOverUnder,
                    Value=1000
                }
            };

            syncMaxbetReposiory
                .GetMaxbetByOwpMatchId(123)
                .ReturnsForAnyArgs(Task.FromResult(maxbet));

            var result = await maxbetBuilder.BuildMaxbet(123);

            var expected = new MaxbetSpread
            {
                Handicap = 1000,
                OverUnder = 1000,
                FirstHalfHandicap = 1000,
                FirstHalfOverUnder = 1000
            };

            DeepAssert.Equal(expected, result);
        }

        [Fact]
        public void BuildMaxbetSender_ListMaxbetEmpty_ReturnMaxbetSenderDefault()
        {
            var maxbet = new List<Maxbet>();
            var result = MaxbetBuilderImp.BuildMaxbetSender(maxbet);
            DeepAssert.Equal(new MaxbetSpread(), result);
        }

        [Fact]
        public void BuildMaxbetSender_ListMaxbet_HasFull_HDP_OU_1HHDP_1HOU_ReturnMaxbetSenderFullBettype()
        {
            var maxbet = new List<Maxbet>
            {
                new Maxbet
                {
                    BettypeId= (int)EnumBettype.Handicap,
                    Value=10000
                },
                new Maxbet
                {
                    BettypeId= (int)EnumBettype.OverUnder,
                    Value=10000
                },
                new Maxbet
                {
                    BettypeId= (int)EnumBettype.FirstHalfHandicap,
                    Value=10000
                },
                new Maxbet
                {
                    BettypeId= (int)EnumBettype.FirstHalfOverUnder,
                    Value=10000
                }
            };

            var result = MaxbetBuilderImp.BuildMaxbetSender(maxbet);
            var expected = new MaxbetSpread
            {
                Handicap = 10000,
                OverUnder = 10000,
                FirstHalfOverUnder = 10000,
                FirstHalfHandicap = 10000
            };
            DeepAssert.Equal(expected, result);
        }

        [Fact]
        public void BuildMaxbetFirstHalfHandicap_Found_HDP_InMaxbetMapper_AddMaxbet1HHDPToListSuccess()
        {
            var maxbet = new List<Maxbet>
            {
                new Maxbet
                {
                    BettypeId= (int)EnumBettype.Handicap,
                    Value=400000
                }
            };

            maxbetBuilder.BuildMaxbetFirstHalfHandicap(maxbet);

            var expected = new List<Maxbet>
            {
                new Maxbet
                {
                    BettypeId= (int)EnumBettype.Handicap,
                    Value=400000
                },
                new Maxbet
                {
                    BettypeId= (int)EnumBettype.FirstHalfHandicap,
                    Value=80000
                }
            };
            DeepAssert.Equal(expected, maxbet);
        }
    }
}
