using OWP.PreMatchNotification.Constants;
using OWP.PreMatchNotification.LinkedMatch.Models;
using OWP.PreMatchNotification.LinkedMatch.Services;
using OWP.PreMatchNotification.Models;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.PreMatchNotification.UnitTests.LinkedMatch.Services
{
    public class ChangedLiveMatchGeneratorUnitTests
    {
        public readonly ChangedLiveMatchGenerator service;

        public ChangedLiveMatchGeneratorUnitTests()
        {
            service = new ChangedLiveMatchGeneratorImp();
        }

        [Fact]
        public void Create_Always_ReturnChangedValue()
        {
            var changedSnapshot = new ChangedSnapshot
            {
                DeletedMatches = new List<GeneralMatch>()
                {
                    new(){MatchId=111, InfoSiteMatchId=111,EventDateTime=new DateTime(2021,6,18,14,12,20),
                        LeagueName="LeagueA",Home="HomeA",Away="AwayA"},
                    new(){MatchId=222, InfoSiteMatchId=222,EventDateTime=new DateTime(2021,6,18,02,10,20),
                        LeagueName="LeagueB",Home="HomeB",Away="AwayB"}
                },
                InsertedMatches = new List<GeneralMatch>()
                {
                    new(){MatchId=300, InfoSiteMatchId=333,EventDateTime=new DateTime(2021,6,18,5,12,20),
                        LeagueName="LeagueC",Home="HomeC",Away="AwayC"}
                }
            };
            var employees = new Employee
            {
                PrematchInfosites = new Dictionary<int, InfositePrematchLeague>
                {
                    {111, new(){ PreMatchLeagueId=100, InfoSiteMatchId=111, PrematchAssigneeIds=new List<int>{8,9} } },
                    {222, new(){ PreMatchLeagueId=200, InfoSiteMatchId=222, PrematchAssigneeIds=new List<int>{} } },
                    {333, new(){ PreMatchLeagueId=300, InfoSiteMatchId=222, PrematchAssigneeIds=new List<int>{10,11} } },
                },
                AllLSTEmployees = new List<int> { 1, 2, 3 },
                AllPrematchEmployees = new List<int> { 4, 5, 6 }
            };

            var expected = new List<ChangedValue>
            {
                new(){
                    MessageTypeId=(int)MsgTemplateCode.LGACancelLiveMatch,
                    League="LeagueA",
                    Home="HomeA",
                    Away="AwayA",
                    KoTime = "02:12 PM",
                    ReceivedUserIds=new List<int>{1,2,3,8,9}
                },
                new(){
                    MessageTypeId=(int)MsgTemplateCode.LGACancelLiveMatch,
                    League="LeagueB",
                    Home="HomeB",
                    Away="AwayB",
                    KoTime = "02:10 AM",
                    ReceivedUserIds=new List<int>{1,2,3,4,5,6}
                },
                new(){
                    MessageTypeId=(int)MsgTemplateCode.LGAAddLiveMatch,
                    League="LeagueC",
                    Home="HomeC",
                    Away="AwayC",
                    KoTime = "05:12 AM",
                    ReceivedUserIds=new List<int>{1,2,3,10,11}
                }
            };

            var actual = service.Create(changedSnapshot, employees);
            DeepAssert.Equal(expected[0], actual[0]);
            DeepAssert.Equal(expected[1], actual[1]);
            DeepAssert.Equal(expected[2], actual[2]);
        }
    }
}