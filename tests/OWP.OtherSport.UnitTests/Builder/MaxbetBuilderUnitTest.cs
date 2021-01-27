namespace OWP.OtherSport.UnitTests.Builder
{
    using OWP.OtherSport.EventAssignment.Builder;
    using OWP.OtherSport.EventAssignment.Models;
    using OWP.OtherSport.Submit.Models;
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class MaxbetBuilderUnitTest
    {
        private readonly MaxbetBuilder maxbetBuilder;
        public MaxbetBuilderUnitTest()
        {
            maxbetBuilder = new MaxbetBuilderImp();
        }

        [Fact]
        public void BuildMaxbet_WhenMatchHasMaxbet_BuildMaxbetCorrect()
        {
            var traderMaxBetModels = new List<TraderMaxBetModel>
            {
                new TraderMaxBetModel
                {
                    Maxbet= new MatchMaxBet
                    {
                        BetTypeName= "1Q & 3Q (OU/OE)"
                    }
                }
            };

            var matchMaxBets = new List<MatchMaxBet>
            {
                new MatchMaxBet
                {
                    MaxBet= 10m,
                    BetTypeName= "1Q & 3Q (OU/OE)"
                }
            };

            maxbetBuilder.BuildMaxbet(traderMaxBetModels, matchMaxBets);

            var expected = new List<TraderMaxBetModel>
            {
                new TraderMaxBetModel
                {
                    Maxbet= new MatchMaxBet
                    {
                        MaxBet= 10m,
                        BetTypeName= "1Q & 3Q (OU/OE)"
                    }
                }
            };

            DeepAssert.Equal(expected, traderMaxBetModels);
        }
    }
}
