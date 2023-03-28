using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;


namespace OWP.AssigneeSuggestionList.Services
{
    public interface MJGroupService
    {
        Task<Unit> Group(IList<AssignmentModel> assignments, WorkingSchedule schedules);
    }

    public class MJGroupServiceImp : MJGroupService
    {
        private readonly SingleGameGroupService workingSchedule;

        public MJGroupServiceImp(SingleGameGroupService workingSchedule)
        {
            this.workingSchedule = workingSchedule;
        }
        public async Task<Unit> Group(IList<AssignmentModel> assignments, WorkingSchedule schedules)
        {
            var mJGroup = assignments?.Where(a => a.IsMajorLeague)
            ?.ToList();
            await workingSchedule.Group(mJGroup, schedules, new AssignmentCell
            {
                Color = ColorCodeConst.MJ,
                Text = "MJ"
            }, "[LinkNGame]");
            return Unit.Value;
        }
    }
}