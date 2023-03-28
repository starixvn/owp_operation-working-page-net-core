using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface OneGameService
    {
        Task<Unit> Group(
            IList<AssignmentModel> assignments,
             IList<MaxbetModel> maxbetModels,
             WorkingSchedule schedules);
    }

    public class OneGameServiceImp : OneGameService
    {
        private const decimal MIN_MAXBET = 12000;

        private readonly SingleGameGroupService workingSchedule;

        public OneGameServiceImp(SingleGameGroupService workingSchedule)
        {
            this.workingSchedule = workingSchedule;
        }

        public async Task<Unit> Group(IList<AssignmentModel> assignments, IList<MaxbetModel> maxbetModels, WorkingSchedule schedules)
        {
            var oneGameGroup = assignments
            ?.Where(a => LinkGameConditionHelper.CanGroupNGame(a, maxbetModels, maxbet => maxbet > MIN_MAXBET))
            .ToList();

            await workingSchedule.Group(oneGameGroup, schedules, new AssignmentCell
            {
                Color = ColorCodeConst.ONE_GAME,
                Text = "ONE_GAME"
            }, "[oneGameGroup]");
            return Unit.Value;
        }
    }
}