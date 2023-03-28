using Fanex.Logging;
using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;


namespace OWP.AssigneeSuggestionList.Services
{
    public interface CornerService
    {
        Task<Unit> Group(IList<AssignmentModel> assignments, WorkingSchedule schedules);

    }

    public class CornerServiceImp : CornerService
    {
        private readonly SingleGameGroupService workingSchedule;

        public CornerServiceImp(SingleGameGroupService workingSchedule)
        {
            this.workingSchedule = workingSchedule;
        }

        public async Task<Unit> Group(IList<AssignmentModel> assignments, WorkingSchedule schedules)
        {
            var cornerMatchGroup = assignments?.Where(a => a.IsCornerMatch)
             ?.ToList();
            await workingSchedule.Group(cornerMatchGroup, schedules, new AssignmentCell
            {
                Color = ColorCodeConst.CORNER,
                Text = "CORNER"
            }, "[CORNER]");
            return Unit.Value;
        }
    }
}