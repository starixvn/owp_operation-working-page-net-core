using NSubstitute;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services.RealMatch;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.AssigneeSuggestionList.UnitTests.Services
{
    public class FirstHalfServiceUnitTests
    {
        private List<AssignmentModel> assignments = new List<AssignmentModel>();

        private readonly SingleGameGroupService singleGameGroupService;

        public FirstHalfServiceUnitTests()
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
                    Remark = "ETPK"
                }
            };
        }

        [Fact]
        public void BuildScheduleUpdateModel_ReturnScheduleUpdateModel()
        {
            // Arrange
            Initialize();

            // Act
            var result = FirstHalfServiceImp.BuildScheduleUpdateModel(assignments, DateTime.Parse("2023-3-31 18:00:00"));

            var expect = new ScheduleUpdateModel
            {
                FilterDate = DateTime.Parse("2023-3-31 18:00:00"),
                EtPk = new ETPKUpdateSchedule
                {
                    IsHasEtPk = true,
                    EtPkValue = "ETPK"
                },
                AssignmentCell = new AssignmentCell
                {
                    Color = ColorCodeConst.FIRST_HALF,
                    TextColor = ColorCodeConst.TEXT_FIRST_HALF,
                    Text = "V"
                },
                TimeRange = (DateTime.Parse("2023-3-31 18:00:00"), DateTime.Parse("2023-3-31 21:00:00"))
            };

            //Assert
            DeepAssert.Equal(expect, result);
        }
    }
}