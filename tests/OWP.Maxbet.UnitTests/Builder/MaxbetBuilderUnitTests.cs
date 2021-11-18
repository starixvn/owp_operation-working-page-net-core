using NSubstitute;
using OWP.Maxbet.Builder;
using OWP.Maxbet.Models;
using OWP.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.Maxbet.UnitTests.Builder
{
    public class MaxbetBuilderUnitTests
    {
        private readonly MaxbetReposiory syncMaxbetReposiory;

        private readonly MaxbetBuilderImp maxbetBuilder;

        private readonly Dictionary<int, int> maxbetMapper = new Dictionary<int, int>
            {
                {500_000,120_000 },
                {400_000,120_000 },
                {350_000,100_000 },
                {300_000,100_000 },
                {200_000,100_000 },
                {180_000,50_000 },
                {150_000,40_000 },
                {130_000,40_000 },
                {120_000,40_000 },
                {110_000,40_000 },
                {100_000,40_000 },
                {90_000,40_000 },
                {80_000,30_000 },
                {70_000,30_000 },
                {60_000,25_000 },
                {50_000,25_000 },
                {40_000,20_000 },
                {35_000,18_000 },
                {30_000,15_000 },
                {25_000,12_000 },
                {20_000,10_000 },
                {18_000,10_000 },
                {15_000,8_000 },
                {12_000,6_000 },
                {10_000,5_000 },
                {8_000,4_000 },
                {7_000,4_000 },
                {6_000,3_000 },
                {5_000,3_000 },
                {4_000,2_000 },
                {3_000,2_000 },
                {2_000,2_000 }
            };

        public MaxbetBuilderUnitTests()
        {
            syncMaxbetReposiory = Substitute.For<MaxbetReposiory>();
            maxbetBuilder = new MaxbetBuilderImp(syncMaxbetReposiory);
        }

        [Fact]
        public async Task BuildMaxbet_OwpmatchId_NotFoundMaxbet_ReturnDefaultmaxbetInfo()
        {
            var matchId = 123;
            IEnumerable<MaxbetModel> maxbet = new List<MaxbetModel>();
            syncMaxbetReposiory
              .GetMaxbetByOwpMatchId(matchId)
              .ReturnsForAnyArgs(Task.FromResult(maxbet));

            var result = await maxbetBuilder.BuildMaxbet(matchId);
            DeepAssert.Equal(result, new MaxbetSpread());
        }

        [Fact]
        public async Task BuildMaxbet_Found_HDP_OU_1HHDP_1HOU_ReturnMaxbetSpreadCorrect()
        {
            IEnumerable<MaxbetModel> maxbet = new List<MaxbetModel>
            {
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.Handicap,
                    Value=1000
                },
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.OverUnder,
                    Value=1000
                },
                 new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.FirstHalfHandicap,
                    Value=1000
                },
                new MaxbetModel
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
            var maxbet = new List<MaxbetModel>();
            var result = MaxbetBuilderImp.BuildMaxbetSender(maxbet);
            DeepAssert.Equal(new MaxbetSpread(), result);
        }

        [Fact]
        public void BuildMaxbetSender_ListMaxbet_HasFull_HDP_OU_1HHDP_1HOU_ReturnMaxbetSenderFullBettype()
        {
            var maxbet = new List<MaxbetModel>
            {
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.Handicap,
                    Value=10000
                },
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.OverUnder,
                    Value=10000
                },
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.FirstHalfHandicap,
                    Value=10000
                },
                new MaxbetModel
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
        public async Task BuildMaxbet_FirstHalfHandicap_NotFound_ReturnMaxbetSpreadIncludeFirstHaflHandicap()
        {
            var matchId = 123;
            IEnumerable<MaxbetModel> maxbet = new List<MaxbetModel>
            {
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.Handicap,
                    Value=400000
                }
            };
            syncMaxbetReposiory
              .GetMaxbetByOwpMatchId(matchId)
              .ReturnsForAnyArgs(Task.FromResult(maxbet));

            var result = await maxbetBuilder.BuildMaxbet(matchId);
            var expected = new MaxbetSpread
            {
                Handicap = 400000,
                FirstHalfHandicap = 120000,
                FirstHalfOverUnder = 120000
            };
            DeepAssert.Equal(expected, result);
        }

        [Fact]
        public async Task BuildMaxbet_FirstHalfOverUnder_NotFound_ReturnMaxbetSpreadIncludeFirstHaflOverUnder()
        {
            var matchId = 123;
            IEnumerable<MaxbetModel> maxbet = new List<MaxbetModel>
            {
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.OverUnder,
                    Value=400000
                },
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.Handicap,
                    Value=400000
                }
            };
            syncMaxbetReposiory
              .GetMaxbetByOwpMatchId(matchId)
              .ReturnsForAnyArgs(Task.FromResult(maxbet));

            var result = await maxbetBuilder.BuildMaxbet(matchId);
            var expected = new MaxbetSpread
            {
                Handicap = 400000,
                OverUnder = 400000,
                FirstHalfHandicap = 120000,
                FirstHalfOverUnder = 120000
            };
            DeepAssert.Equal(expected, result);
        }

        [Fact]
        public void BuildMaxbetFirstHalfHandicap_Found_HDP_InMaxbetMapper_AddMaxbet1HHDPToListSuccess()
        {
            var maxbet = new List<MaxbetModel>
            {
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.Handicap,
                    Value=400000
                }
            };

            maxbetBuilder.BuildMaxbetFirstHalfHandicap(maxbet, maxbetMapper);

            var expected = new List<MaxbetModel>
            {
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.Handicap,
                    Value=400000
                },
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.FirstHalfHandicap,
                    Value=120000
                }
            };
            DeepAssert.Equal(expected, maxbet);
        }

        [Fact]
        public void BuildMaxbetFirstHalfHandicap_NotFound_HDP_InMaxbetMapper_AddMaxbet1HHDPToListSuccess()
        {
            var maxbet = new List<MaxbetModel>
            {
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.OverUnder,
                    Value=400
                }
            };

            maxbetBuilder.BuildMaxbetFirstHalfHandicap(maxbet, maxbetMapper);

            var expected = new List<MaxbetModel>
            {
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.OverUnder,
                    Value=400
                },
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.FirstHalfHandicap,
                }
            };
            DeepAssert.Equal(expected, maxbet);
        }

        [Fact]
        public void BuildMaxbetFirstHalfOverUnder_Found_HDP_InMaxbetMapper_AddMaxbet1HOUToListSuccess()
        {
            var maxbet = new List<MaxbetModel>
            {
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.Handicap,
                    Value=400000
                }
            };

            maxbetBuilder.BuildMaxbetFirstHalfOverUnder(maxbet, maxbetMapper);

            var expected = new List<MaxbetModel>
            {
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.Handicap,
                    Value=400000
                },
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.FirstHalfOverUnder,
                    Value=120000
                }
            };
            DeepAssert.Equal(expected, maxbet);
        }

        [Fact]
        public void BuildMaxbetFirstHalfOverUnder_NotFound_HDP_InMaxbetMapper_AddMaxbet1HOUToListSuccess()
        {
            var maxbet = new List<MaxbetModel>
            {
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.OverUnder,
                    Value=400
                }
            };

            maxbetBuilder.BuildMaxbetFirstHalfOverUnder(maxbet, maxbetMapper);

            var expected = new List<MaxbetModel>
            {
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.OverUnder,
                    Value=400
                },
                new MaxbetModel
                {
                    BettypeId= (int)EnumBettype.FirstHalfOverUnder,
                }
            };
            DeepAssert.Equal(expected, maxbet);
        }
    }
}