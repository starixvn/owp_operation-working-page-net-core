using Fanex.Logging;
using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;


namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface LinkErrorService : IRequest<Unit>
{
    Task Group(
        IList<AssignmentModel> assignments,
        WorkingSchedule schedule,
         DateTime filterDate,
        List<int> owpIdExclude);
}

public class LinkErrorServiceImp : LinkErrorService
{
    private readonly WorkingScheduleService workingSchedule;

    private readonly ILogger logger;

    public LinkErrorServiceImp(WorkingScheduleService workingSchedule, ILogger logger)
    {
        this.workingSchedule = workingSchedule;
        this.logger = logger;
    }

    public async Task Group(
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

            workingSchedule.UpdateWorkingSchedule(scheduleUpdateModel,
                schedule);

            await logger.InfoAsync("[Error]" + string.Join(",", linkNGamePair[eventDate].Select(x => x.OWPMatchId)));
            await logger.InfoAsync("[Error]" + string.Join(",", linkNGamePair[eventDate].Select(x => x.BettypeId)));
        }
    }
}