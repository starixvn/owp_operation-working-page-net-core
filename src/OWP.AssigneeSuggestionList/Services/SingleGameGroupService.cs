using Fanex.Logging;
using MediatR;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface SingleGameGroupService
    {
        Task<Unit> Group(
                    IList<AssignmentModel> assignments,
                    WorkingSchedule schedule,
                    AssignmentCell assignmentCell,
                    string groupType);
    }
    public class SingleGameGroupServiceImp : SingleGameGroupService
    {
        private readonly ILogger logger;

        private readonly WorkingScheduleService workingSchedule;

        public SingleGameGroupServiceImp(ILogger logger, WorkingScheduleService workingSchedule)
        {
            this.logger = logger;
            this.workingSchedule = workingSchedule;
        }

        public async Task<Unit> Group(
            IList<AssignmentModel> assignments,
            WorkingSchedule schedule,
            AssignmentCell assignmentCell,
            string groupType)
        {
            var mJGroupPair = assignments
                ?.GroupBy(a => a.OWPMatchId)
                .ToDictionary(b => b.Key, b => b.ToList());
            foreach (var key in mJGroupPair.Keys)
            {
                var eventDate = mJGroupPair[key].First().EventDateTime;
                var bettypeIds = mJGroupPair[key].Select(x => x.BettypeId).ToList();
                // LOS: TODO
                workingSchedule.UpdateWorkingSchedule(eventDate,
                 bettypeIds,
                 assignmentCell,
                    schedule,
                    new ETPKUpdateSchedule
                    {
                        EtPkValue = "",
                        IsHasEtPk = true,
                    });

                await logger.InfoAsync(groupType + string.Join(",", mJGroupPair[key]?.Select(x => x.OWPMatchId)));
                await logger.InfoAsync(groupType + string.Join(",", mJGroupPair[key]?.Select(x => x.BettypeId)));
            }
            return Unit.Value;
        }
    }
}