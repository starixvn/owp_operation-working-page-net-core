using MediatR;
using OWP.AssigneeSuggestionList.Constant;
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
            var assignmentCell = new AssignmentCell
            {
                Color = ColorCodeConst.X_MINS,
                TextColor = ColorCodeConst.TEXT_X_MINS
            };
            await workingSchedule.Group(
                gameGroup,
                schedule,
                assignmentCell,
                "[X_MINS]");
        }
    }
}