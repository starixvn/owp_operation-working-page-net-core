using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface FantasyService
    {
        Task Group(
            (IList<AssignmentModel>, IList<MaxbetModel>) assignments,
            WorkingSchedule schedule
            );
    }

    public class FantasyServiceImp : FantasyService
    {
        private readonly SingleGameGroupService workingSchedule;

        public FantasyServiceImp(SingleGameGroupService workingSchedule)
        {
            this.workingSchedule = workingSchedule;
        }

        public async Task Group(
            (IList<AssignmentModel>, IList<MaxbetModel>) assignments,
            WorkingSchedule schedule
            )
        {
            var matchGroup = assignments.Item1
            ?.Where(a => a.IsFantasyMatch)
            ?.ToList();

            await workingSchedule.Group((matchGroup, assignments.Item2), schedule, ColorCodeConst.FANTASY, "[FANTASY]");
        }
    }
}