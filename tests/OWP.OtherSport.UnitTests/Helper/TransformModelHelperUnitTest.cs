using System;
using System.Collections.Generic;
using OWP.OtherSport.Submit.Models;
using Xunit.Asserts.Compare;
using OWP.OtherSport.EventAssignment.Models;
using Xunit;

namespace OWP.OtherSport.UnitTests.Helper
{
    public class TransformModelHelperUnitTest
    {
        [Fact]
        public static void TransformModelHelper_Transform_MatchInfo_To_EventAssignmentModel_Successfully()
        {
            var matchsAssignments = new List<MatchInfo>
            {
                new MatchInfo
                {
                     OSMatchId = 123,
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
                },
                new MatchInfo
                {
                    OSMatchId = 124,
                    League = "NBA",
                    MatchStatus = "Live",
                    DateTime = DateTime.Parse("2021-01-12"),
                    Home = "Golden State Warrior",
                    Away = "Brooklyn NET",
                    Remark = "TA ROBOT_Draw will PK",
                    IPTV= "ipTv aaaaa",
                    WB= "Streaming bbbbbbb",
                    Provider= "Provider",
                    AutoLink="Yes",
                    Category="Category",
                    NoHDP="NoHDP",
                    MatchOffer="MatchOffer",
                    OtherProvider="OtherProvider",
                    Service="Medium",
                    Sport = "Basketball"
                }
            };
            var actual = TransformModelHelper.TransformToAssignmentModelIgnoreTraderMaxbet(matchsAssignments);
            var expected= new List<EventAssignmentModel>
            {
                new EventAssignmentModel
                {
                     OSMatchId = 123,
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
                },
                new EventAssignmentModel
                {
                    OSMatchId = 124,
                    League = "NBA",
                    MatchStatus = "Live",
                    DateTime = DateTime.Parse("2021-01-12"),
                    Home = "Golden State Warrior",
                    Away = "Brooklyn NET",
                    Remark = "TA ROBOT_Draw will PK",
                    IPTV= "ipTv aaaaa",
                    WB= "Streaming bbbbbbb",
                    Provider= "Provider",
                    AutoLink="Yes",
                    Category="Category",
                    NoHDP="NoHDP",
                    MatchOffer="MatchOffer",
                    OtherProvider="OtherProvider",
                    Service="Medium",
                    Sport = "Basketball"
                }
            };

            DeepAssert.Equal(expected, actual);
        }
    }
}
