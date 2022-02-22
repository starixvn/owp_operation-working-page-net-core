namespace OWP.OtherSport.UnitTests.Submit.Services
{
    using OWP.OtherSport.Submit.Contants;
    using OWP.OtherSport.Submit.Models;
    using OWP.OtherSport.Submit.Services;
    using OWP.OtherSport.Validation.Models;

    using Xunit;
    using Xunit.Asserts.Compare;

    public class AssignmentAnalyzerUnitTests
    {
        private readonly AssignmentAnalyzerImp services;

        public AssignmentAnalyzerUnitTests()
        {
            services = new AssignmentAnalyzerImp();
        }

        [Fact]
        public void ReassignOSMatchId_Map_MapSuccessful()
        {
            var hashKeys = new List<MatchByHashKey>
            {
                new MatchByHashKey
                {
                    OSMatchId = 1,
                    HashKey="abc123"
                },
                new MatchByHashKey
                {
                    OSMatchId = 2,
                    HashKey = "xyz"
                }
            };
            var assignments = new List<AssignmentModel>
            {
               new AssignmentModel { HashKey = "abc123", Home = "ABC" },
               new AssignmentModel { HashKey = "xyz", Home = "XYZ" }
            };
            services.ReassignOSMatchId(hashKeys, assignments);

            Assert.Equal(1, assignments[0].OSMatchId);
            Assert.Equal(2, assignments[1].OSMatchId);
        }

        [Fact]
        public void DetectOldMatchIds()
        {
            var oldMatchIds = new List<int> { 1, 2, 3, 4 };
            var newMatchIds = new List<int> { 1, 4, 5, 6, 7 };
            var expected = "2,3";

            var actual = services.DetectOldMatchIds(oldMatchIds, newMatchIds);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetMaxBets()
        {
            var assignment = new AssignmentModel
            {
                OSMatchId = 123
            };
            var assignmentMaxbet = new List<AssignmentMaxbet>
            {
                new AssignmentMaxbet { MaxBet = 1200, Spread= 10, Bettype = Bettype.First5FramesHDP },
                new AssignmentMaxbet { MaxBet = 30000, Spread= 10,Bettype = Bettype.FirstHOU },
                new AssignmentMaxbet { MaxBet = null, Spread= null, Bettype = Bettype.AllProductLS },
                new AssignmentMaxbet { Bettype = Bettype.SetXWinnerHDP },
            };
            var expected = new List<MatchSetting>
            {
                new MatchSetting{ OSMatchId = 123, BetTypeName = Bettype.First5FramesHDP,MaxBet = 1200, Spread = 10 },
                new MatchSetting{ OSMatchId = 123, BetTypeName = Bettype.FirstHOU,MaxBet = 30000, Spread = 10}
            };

            var actual = services.GetMaxBets(assignment, assignmentMaxbet).ToList();

            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void GetAllAssignmentsOfMatch()
        {
            var assignment = new AssignmentModel
            {
                OSMatchId = 123
            };
            var assignmentMaxbet = new List<AssignmentMaxbet>
            {
                new AssignmentMaxbet { Bettype = Bettype.First5FramesHDP, Trader="100,#<user>", OtherTrader="122,#<user> my note" },
                new AssignmentMaxbet { Bettype = Bettype.FirstHOU, Trader="200,(01) #<user>", OtherTrader="0,my note"},
                new AssignmentMaxbet { Bettype = Bettype.FirstHOU, Trader="", OtherTrader=""},
                new AssignmentMaxbet { Bettype = Bettype.SetXWinnerHDP, Trader="0,note", OtherTrader=""},
            };
            var expected = new List<MatchAssignment>
            {
                new MatchAssignment{ OSMatchId = 123, AssignmentType=1, BetTypeName = Bettype.First5FramesHDP, UserId = 100, AssignmentNote="#<user>"},
                new MatchAssignment{ OSMatchId = 123, AssignmentType=2, BetTypeName = Bettype.First5FramesHDP, AssignmentNote="#<user> my note", UserId=122},
                new MatchAssignment{ OSMatchId = 123, AssignmentType=1, BetTypeName = Bettype.FirstHOU, UserId= 200, AssignmentNote="(01) #<user>"},
                new MatchAssignment{ OSMatchId = 123, AssignmentType=2, BetTypeName = Bettype.FirstHOU, AssignmentNote="my note", UserId=0},
                new MatchAssignment{ OSMatchId = 123, AssignmentType=1, BetTypeName = Bettype.SetXWinnerHDP, UserId=0, AssignmentNote="note"}
            };

            var actual = services.GetAllAssignmentsOfMatch(assignment, assignmentMaxbet).ToList();

            DeepAssert.Equal(expected, actual);
        }
    }
}