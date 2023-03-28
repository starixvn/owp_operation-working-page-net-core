using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface XMinsGroupService
    {
        Task<Unit> Group(IList<AssignmentModel> assignments, WorkingSchedule schedule);
    }

    public class XMinsGroupServiceImp : XMinsGroupService
    {

        private readonly SingleGameGroupService workingSchedule;

        public XMinsGroupServiceImp(SingleGameGroupService workingSchedule)
        {
            this.workingSchedule = workingSchedule;
        }

        public async Task<Unit> Group(IList<AssignmentModel> assignments, WorkingSchedule schedule)
        {
            var xMinsMatchGroup = assignments?.Where(a => a.Is15Mins)
                ?.ToList();
            await workingSchedule.Group(xMinsMatchGroup, schedule, new AssignmentCell
            {
                Color = ColorCodeConst.X_MINS,
                Text = "X_MINS"
            }, "[X_MINS]");
            return Unit.Value;
        }
    }
}