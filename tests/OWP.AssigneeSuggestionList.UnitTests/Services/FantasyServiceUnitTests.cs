using NSubstitute;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services;
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

        public void Initialize()
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
                    Remark = "ETPK",
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
            var result = FantasyServiceImp.BuildScheduleUpdateModel(assignments);

            var expect = new ScheduleUpdateModel
            {
                EventDate = DateTime.Parse("2023-3-31 18:00:00"),
                EtPk = new ETPKUpdateSchedule
                {
                    IsHasEtPk = true,
                    EtPkValue = "ETPK"
                },
                AssignmentCell = new AssignmentCell
                {
                    Color = ColorCodeConst.FANTASY,
                    TextColor = ColorCodeConst.TEXT_FANTASY,
                    Text = "V"
                },
                TimeRange = (DateTime.Parse("2023-3-31 18:00:00"), DateTime.Parse("2023-3-31 21:00:00"))
            };

            //Assert
            DeepAssert.Equal(expect, result);
        }
    }
}