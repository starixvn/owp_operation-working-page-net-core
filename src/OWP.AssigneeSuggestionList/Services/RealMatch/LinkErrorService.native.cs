using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;
using MatchType = OWP.AssigneeSuggestionList.Constant.MatchType;


namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface LinkErrorService : IRequest<Unit>
{
    void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
         DateTime filterDate,
        List<int> owpIdExclude);
}

public class LinkErrorServiceImp : LinkErrorService
{
    private readonly WorkingScheduleService workingSchedule;

    public LinkErrorServiceImp(WorkingScheduleService workingSchedule)
    {
        this.workingSchedule = workingSchedule;
    }

    public void Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
        DateTime filterDate,
        List<int> owpIdExclude)
    {
        var linkNGamePair = LinkGameConditionHelper.GroupGame(assignments, numberGameInGroup => numberGameInGroup > 3);
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.ERROR,
            TextColor = ColorCodeConst.TEXT_ERROR
        };
        foreach (var eventDate in linkNGamePair.Keys)
        {
            var owpMatchId = linkNGamePair[eventDate].Select(x => x.OWPMatchId).Distinct();
            owpIdExclude.AddRange(owpMatchId);
            assignmentCell.Text = "Error";

            var scheduleUpdateModel = new ScheduleUpdateModel
            {
                FilterDate = filterDate,
                EtPk = new ETPKUpdateSchedule(),
                AssignmentCell = assignmentCell,
                TimeRange = (eventDate, eventDate)
            };
            var assignmentPresent = linkNGamePair[eventDate].MinBy(x => x.BettypeId);
            if (assignmentPresent != null)
            {
                assignmentPresent.MatchType = MatchType.RealMatchLinkError;
            }
            workingSchedule.UpdateWorkingSchedule(scheduleUpdateModel,
                schedule, assignmentPresent);
        }
    }
}