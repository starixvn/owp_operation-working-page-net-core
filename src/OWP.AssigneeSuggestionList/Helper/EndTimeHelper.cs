using OWP.AssigneeSuggestionList.Models;
using OWP.Constants;
using OWP.Extensions;

namespace OWP.AssigneeSuggestionList.Helper;
public static class EndTimeHelper
{
    public static List<AssignmentModel> Update(IList<AssignmentModel> assignmentModels,
        IList<ParentMatchInfo> parentMatchInfos)
        => assignmentModels.Select(x => x.IsSaba
            ? UpdateEndTimeForSabaMatch(x)
            : UpdateEndTimeForRealMatch(x, parentMatchInfos))
            .ToList();

    private static AssignmentModel UpdateEndTimeForSabaMatch(AssignmentModel assignment)
    {
        assignment.EndTime = assignment.EventDateTime.AddHours(1);
        return assignment;
    }

    private static AssignmentModel UpdateEndTimeForRealMatch(AssignmentModel assignment,
        IList<ParentMatchInfo> parentMatchInfos)
        => assignment.IsMainMatch
            ? UpdateEndTimeForFtMatch(assignment)
            : UpdateEndTimeForCornerBooking(assignment, parentMatchInfos);

    private static AssignmentModel UpdateEndTimeForFtMatch(AssignmentModel assignment)
    {
        assignment.EndTime = assignment.BettypeId == Bettype.FirstHalfHandicap ||
                             assignment.BettypeId == Bettype.FirstHalfOverUnder
            ? assignment.EventDateTime.AddHours(1)
            : assignment.EventDateTime.Add(CalculateFtMatchEndTime(assignment.Remark));
        return assignment;
    }

    private static AssignmentModel UpdateEndTimeForCornerBooking(AssignmentModel assignment, IList<ParentMatchInfo> parentMatchInfos)
    {

        if (assignment.BettypeId is Bettype.NextCorner or Bettype.NextBooking or Bettype.Corner or Bettype.TotalBooking)
        {
            var remark = parentMatchInfos
                .FirstOrDefault(x => x.OwpMatchId == assignment.ParentMatchId)?.Remark;
            assignment.EndTime = assignment.EventDateTime.Add(CalculateCornerMatchEndTime(remark));
        }
        else
        {
            assignment.EndTime = assignment.EventDateTime.Add(new TimeSpan(2, 0, 0));
        }

        return assignment;
    }

    public static TimeSpan CalculateFtMatchEndTime(string remark)
        => remark switch
        {
            _ when remark.IsContains("ET") && remark.IsContains("PK")
                => new TimeSpan(3, 0, 0),
            _ when !remark.IsContains("ET") && remark.IsContains("PK")
                => new TimeSpan(2, 30, 0),
            _ => new TimeSpan(2, 0, 0)
        };

    public static TimeSpan CalculateCornerMatchEndTime(string remark)
     => remark.IsContains("ET") && !remark.IsContains("PK")
         ? new TimeSpan(3, 0, 0)
         : new TimeSpan(2, 0, 0);
}
