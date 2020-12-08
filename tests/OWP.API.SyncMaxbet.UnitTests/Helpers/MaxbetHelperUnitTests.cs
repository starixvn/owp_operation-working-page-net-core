using OWP.API.SyncMaxbet.Models;
using OWP.Enums;
using OWP.Helpers;
using System.Collections.Generic;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.API.SyncMaxbet.UnitTests.Helpers
{
    public class MaxbetHelperUnitTests
    {
        #region trader assignment
        [Fact]
        public void FindTraderAssinmentByBettype_ListAssignment_BettypeId_Found_ReturnAssignmentHDP()
        {
            var assignments = new List<TraderAssignment>
            {
                new TraderAssignment
                {
                    BettypeId= (int)EnumBettype.Handicap,
                    UserId=111
                },
                 new TraderAssignment
                {
                    BettypeId= (int)EnumBettype.OverUnder,
                    UserId=222
                },
                  new TraderAssignment
                {
                    BettypeId= (int)EnumBettype.FirstHalfHandicap,
                    UserId=333
                },
                   new TraderAssignment
                {
                    BettypeId= (int)EnumBettype.FirstHalfOverUnder,
                    UserId=444
                }
            };
            var result = MaxbetHelper.FindTraderAssinmentByBettype(assignments, (int)EnumBettype.Handicap);
            var expected = new TraderAssignment
            {
                BettypeId = (int)EnumBettype.Handicap,
                UserId = 111
            };
            DeepAssert.Equal(expected, result);
        }

        [Fact]
        public void FindTraderAssinmentByBettype_ListAssignment_BettypeId_NotFound_ReturnNull()
        {
            var assignments = new List<TraderAssignment>
            {
                 new TraderAssignment
                {
                    BettypeId= (int)EnumBettype.OverUnder,
                    UserId=222
                },
                  new TraderAssignment
                {
                    BettypeId= (int)EnumBettype.FirstHalfHandicap,
                    UserId=333
                },
                   new TraderAssignment
                {
                    BettypeId= (int)EnumBettype.FirstHalfOverUnder,
                    UserId=444
                }
            };
            var result = MaxbetHelper.FindTraderAssinmentByBettype(assignments, (int)EnumBettype.Handicap);
            Assert.Null(result);
        }

        #endregion

        #region spread
        [Fact]
        public void FindSpreadByBettype_ListSpread_BettypeId_Found_ReturnSpreadHDP()
        {
            var spreads = new List<SpreadModel>
            {
                new SpreadModel
                {
                    BettypeId= (int)EnumBettype.Handicap,
                    Spread=(decimal)0.1
                },
                 new SpreadModel
                {
                    BettypeId= (int)EnumBettype.OverUnder,
                     Spread=(decimal)0.3
                },
                  new SpreadModel
                {
                    BettypeId= (int)EnumBettype.FirstHalfHandicap,
                     Spread=(decimal)0.3
                },
                   new SpreadModel
                {
                    BettypeId= (int)EnumBettype.FirstHalfOverUnder,
                    Spread=(decimal)0.4
                }
            };
            var result = MaxbetHelper.FindSpreadByBettype(spreads, (int)EnumBettype.Handicap);
            var expected = new SpreadModel
            {
                BettypeId = (int)EnumBettype.Handicap,
                Spread = (decimal)0.1
            };
            DeepAssert.Equal(expected, result);
        }

        [Fact]
        public void FindSpreadByBettype_ListSpread_BettypeId_Found_ReturnNull()
        {
            var spreads = new List<SpreadModel>
            {
                 new SpreadModel
                {
                    BettypeId= (int)EnumBettype.OverUnder,
                     Spread=(decimal)0.3
                },
                  new SpreadModel
                {
                    BettypeId= (int)EnumBettype.FirstHalfHandicap,
                     Spread=(decimal)0.3
                },
                   new SpreadModel
                {
                    BettypeId= (int)EnumBettype.FirstHalfOverUnder,
                    Spread=(decimal)0.4
                }
            };
            var result = MaxbetHelper.FindSpreadByBettype(spreads, (int)EnumBettype.Handicap);
            Assert.Null(result);
        }

        #endregion

        #region maxbet
        [Fact]
        public void FindMaxbetByBettype_ListMaxbet_BettypeId_Found_ReturnMaxbetHDP()
        {
            var maxbets = new List<Maxbet>
            {
                new Maxbet
                {
                    BettypeId= (int)EnumBettype.Handicap,
                    Value=(decimal)0.1
                },
                 new Maxbet
                {
                    BettypeId= (int)EnumBettype.OverUnder,
                     Value=(decimal)0.3
                },
                  new Maxbet
                {
                    BettypeId= (int)EnumBettype.FirstHalfHandicap,
                     Value=(decimal)0.3
                },
                   new Maxbet
                {
                    BettypeId= (int)EnumBettype.FirstHalfOverUnder,
                    Value=(decimal)0.4
                }
            };
            var result = MaxbetHelper.FindMaxbetByBettype(maxbets, (int)EnumBettype.Handicap);
            var expected = new Maxbet
            {
                BettypeId = (int)EnumBettype.Handicap,
                Value = (decimal)0.1
            };
            DeepAssert.Equal(expected, result);
        }

        [Fact]
        public void FindMaxbetByBettype_ListSpread_BettypeId_Found_ReturnNull()
        {
            var maxbets = new List<Maxbet>
            {
                 new Maxbet
                {
                    BettypeId= (int)EnumBettype.OverUnder,
                     Value=(decimal)111
                },
                  new Maxbet
                {
                    BettypeId= (int)EnumBettype.FirstHalfHandicap,
                      Value=(decimal)111
                },
                   new Maxbet
                {
                    BettypeId= (int)EnumBettype.FirstHalfOverUnder,
                     Value=(decimal)111
                }
            };
            var result = MaxbetHelper.FindMaxbetByBettype(maxbets, (int)EnumBettype.Handicap);
            Assert.Null(result);
        }

        #endregion
    }
}
