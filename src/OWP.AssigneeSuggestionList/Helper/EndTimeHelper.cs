using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.Constants;
using OWP.Extensions;

namespace OWP.AssigneeSuggestionList.Helper;
public static class EndTimeHelper
{
    public static List<AssignmentModel> Update(IList<AssignmentModel> assignmentModels)
        => assignmentModels.Select(x => x.IsSaba
            ? UpdateEndTimeForSabaMatch(x)
            : UpdateEndTimeForRealMatch(x)
        ).ToList();

    private static AssignmentModel UpdateEndTimeForSabaMatch(AssignmentModel assignment)
    {
        assignment.EndTime = assignment.EventDateTime.AddHours(1);
        return assignment;
    }

    private static AssignmentModel UpdateEndTimeForRealMatch(AssignmentModel assignment)
    {
        var etpkType = ETPKHelper.DetectExtraTime(assignment);
        return assignment.IsMainMatch
            ? UpdateEndTimeForFtMatch(assignment, etpkType)
            : UpdateEndTimeForCornerBooking(assignment, etpkType);
    }

    private static AssignmentModel UpdateEndTimeForFtMatch(AssignmentModel assignment, string remarkType)
    {
        assignment.EndTime = assignment.BettypeId is Bettype.FirstHalfHandicap or Bettype.FirstHalfOverUnder
            ? assignment.EventDateTime.AddHours(1)
            : assignment.EventDateTime.Add(CalculateFtMatchEndTime(remarkType));
        return assignment;
    }

    private static AssignmentModel UpdateEndTimeForCornerBooking(AssignmentModel assignment, string remarkType)
    {
        var isCornerBooking = assignment.MinorBettypeId is Bettype.NextCorner or Bettype.NextBooking or Bettype.Corner or Bettype.TotalBooking;
        assignment.EndTime = isCornerBooking
            ? assignment.EventDateTime.Add(CalculateCornerMatchEndTime(remarkType))
            : assignment.EventDateTime.Add(new TimeSpan(2, 0, 0));

        return assignment;
    }

    public static TimeSpan CalculateFtMatchEndTime(string remark)
        => remark switch
        {
            ETPKConstant.ETPK => new TimeSpan(3, 0, 0),
            ETPKConstant.PK => new TimeSpan(2, 30, 0),
            _ => new TimeSpan(2, 0, 0)
        };

    public static TimeSpan CalculateCornerMatchEndTime(string remark)
     => remark.IsContains("ET") && !remark.IsContains("PK")
         ? new TimeSpan(3, 0, 0)
         : new TimeSpan(2, 0, 0);
}
