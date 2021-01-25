namespace OWP.OtherSport.UnitTests.Builder
{
    using OWP.OtherSport.EventAssignment.Builder;
    using OWP.OtherSport.EventAssignment.Models;
    using OWP.OtherSport.Submit.Contants;
    using OWP.OtherSport.Submit.Models;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class AssignmentBuilderUnitTest
    {
        private readonly AssignmentBuilder assignmentBuilder;

        public AssignmentBuilderUnitTest()
        {
            assignmentBuilder = new AssignmentBuilderImp();
        }

        [Fact]
        public void BuildAssignment_WhenExistsUserName_BuilAssignmentHasMaintraderCorrect()
        {
            var asssignment = new List<TraderMaxBetModel>
            {
                new TraderMaxBetModel()
                {
                    TraderAssignment = new TraderAssignment {
                        AssignmentNote = "check other #<user>",
                        BetTypeName = "1Q & 3Q (OU/OE)",
                        OSMatchId = 123,
                        MainTrader = string.Empty
                    }
                }
            };

            var matchAssignments = new List<MatchAssignment>
            {
                new MatchAssignment
                {
                    BetTypeName = "1Q & 3Q (OU/OE)",
                    Username= "sa",
                    AssignmentType = (int)AssignmentType.Assignment,
                    UserId=8,
                },
                 new MatchAssignment
                {
                    AssignmentNote="#<user> acb123",
                    BetTypeName = "1Q & 3Q (OU/OE)",
                    Username= "starix15",
                    AssignmentType = (int)AssignmentType.OtherAssignment,
                    UserId=128,
                }
            };
            assignmentBuilder.BuildAssignment(asssignment, matchAssignments);
            var expected = new TraderAssignment
            {
                AssignmentNote = "#starix15 acb123",
                BetTypeName = "1Q & 3Q (OU/OE)",
                MainTrader = "sa",
                OSMatchId = 123
            };
            DeepAssert.Equal(expected, asssignment[0].TraderAssignment);
        }

        [Fact]
        public void CreateAssignment_WhenExistsUserName_BuilAssignmentHasAssignmentNoteCorrect()
        {
            var asssignment = new TraderAssignment
            {
                AssignmentNote = "check other #<user>",
                BetTypeName = "1Q & 3Q (OU/OE)",
                OSMatchId = 123,
                MainTrader = string.Empty
            };

            var matchAssignments = new List<MatchAssignment>
            {
                new MatchAssignment
                {
                    BetTypeName = "1Q & 3Q (OU/OE)",
                    Username= "sa",
                    UserId=8,
                    AssignmentType = (int)AssignmentType.Assignment
                },
                 new MatchAssignment
                {
                    AssignmentNote="#<user> acb",
                    BetTypeName = "1Q & 3Q (OU/OE)",
                    Username= "starix15",
                    UserId=128,
                    AssignmentType = (int)AssignmentType.OtherAssignment
                }
            };
            assignmentBuilder.CreateAssignment(asssignment, matchAssignments);
            var expected = new TraderAssignment
            {
                AssignmentNote = "#starix15 acb",
                BetTypeName = "1Q & 3Q (OU/OE)",
                MainTrader = "sa",
                OSMatchId = 123
            };
            DeepAssert.Equal(expected, asssignment);
        }

        [Fact]
        public void DetectOtherAssignment_WhenAssignmentNote_Empty_Return_EmptyString()
        {
            var assignment = new MatchAssignment
            {
                AssignmentNote = ""
            };
            var assignmentNote = assignmentBuilder.DetectOtherAssignment(assignment);
            DeepAssert.Equal(string.Empty, assignmentNote);
        }

        [Fact]
        public void DetectOtherAssignment_WhenAssignmentNoteHasValue_ReplaceTraderCorrect()
        {
            var assignment = new MatchAssignment
            {
                AssignmentNote = "#starix15 acb",
                BetTypeName = "1Q & 3Q (OU/OE)",
                Username = "sa",
                OSMatchId = 123
            };
            var assignmentNote = assignmentBuilder.DetectOtherAssignment(assignment);
            DeepAssert.Equal("#starix15 acb", assignmentNote);
        }
    }
}
