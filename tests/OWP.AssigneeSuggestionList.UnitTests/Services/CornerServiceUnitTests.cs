using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.AssigneeSuggestionList.Services.RealMatch;
using Xunit;

namespace OWP.AssigneeSuggestionList.UnitTests.Services;

using Xunit.Asserts.Compare;

public class CornerServiceUnitTests
{
    private readonly List<AssignmentModel> assignmentsCorner;
    private readonly IDictionary<int, List<AssignmentModel>> gameMapGameId;


    public CornerServiceUnitTests()
    {
        assignmentsCorner = new List<AssignmentModel>
        {
                new()
            {
                OWPMatchId=2,
                KOTime=DateTime.Parse("2023-3-31 18:05:00").TimeOfDay,
                EventDateTime=DateTime.Parse("2023-3-31 18:05:00"),
                IsCornerMatch=true,
                BettypeId= 1,
                UserId=123,
                ParentMatchId=1,
                MinorBettypeId = -4,
                Remark="ETPK"
            }
        };
        gameMapGameId = assignmentsCorner.GroupBy(x => x.OWPMatchId).ToDictionary(x => x.Key, x => x.ToList());

    }

    [Fact]
    public void BuildScheduleUpdateModel_ReturnScheduleUpdateModel()
    {
        var result = CornerServiceImp.BuildScheduleUpdateModel(gameMapGameId, assignmentsCorner, DateTime.Parse("2023-3-31 18:05:00"));

        var expect = new ScheduleUpdateModel
        {
            FilterDate = DateTime.Parse("2023-3-31 18:05:00"),
            EtPk = new ETPKUpdateSchedule
            {
                EtPkValue = ""
            },
            AssignmentCell = new AssignmentCell
            {
                Color = ColorCodeConst.CORNER,
                TextColor = ColorCodeConst.TEXT_CORNER,
                Text = "06:05"
            },
            TimeRange = (DateTime.Parse("2023-3-31 18:05:00"), DateTime.Parse("2023-3-31 19:05:00"))
        };
        DeepAssert.Equal(expect, result);
    }
}