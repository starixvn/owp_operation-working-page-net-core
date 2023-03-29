using Fanex.Logging;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Services
{
    public interface SingleGameGroupService
    {
        Task Group(
                     (IList<AssignmentModel>, IList<MaxbetModel>) assignments,
                    WorkingSchedule schedule,
                    string colorCode,
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
            (IList<AssignmentModel>, IList<MaxbetModel>) assignments,
            WorkingSchedule schedule,
            string colorCode,
            string groupType)
        {
            var mJGroupPair = assignments.Item1
                ?.GroupBy(a => a.OWPMatchId)
                .ToDictionary(b => b.Key, b => b.ToList());
            var assignmentCell = new AssignmentCell
            {
                Color = colorCode
            };
            foreach (var key in mJGroupPair.Keys)
            {
                var gameInfo = mJGroupPair[key].First();
                var (eventDate, owpMatchId, bettypeIds) = GetGameInfo(mJGroupPair[key]);
                assignmentCell.Text = ScheduleTextHelper.BuildScheduleTextSingleGame(gameInfo, assignments.Item2);

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
        }

        private (DateTime, int, IList<int>) GetGameInfo(IList<AssignmentModel> assignments)
        {
            var gameInfo = assignments.First();
            var eventDate = gameInfo.EventDateTime;
            var owpMatchId = gameInfo.OWPMatchId;
            var bettypeIds = assignments.Select(x => x.BettypeId).Distinct().ToList();
            return (eventDate, owpMatchId, bettypeIds);
        }


    }
}