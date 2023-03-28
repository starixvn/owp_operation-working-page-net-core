using Fanex.Logging;
using MediatR;
using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Helper;
using OWP.AssigneeSuggestionList.Models;


namespace OWP.AssigneeSuggestionList.Services
{
    public interface MJGroupService
    {
        Task<Unit> Group(IList<AssignmentModel> assignments, IList<WorkingSchedule> schedules);
    }

    public class MJGroupServiceImp : MJGroupService
    {
        private readonly ILogger logger;

        public MJGroupServiceImp(ILogger logger)
        {
            this.logger = logger;
        }
        public async Task<Unit> Group(IList<AssignmentModel> assignments, IList<WorkingSchedule> schedules)
        {
            var mJGroup = assignments?.Where(a => a.IsMajorLeague);
            var mJGroupPair = mJGroup
                ?.GroupBy(a => new { a.UserId, a.OWPMatchId })
                .ToDictionary(b => b.Key, b => b.ToList());
            foreach (var key in mJGroupPair.Keys)
            {
                var eventDate = mJGroupPair[key].First().EventDateTime;
                var bettypeIds = mJGroupPair[key].Select(x => x.BettypeId).ToList();
                var (startTime, endTime) = CalculateTimeRangeHelper.CalculateTimeRange(eventDate, bettypeIds, true);
                var scheduleProperties = AssigneeSuggestionHelper.GetValidProperties(eventDate, startTime, endTime);

                var schedule = CreateWorkingScheduleHelper.CreateWorkingSchedule(new()
                {
                    PropertiesName = scheduleProperties,
                    AssignmentCell = new AssignmentCell
                    {
                        Color = ColorCodeConst.MJ,
                        Text = "MJ"
                    },
                    UserId = key.UserId
                });
                schedules.Add(schedule);
            }
            await logger.InfoAsync("[mJGroup]" + mJGroup?.Select(x => x.OWPMatchId));

            return Unit.Value;
        }


    }
}