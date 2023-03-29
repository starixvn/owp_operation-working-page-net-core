using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;


namespace OWP.AssigneeSuggestionList.Services
{
    public interface MJGroupService
    {
        Task<Unit> Group((IList<AssignmentModel>, IList<MaxbetModel>) assignments,
            WorkingSchedule schedules);
    }

    public class MJGroupServiceImp : MJGroupService
    {
        private readonly SingleGameGroupService workingSchedule;

        public MJGroupServiceImp(SingleGameGroupService workingSchedule)
        {
            this.workingSchedule = workingSchedule;
        }
        public async Task<Unit> Group(
            (IList<AssignmentModel>, IList<MaxbetModel>) assignments,
            WorkingSchedule schedules
            )
        {
            var mJGroup = assignments.Item1?.Where(a => a.IsMajorLeague)
            ?.ToList();
            await workingSchedule.Group(
                (mJGroup, assignments.Item2),
                 schedules, ColorCodeConst.MJ, "[LinkNGame]");
            return Unit.Value;
        }
    }
}