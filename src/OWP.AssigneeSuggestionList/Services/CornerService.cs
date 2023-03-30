using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface CornerService
    {
        Task Group(IList<AssignmentModel> assignments, WorkingSchedule schedules);

    }

    public class CornerServiceImp : CornerService
    {
        private readonly SingleGameGroupService workingSchedule;

        public CornerServiceImp(SingleGameGroupService workingSchedule)
        {
            this.workingSchedule = workingSchedule;
        }

        public async Task Group(
             IList<AssignmentModel> assignments,
            WorkingSchedule schedules)
        {
            var gameGroup = assignments
            ?.Where(a => a.IsCornerMatch)
             ?.ToList();
            var assignmentCell = new AssignmentCell
            {
                Color = ColorCodeConst.CORNER,
                TextColor = ColorCodeConst.TEXT_CORNER
            };
            await workingSchedule.Group(
                gameGroup,
                schedules,
                assignmentCell,
                 "[CORNER]");
        }
    }
}