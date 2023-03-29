using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface XMinsGroupService
    {
        Task Group((IList<AssignmentModel>, IList<MaxbetModel>) assignments, WorkingSchedule schedule);
    }

    public class XMinsGroupServiceImp : XMinsGroupService
    {

        private readonly SingleGameGroupService workingSchedule;

        public XMinsGroupServiceImp(SingleGameGroupService workingSchedule)
        {
            this.workingSchedule = workingSchedule;
        }

        public async Task Group((IList<AssignmentModel>, IList<MaxbetModel>) assignments, WorkingSchedule schedule)
        {
            var gameGroup = assignments.Item1?.Where(a => a.Is15Mins)
                ?.ToList();
            await workingSchedule.Group((gameGroup, assignments.Item2), schedule, ColorCodeConst.X_MINS, "[X_MINS]");
        }
    }
}