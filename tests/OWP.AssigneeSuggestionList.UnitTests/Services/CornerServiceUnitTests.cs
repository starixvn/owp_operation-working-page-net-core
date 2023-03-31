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
                    KOTime=DateTime.Now.TimeOfDay,
                    EventDateTime=DateTime.Now,
                    BettypeId= 1,
                    UserId=123,
                    Remark="ETPK"
                },
                new AssignmentModel(){
                    OWPMatchId=2,
                    KOTime=DateTime.Now.TimeOfDay,
                    EventDateTime=DateTime.Now,
                    IsCornerMatch=true,
                    BettypeId= 1,
                    UserId=123
                }
            };
            assignmentsCorner = new List<AssignmentModel>()
            {
                new AssignmentModel(){
                    OWPMatchId=2,
                    KOTime=DateTime.Now.TimeOfDay,
                    EventDateTime=DateTime.Now,
                    IsCornerMatch=true,
                    BettypeId= 1,
                    UserId=123
                }
            };
        }

        [Fact]
        public void BuildScheduleUpdateModel_ReturnScheduleUpdateModel()
        {
            // Arrange
            Initialize();
            var service = new CornerServiceImp(singleGameGroupService);
            var expect = new WorkingSchedule
            {
                UserInfo = new UserInfo
                {
                    UserId = 123,
                    Username = "sa",
                    Role = "Administrator",
                    ShiftStartTime = DateTime.Now,
                    ShiftEndTime = DateTime.Now.AddHours(12)
                },
                Today_00h = new AssignmentCell
                {
                    Color = ColorCodeConst.CORNER,
                    TextColor = ColorCodeConst.TEXT_CORNER,
                    Text = "V"
                }
            };
            var result = CornerServiceImp.BuildScheduleUpdateModel(assignmentsCorner, assignments);

            var expect1 = expect;
            //DeepAssert.ReferenceEquals(expect,)
        }
    }
}