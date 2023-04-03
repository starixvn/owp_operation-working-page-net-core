using Fanex.Logging;
using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;


namespace OWP.AssigneeSuggestionList.Services.RealMatch;

public interface LinkErrorService : IRequest<Unit>
{
    Task Group(IList<AssignmentModel> assignments, WorkingSchedule schedule);
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

    public async Task Group(IList<AssignmentModel> assignments, WorkingSchedule schedule)
    {
        var linkNGamePair = GetValidGame(assignments);
        var assignmentCell = new AssignmentCell
        {
            Color = ColorCodeConst.ERROR,
            TextColor = ColorCodeConst.TEXT_ERROR
        };
        foreach (var eventDate in linkNGamePair.Keys)
        {
            assignmentCell.Text = "Error";

            var scheduleUpdateModel = new ScheduleUpdateModel
            {
                EventDate = eventDate,
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

    private static IDictionary<DateTime, List<AssignmentModel>> GetValidGame(IEnumerable<AssignmentModel> models)
        => models.Where(a => LinkGameConditionHelper.CanGroupNGame(a, _ => true))
            .GroupBy(a => a.EventDateTime)
            .Where(g => g.Select(x => x.OWPMatchId).Distinct().Count() > 3)
            .ToDictionary(x => x.Key, x => x.ToList());
}