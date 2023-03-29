using Fanex.Logging;
using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;


namespace OWP.AssigneeSuggestionList.Services
{
    public interface CornerService
    {
        Task Group((IList<AssignmentModel>, IList<MaxbetModel>) assignments, WorkingSchedule schedules);

    }

    public class CornerServiceImp : CornerService
    {
        private readonly SingleGameGroupService workingSchedule;

        public CornerServiceImp(SingleGameGroupService workingSchedule)
        {
            this.workingSchedule = workingSchedule;
        }

        public async Task Group(
             (IList<AssignmentModel>, IList<MaxbetModel>) assignments,
            WorkingSchedule schedules)
        {
            var cornerMatchGroup = assignments.Item1?.Where(a => a.IsCornerMatch)
             ?.ToList();
            await workingSchedule.Group((cornerMatchGroup, assignments.Item2), schedules, ColorCodeConst.CORNER, "[CORNER]");
        }
    }
}