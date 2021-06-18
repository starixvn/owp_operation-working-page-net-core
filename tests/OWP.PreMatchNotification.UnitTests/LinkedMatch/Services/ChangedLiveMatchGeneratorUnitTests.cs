using OWP.PreMatchNotification.LinkedMatch.Models;
using OWP.PreMatchNotification.LinkedMatch.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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
        public void Create()
        {
            var changedSnapshot = new ChangedSnapshot
            {
                DeletedMatches = new List<GeneralMatch>()
                {
                    new(){MatchId=111, InfoSiteMatchId=111,EventDateTime=new DateTime(2021,6,18,14,12,20)},
                    new(){MatchId=222, InfoSiteMatchId=222,EventDateTime=new DateTime(2021,6,18,02,10,20)}
                },
                InsertedMatches = new List<GeneralMatch>()
                {
                    new(){MatchId=111, InfoSiteMatchId=222,EventDateTime=new DateTime(2021,6,18,5,12,20)}
                }
            };
            var employees = new Employee
            {
                PrematchInfosites = new Dictionary<int, InfositePrematchLeague>
                {
                    {111, new(){ PreMatchLeagueId=100, InfoSiteMatchId=111, PrematchAssigneeIds=new List<int>{8,9} } },
                    {222, new(){ PreMatchLeagueId=200, InfoSiteMatchId=222, PrematchAssigneeIds=new List<int>{} } },
                },
                AllLSTEmployees = new List<int> { 1, 2, 3 },
                AllPrematchEmployees = new List<int> { 4, 5, 6 }
            };
            

            var actual = service.Create(changedSnapshot, employees);
        }
    }
}
