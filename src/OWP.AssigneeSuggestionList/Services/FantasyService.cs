using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface FantasyService
    {
        Task<Unit> Group(IList<AssignmentModel> assignments, WorkingSchedule schedule);
    }

    public class FantasyServiceImp : FantasyService
    {
        private readonly SingleGameGroupService workingSchedule;

        public FantasyServiceImp(SingleGameGroupService workingSchedule)
        {
            this.workingSchedule = workingSchedule;
        }

        public async Task<Unit> Group(
            IList<AssignmentModel> assignments,
            WorkingSchedule schedule
            )
        {
            var fantasyMatchGroup = assignments
            ?.Where(a => a.IsFantasyMatch)
            ?.ToList();

            await workingSchedule.Group(fantasyMatchGroup, schedule, new AssignmentCell
            {
                Color = ColorCodeConst.FANTASY,
                Text = "FANTASY"
            }, "[FANTASY]");
            return Unit.Value;
        }
    }
}