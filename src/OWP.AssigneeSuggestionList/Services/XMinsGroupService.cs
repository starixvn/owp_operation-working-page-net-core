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
             , assignments15Mins => BuildScheduleUpdateModel(assignments15Mins, assignments));
        }

        private ScheduleUpdateModel BuildScheduleUpdateModel(
            IList<AssignmentModel> assignments15Mins,
            IList<AssignmentModel> assignments)
        {
            var (eventDate, owpMatchId, _, parentMatchId) = ScheduleHelper.GetGameInfo15Mins(assignments15Mins);
            var parentGame = assignments.FirstOrDefault(g => g.OWPMatchId == parentMatchId);
            var assignmentCell = new AssignmentCell
            {
                Color = ColorCodeConst.X_MINS,
                TextColor = ColorCodeConst.TEXT_X_MINS,
                Text = ScheduleHelper.BuildScheduleTextSingleGame(parentGame)
            };

            // TODO
            return new()
            {
                EventDate = eventDate,
                EtPk = new ETPKUpdateSchedule(),
                AssignmentCell = assignmentCell,
                TimeRange = (eventDate, eventDate)
            };
        }
    }
}