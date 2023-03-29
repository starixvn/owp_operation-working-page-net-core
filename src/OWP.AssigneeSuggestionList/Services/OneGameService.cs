using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface OneGameService
    {
        Task Group((IList<AssignmentModel>, IList<MaxbetModel>) assignments, WorkingSchedule schedules);
    }

    public class OneGameServiceImp : OneGameService
    {
        private const decimal MIN_MAXBET = 12000;

        private readonly SingleGameGroupService workingSchedule;

        public OneGameServiceImp(SingleGameGroupService workingSchedule)
        {
            this.workingSchedule = workingSchedule;
        }

        public async Task Group((IList<AssignmentModel>, IList<MaxbetModel>) assignments, WorkingSchedule schedules)
        {
            var oneGameGroup = assignments.Item1
            ?.Where(a => LinkGameConditionHelper.CanGroupNGame(a, assignments.Item2, maxbet => maxbet > MIN_MAXBET))
            .ToList();

            await workingSchedule.Group((oneGameGroup, assignments.Item2), schedules, ColorCodeConst.ONE_GAME, "[oneGameGroup]");
        }
    }
}