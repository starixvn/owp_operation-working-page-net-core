using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.Enums;


namespace OWP.AssigneeSuggestionList.Services
{
    public interface FirstHalfService : IRequest<Unit>
    {
        Task<Unit> Group(IList<AssignmentModel> assignments, WorkingSchedule schedules);
    }

    public class FirstHalfServiceImp : FirstHalfService
    {
        private readonly SingleGameGroupService workingSchedule;

        public FirstHalfServiceImp(SingleGameGroupService workingSchedule)
        {
            this.workingSchedule = workingSchedule;
        }

        public async Task<Unit> Group(IList<AssignmentModel> assignments, WorkingSchedule schedule)
        {
            var firstHalfMatchGroupPair = assignments?
            .Where(a => a.UserId == schedule.UserInfo.UserId
                && DateTime.Compare(a.EventDateTime, schedule.UserInfo.EventDate) == 0)
            .GroupBy(x => new { x.UserId, x.OWPMatchId })
            .Where(x => x.All(a =>
                        Equals(a.BettypeId, EnumBettype.FirstHalfHandicap)
                        || Equals(a.BettypeId, EnumBettype.FirstHalfOverUnder)))
            .ToDictionary(a => a.Key, a => a.ToList());

            var firstHalfMatchGroup = new List<AssignmentModel>();
            foreach (var item in firstHalfMatchGroupPair)
            {
                firstHalfMatchGroup.AddRange(item.Value);
            }

            await workingSchedule.Group(firstHalfMatchGroup, schedule, new AssignmentCell
            {
                Color = ColorCodeConst.FIRST_HALF,
                Text = "FIRST_HALF"
            }, "[FIRST_HALF]");
            return Unit.Value;
        }
    }
}