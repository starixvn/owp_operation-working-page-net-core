using NSubstitute;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.AssigneeSuggestionList.UnitTests.Services
{
    public class CornerServiceUnitTests
    {
        private WorkingSchedule schedules = new WorkingSchedule();

        private List<AssignmentModel> assignments = new List<AssignmentModel>();

        private List<AssignmentModel> assignmentsCorner = new List<AssignmentModel>();

        private readonly SingleGameGroupService singleGameGroupService;

        public CornerServiceUnitTests()
        {
            this.singleGameGroupService = Substitute.For<SingleGameGroupService>(); ;
        }

        public void Initialize()
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
            var service = new CornerServiceImp(singleGameGroupService);

            var result = CornerServiceImp.BuildScheduleUpdateModel(assignmentsCorner, assignments);

            var expect = new ScheduleUpdateModel
            {
                EventDate = DateTime.Parse("2023-3-31 18:00:00"),
                EtPk = new ETPKUpdateSchedule
                {
                    IsHasEtPk = true,
                    EtPkValue = "ET"
                },
                AssignmentCell = new AssignmentCell
                {
                    Color = ColorCodeConst.CORNER,
                    TextColor = ColorCodeConst.TEXT_CORNER,
                    Text = "V"
                },
                TimeRange = (DateTime.Parse("2023-3-31 18:05:00"), DateTime.Parse("2023-3-31 21:05:00"))
            };
            DeepAssert.ReferenceEquals(expect, result);
        }
    }
}