using NSubstitute;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services.RealMatch;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.AssigneeSuggestionList.UnitTests.Services
{
    public class FantasyServiceUnitTests
    {
        private WorkingSchedule schedules = new WorkingSchedule();

        private List<AssignmentModel> assignments = new List<AssignmentModel>();

        private readonly SingleGameGroupService singleGameGroupService;

        public FantasyServiceUnitTests()
        {
            this.singleGameGroupService = Substitute.For<SingleGameGroupService>(); ;
        }

        private void Initialize()
        {
            assignments = new List<AssignmentModel>()
            {
                new AssignmentModel()
                {
                    OWPMatchId = 1,
                    KOTime = DateTime.Parse("2023-3-31 18:00:00").TimeOfDay,
                    EventDateTime = DateTime.Parse("2023-3-31 18:00:00"),
                    BettypeId = 1,
                    UserId = 123,
                    Remark = "ET/PK",
                    LeagueName= "VN FANTASY MATCH"
                }
            };
        }

        [Fact]
        public void BuildScheduleUpdateModel_ReturnScheduleUpdateModel()
        {
            // Arrange
            Initialize();

            // Act
            var result = FantasyServiceImp.BuildScheduleUpdateModel(assignments, DateTime.Parse("2023-3-31 18:00:00"));

            var expect = new ScheduleUpdateModel
            {
                FilterDate = DateTime.Parse("2023-3-31 18:00:00"),
                EtPk = new ETPKUpdateSchedule
                {
                    IsHasEtPk = true,
                    EtPkValue = "ET/PK"
                },
                AssignmentCell = new AssignmentCell
                {
                    Color = ColorCodeConst.FANTASY,
                    TextColor = ColorCodeConst.TEXT_FANTASY,
                    Text = "V"
                },
                TimeRange = (DateTime.Parse("2023-3-31 18:00:00"), DateTime.Parse("2023-3-31 20:00:00"))
            };

            //Assert
            DeepAssert.Equal(expect, result);
        }
    }
}