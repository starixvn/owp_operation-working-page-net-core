using NSubstitute;
using OWP.Maxbet.Builder;
using OWP.Maxbet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.Maxbet.UnitTests.Builder
{
    public class SpecialProductMaxbetBuilderUnitTests
    {
        private readonly SpecialProductMaxbetBuilder specialProductMaxbetBuilder;

        private readonly MaxbetReposiory syncMaxbetReposiory;

        private readonly MaxbetBuilder maxbetBuilder;

        public SpecialProductMaxbetBuilderUnitTests()
        {
            syncMaxbetReposiory = Substitute.For<MaxbetReposiory>();
            maxbetBuilder = Substitute.For<MaxbetBuilder>();
            specialProductMaxbetBuilder = new SpecialProductMaxbetBuilderImp(syncMaxbetReposiory, maxbetBuilder);
        }

        [Fact]
        public async void BuildMaxbet_Not_Found_Maxbet_Return_ModelDefault()
        {
            syncMaxbetReposiory
                .GetMaxbetByOwpMatchId(123)
                    .ReturnsForAnyArgs(new List<MaxbetModel>());
            var actual = await specialProductMaxbetBuilder.BuildMaxbet(123);
            DeepAssert.Equal(new(), actual);
        }

        [Fact]
        public async void BuildMaxbet_Found_Maxbet_Return_ModelMaxbetSpread()
        {
            syncMaxbetReposiory
                .GetMaxbetByOwpMatchId(123)
                    .ReturnsForAnyArgs(new List<MaxbetModel>()
                    {
                        new ()
                        {
                            BettypeId=1,
                            Value=10
                        },
                         new ()
                        {
                            BettypeId=3,
                            Value=10
                        },
                          new ()
                        {
                            BettypeId=7,
                            Value=10
                        },
                           new ()
                        {
                            BettypeId=8,
                            Value=10
                        }
                    });
            var actual = await specialProductMaxbetBuilder.BuildMaxbet(123);
            DeepAssert.Equal(new()
            {
                Handicap = 10,
                OverUnder = 10,
                FirstHalfHandicap = 10,
                FirstHalfOverUnder = 10
            }, actual);
        }

        [Fact]
        public async void BuildMaxbetXMins_Not_Found_Maxbet_Return_ModelDefaultMaxbetSpread()
        {
            syncMaxbetReposiory
                .GetMaxbetXMins(123)
                    .ReturnsForAnyArgs(new List<decimal?>());
            var actual = await specialProductMaxbetBuilder
                .BuildMaxbetXMins(123);
            DeepAssert.Equal(new(), actual);
        }

        [Fact]
        public async void BuildMaxbetXMins_Found_Maxbet_Return_ModelMaxbetSpread()
        {
            syncMaxbetReposiory
                .GetMaxbetXMins(123)
                    .ReturnsForAnyArgs(new List<decimal?>() { 123 });
            var actual = await specialProductMaxbetBuilder
                .BuildMaxbetXMins(123);
            DeepAssert.Equal(new()
            {
                Handicap = 123,
                OverUnder = 123
            }, actual);
        }
    }
}