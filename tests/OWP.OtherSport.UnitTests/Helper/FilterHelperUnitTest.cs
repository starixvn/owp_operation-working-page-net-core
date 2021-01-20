using System;
using System.Collections.Generic;
using OWP.OtherSport.EventAssignment.Helper;
using OWP.OtherSport.Submit.Models;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.OtherSport.UnitTests.Helper
{
    public static class FilterHelperUnitTest
    {
        [Fact]
        public static void FilterMatchByEmployee_WhenEmployeeEmpty_ReturnMatchsAssignmentsOriginal()
        {
            var employee = string.Empty;
            var dicMatchAssignment = new Dictionary<int, List<MatchAssignment>>();
            var matchsAssignments = new List<MatchInfo>
            {
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
            var actual = FilterHelper.FilterMatchByEmployee(employee, matchsAssignments, dicMatchAssignment);
            var expected = new List<MatchInfo>
            {
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

            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public static void FilterMatchByEmployee_WhenEmployeeHasvalue_MatchUsername_ReturnMatchsAssignmentsCorrect()
        {
            var employee = "s";
            var dicMatchAssignment = new Dictionary<int, List<MatchAssignment>>
        {
            {124, new List<MatchAssignment>
            {
                new MatchAssignment
                {
                    OSMatchId= 124,
                    Username= "sa"
                }
            } }
        };
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
            var actual = FilterHelper.FilterMatchByEmployee(employee, matchsAssignments, dicMatchAssignment);
            var expected = new List<MatchInfo>
            {
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

            DeepAssert.Equal(expected, actual);
        }
    }
}
