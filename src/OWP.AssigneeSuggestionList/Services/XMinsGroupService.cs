using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface XMinsGroupService
    {
        Task Group(IList<AssignmentModel> assignments, WorkingSchedule schedule);
    }

    public class XMinsGroupServiceImp : XMinsGroupService
    {

        private readonly SingleGameGroupService workingSchedule;

        public XMinsGroupServiceImp(SingleGameGroupService workingSchedule)
        {
            this.workingSchedule = workingSchedule;
        }

        public async Task Group(IList<AssignmentModel> assignments, WorkingSchedule schedule)
        {
            var gameGroup = assignments?.Where(a => a.Is15Mins)
                ?.ToList();

            await workingSchedule.Group(
                gameGroup,
                schedule,
                "[X_MINS]"
             , BuildScheduleUpdateModel);
        }

        private ScheduleUpdateModel BuildScheduleUpdateModel(IList<AssignmentModel> assignments)
        {
            var assignmentCell = new AssignmentCell
            {
                Color = ColorCodeConst.X_MINS,
                TextColor = ColorCodeConst.TEXT_X_MINS,
                //Text = ScheduleHelper.BuildScheduleTextSingleGame(assignments.First())
            };
            var (eventDate, owpMatchId, bettypeIds) = ScheduleHelper.GetGameInfo(assignments);

            // TODO
            return new ScheduleUpdateModel
            {
                EventDate = eventDate,
                EtPk = new ETPKUpdateSchedule(),
                AssignmentCell = assignmentCell,
                //TimeRange = timeRange
            };
        }
    }
}