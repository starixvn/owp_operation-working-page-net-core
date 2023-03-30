using Fanex.Logging;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface SingleGameGroupService
    {
        Task Group(
                     IList<AssignmentModel> assignments,
                    WorkingSchedule schedule,
                    AssignmentCell cell,
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

        public async Task Group(
            IList<AssignmentModel> assignments,
            WorkingSchedule schedule,
            AssignmentCell assignmentCell,
            string groupType)
        {
            var mJGroupPair = assignments
                ?.GroupBy(a => a.OWPMatchId)
                .ToDictionary(b => b.Key, b => b.ToList());
            if (mJGroupPair is not null)
            {
                foreach (var key in mJGroupPair.Keys)
                {
                    var gameInfo = mJGroupPair[key].First();
                    var (eventDate, owpMatchId, bettypeIds, is15Mins) = GetGameInfo(mJGroupPair[key]);
                    assignmentCell.Text = ScheduleTextHelper.BuildScheduleTextSingleGame(gameInfo);

                    var scheduleUpdateModel = new ScheduleUpdateModel
                    {
                        EventDate = eventDate,
                        BettypeIds = bettypeIds,
                        EtPk = new ETPKUpdateSchedule(),
                        Is15Mins = is15Mins,
                        AssignmentCell = assignmentCell
                    };
                    // LOS: TODO
                    workingSchedule.UpdateWorkingSchedule(scheduleUpdateModel, schedule);

                    await logger.InfoAsync(groupType + string.Join(",", mJGroupPair[key]?.Select(x => x.OWPMatchId)));
                    await logger.InfoAsync(groupType + string.Join(",", mJGroupPair[key]?.Select(x => x.BettypeId)));
                }
            }
        }

        private (DateTime eventDate, int owpMatchId, IList<int> bettypeIds, bool Is15Mins) GetGameInfo(IList<AssignmentModel> assignments)
        {
            var gameInfo = assignments.First();
            var eventDate = gameInfo.EventDateTime;
            var owpMatchId = gameInfo.OWPMatchId;
            var is15Mins = gameInfo.Is15Mins;
            var bettypeIds = assignments.Select(x => x.BettypeId).Distinct().ToList();
            return (eventDate, owpMatchId, bettypeIds, is15Mins);
        }


    }
}