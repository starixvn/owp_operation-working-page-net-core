namespace OWP.OtherSport.UnitTests.Builder
{
    using OWP.OtherSport.EventAssignment.Builder;
    using OWP.OtherSport.EventAssignment.Models;
    using OWP.OtherSport.Submit.Models;
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class BettypeBuilderUnitTest
    {
        private readonly BettypeBuilder bettypeBuilder;
        public BettypeBuilderUnitTest()
        {
            bettypeBuilder = new BettypeBuilderImp();
        }

        [Fact]
        public void BuildBettype_WhenMatchHasAssignee_BuildBettypeCorrect()
        {
            var matchsAssignments = new List<EventAssignmentModel>
            {
                new EventAssignmentModel
                {
                    OSMatchId= 123,
                    League = "*SPAIN PRIMERA LALIGA",
                    MatchStatus = "Live",
                    DateTime = DateTime.Parse("2021-01-12"),
                    Home = "CD Universidad Catolica(V)_Home",
                    Away = "AEK Athens (V)_Away",
                    Remark = "TA ROBOT_Draw will PK",
                    IPTV= "ipTv aaaaa",
                    WB= "Streaming bbbbbbb",
                    Provider= "Provider JAck",
                    AutoLink="Yes",
                    Category="Category",
                    NoHDP="NoHDP",
                    MatchOffer="MatchOffer",
                    OtherProvider="OtherProvider",
                    Service="Medium",
                    Sport = "Football"
                }
            };

            var dicMatchAssignment = new Dictionary<int, List<MatchAssignment>>()
            {
                {123, new  List<MatchAssignment>{
                    new MatchAssignment
                    {
                        AssignmentNote = "#<user> acb123",
                        BetTypeName = "1Q & 3Q (OU/OE)",
                        Username = "starix15",
                        UserId = 123,
                    }
                } }
            };

            bettypeBuilder.BuildBettype(matchsAssignments, dicMatchAssignment);
            var expected = new List<EventAssignmentModel>
            {
                new EventAssignmentModel
                {
                     OSMatchId= 123,
                    League = "*SPAIN PRIMERA LALIGA",
                    MatchStatus = "Live",
                    DateTime = DateTime.Parse("2021-01-12"),
                    Home = "CD Universidad Catolica(V)_Home",
                    Away = "AEK Athens (V)_Away",
                    Remark = "TA ROBOT_Draw will PK",
                    IPTV= "ipTv aaaaa",
                    WB= "Streaming bbbbbbb",
                    Provider= "Provider JAck",
                    AutoLink="Yes",
                    Category="Category",
                    NoHDP="NoHDP",
                    MatchOffer="MatchOffer",
                    OtherProvider="OtherProvider",
                    Service="Medium",
                    Sport = "Football",
                    TraderMaxbetModels= new List<TraderMaxBetModel>
                    {
                        new TraderMaxBetModel
                        {
                            TraderAssignment = new TraderAssignment
                            {
                                OSMatchId= 123,
                                BetTypeName= "1Q & 3Q (OU/OE)"
                            },
                            Maxbet= new MatchSetting
                            {
                                OSMatchId= 123,
                                BetTypeName= "1Q & 3Q (OU/OE)"
                            }
                        }
                    }
                }
            };

            DeepAssert.Equal(expected, matchsAssignments);
        }
    }
}
