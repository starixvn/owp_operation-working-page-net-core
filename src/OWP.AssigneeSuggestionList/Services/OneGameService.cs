using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface OneGameService
    {
        Task Group(IList<AssignmentModel> assignments, WorkingSchedule schedules);
    }

    public class OneGameServiceImp : OneGameService
    {
        private const decimal MIN_MAXBET = 12000;

        private readonly SingleGameGroupService workingSchedule;

        public OneGameServiceImp(SingleGameGroupService workingSchedule)
        {
            this.workingSchedule = workingSchedule;
        }

        public async Task Group(IList<AssignmentModel> assignments, WorkingSchedule schedules)
        {
            var oneGameGroup = assignments
            ?.Where(a => LinkGameConditionHelper.CanGroupNGame(a, maxbet => maxbet > MIN_MAXBET))
            .ToList();
            var assignmentCell = new AssignmentCell
            {
                Color = ColorCodeConst.ONE_GAME,
                TextColor = ColorCodeConst.TEXT_ONE_GAME
            };
            await workingSchedule.Group(
                oneGameGroup,
                schedules,
                assignmentCell,
                "[oneGameGroup]");
        }
    }
}