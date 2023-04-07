using NSubstitute;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services.RealMatch;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.AssigneeSuggestionList.UnitTests.Services
{
    public class CornerServiceUnitTests
    {
        private List<AssignmentModel> assignments = new List<AssignmentModel>();

        private List<AssignmentModel> assignmentsCorner = new List<AssignmentModel>();

        private readonly SingleGameGroupService singleGameGroupService;

        public CornerServiceUnitTests()
        {
            this.singleGameGroupService = Substitute.For<SingleGameGroupService>(); ;
        }

        private void Initialize()
        {
            assignments = new List<AssignmentModel>()
            {
                new AssignmentModel(){
                    OWPMatchId=1,
                    KOTime=DateTime.Parse("2023-3-31 18:00:00").TimeOfDay,
                    EventDateTime=DateTime.Parse("2023-3-31 18:00:00"),
                    BettypeId= 1,
                    UserId=123,
                    Remark="ETPK"
                }
            };
            assignmentsCorner = new List<AssignmentModel>()
            {
                new AssignmentModel(){
                    OWPMatchId=2,
                    KOTime=DateTime.Parse("2023-3-31 18:05:00").TimeOfDay,
                    EventDateTime=DateTime.Parse("2023-3-31 18:05:00"),
                    IsCornerMatch=true,
                    BettypeId= 1,
                    UserId=123,
                    ParentMatchId=1
                }
            };
        }

        [Fact]
        public void BuildScheduleUpdateModel_ReturnScheduleUpdateModel()
        {
            // Arrange
            Initialize();

            var result = CornerServiceImp.BuildScheduleUpdateModel(assignmentsCorner, assignments, DateTime.Parse("2023-3-31 18:05:00"));

            var expect = new ScheduleUpdateModel
            {
                FilterDate = DateTime.Parse("2023-3-31 18:05:00"),
                EtPk = new ETPKUpdateSchedule
                {
                    IsHasEtPk = true,
                    EtPkValue = "ET"
                },
                AssignmentCell = new AssignmentCell
                {
                    Color = ColorCodeConst.CORNER,
                    TextColor = ColorCodeConst.TEXT_CORNER,
                    Text = "18:05"
                },
                TimeRange = (DateTime.Parse("2023-3-31 18:05:00"), DateTime.Parse("2023-3-31 20:05:00"))
            };
            DeepAssert.Equal(expect, result);
        }
    }
}