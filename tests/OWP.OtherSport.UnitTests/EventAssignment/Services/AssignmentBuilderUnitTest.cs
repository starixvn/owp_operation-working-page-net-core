namespace OWP.OtherSport.UnitTests.EventAssignment.Services
{
    using OWP.OtherSport.EventAssignment.Models;
    using OWP.OtherSport.EventAssignment.Services;
    using OWP.OtherSport.Submit.Models;

    using Xunit;
    using Xunit.Asserts.Compare;

    public class AssignmentBuilderUnitTest
    {
        private readonly AssignmentBuilderImp assignmentBuilder;

        public AssignmentBuilderUnitTest()
        {
            assignmentBuilder = new AssignmentBuilderImp();
        }

        [Fact]
        public void Build_MatchAssignment_MapMatchAssignment()
        {
            var eventAssignmentModels = new List<EventAssignmentModel>
            {
                new()
                {
                    OSMatchId = 111,
                    Sport = "Basketball",
                },
                new()
                {
                    OSMatchId = 222,
                    Sport = "Snooker",
                }
            };

            var matchAssignment = new Dictionary<int, List<MatchAssignment>>
            {
                {   111, new List<MatchAssignment>
                    {
                        new(){  BetTypeName = "FT (HDP)", Username= "starix12", UserId=8,AssignmentType = 1,  AssignmentNote = "#<user>"},
                        new(){  BetTypeName = "FT (OU/OE)", Username= "starix14", UserId=17,AssignmentType = 1,  AssignmentNote = "#<user>"},
                        new(){  BetTypeName = "FT (OE)", Username= "starix15", UserId=18,AssignmentType = 1,  AssignmentNote = "#<user>"}
                    }
                },
                {   222, new List<MatchAssignment>
                    {
                        new(){  BetTypeName = "Frames Winner (HDP/OU/OE)",Username= "starix12", UserId=8,AssignmentType = 1,  AssignmentNote = "#<user>"},
                        new(){  BetTypeName = "First 5 Frames (HDP)",Username= "starix14", UserId=17,AssignmentType = 1,  AssignmentNote = "#<user>"},
                    }
                }
            };

            var expected = new List<EventAssignmentModel>
            {
                new()
                {
                    OSMatchId = 111,
                    Sport = "Basketball",
                    TraderMaxbetModels = new List<TraderMaxBetModel>
                    {
                        new(){ BetTypeName = "FT (HDP)", TraderAssignment = new TraderAssignment{MainTrader="#starix12",AssignmentNote=""} },
                        new(){ BetTypeName = "FT (OU/OE)", TraderAssignment = new TraderAssignment{MainTrader="#starix14",AssignmentNote=""} },
                        new(){ BetTypeName = "FT (OE)", TraderAssignment = new TraderAssignment{MainTrader="#starix15",AssignmentNote=""} },
                    }
                },
                new()
                {
                    OSMatchId = 222,
                    Sport = "Snooker",
                    TraderMaxbetModels = new List<TraderMaxBetModel>
                    {
                        new(){ BetTypeName = "Frames Winner (HDP/OU/OE)", TraderAssignment = new TraderAssignment{MainTrader="#starix12",AssignmentNote=""} },
                        new(){ BetTypeName = "First 5 Frames (HDP)", TraderAssignment = new TraderAssignment{MainTrader="#starix14",AssignmentNote=""} },
                    }
                }
            };

            assignmentBuilder.Build(eventAssignmentModels, matchAssignment);

            DeepAssert.Equal(expected, eventAssignmentModels);
        }

        [Fact]
        public void CreateAssignment_WhenExistsUserName_BuilAssignmentHasTraderMaxbetModels()
        {
            var matchAssignments = new List<MatchAssignment>
            {
                new MatchAssignment
                {
                    Username= "starix12",
                    BetTypeName = "1Q & 3Q (OU/OE)",
                    UserId=8,
                    AssignmentType = 1,
                    AssignmentNote = "#<user>"
                },
                new MatchAssignment
                {
                    AssignmentNote="#<user> helloword",
                    BetTypeName = "1Q & 3Q (OU/OE)",
                    Username= "starix15",
                    AssignmentType = 2,
                    UserId=128
                }
            };

            var eventAssignmentModel = new EventAssignmentModel
            {
                OSMatchId = 111,
                Sport = "Basketball",
            };

            var expected = new EventAssignmentModel
            {
                OSMatchId = 111,
                Sport = "Basketball",
                TraderMaxbetModels = new List<TraderMaxBetModel>
                    {
                        new(){ BetTypeName = "1Q & 3Q (OU/OE)", TraderAssignment = new TraderAssignment{MainTrader="#starix12",AssignmentNote="#starix15 helloword"} }
                    }
            };

            assignmentBuilder.CreateAssignments(matchAssignments, eventAssignmentModel);

            DeepAssert.Equal(expected, eventAssignmentModel);
        }

        [Fact]
        public void DetectAssignment_WhenAssignmentNote_Empty_Return_EmptyString()
        {
            var assignment = new MatchAssignment
            {
                AssignmentNote = null
            };

            var assignmentNote = assignmentBuilder.DetectAssignment(assignment);

            Assert.Equal(string.Empty, assignmentNote);
        }

        [Fact]
        public void DetectAssignment_WhenAssignmentNoteHasValue_ReplaceTraderCorrect()
        {
            var assignment = new MatchAssignment
            {
                AssignmentNote = "#starix15 acb",
            };

            var assignmentNote = assignmentBuilder.DetectAssignment(assignment);

            Assert.Equal("#starix15 acb", assignmentNote);
        }
    }
}