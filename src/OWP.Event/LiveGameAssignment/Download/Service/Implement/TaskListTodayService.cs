using OWP.Services;

namespace Fanex.NAP.OWP.TaskScheduling.Service.Implement
{
    using Criteria.TaskListToday;
    using global::OWP.Event.EventShared.Builder;
    using global::OWP.Event.EventShared.Models;
    using Interfaces;
    using Repositories.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TaskListTodayService : OWPBaseService, ITaskListTodayService
    {
        private readonly ITaskListTodayRepository taskListTodayRepository;

        private readonly GetAdminSiteMatchIdService getAdminSiteMatchIdService;

        public TaskListTodayService(ITaskListTodayRepository taskListTodayRepository,
            GetAdminSiteMatchIdService getAdminSiteMatchIdService)
        {
            this.taskListTodayRepository = taskListTodayRepository;
            this.getAdminSiteMatchIdService = getAdminSiteMatchIdService;
        }

        public async Task<IEnumerable<MatchInfo>> GetMatchListToday(GetTaskListTodayCriteria criteria)
        {
            var matchInfos = new List<MatchInfo>();

            if (criteria != null && criteria.IsValid())
            {
                matchInfos = taskListTodayRepository.GetMatchListToday(criteria).ToList();
            }
            BuildIPTVCopy(matchInfos);
            await BuildAdminSiteId(matchInfos);

            return matchInfos;
        }

        public OWPCommandResult UpdateMatchNoteStatus(UpdateMatchNoteViewCriteria criteria)
        {
            var commandResult = new OWPCommandResult();

            if (criteria != null && criteria.IsValid())
            {
                commandResult = Repository.UpdateData(criteria);
            }

            return commandResult;
        }

        public IEnumerable<DateTime> UpdateReadyTradeStatus(UpdateReadyTradeStatusCriteria criteria)
        {
            IEnumerable<DateTime> result = new List<DateTime>();
            if (criteria != null && criteria.IsValid())
            {
                result = taskListTodayRepository.UpdateTradeStatus(criteria);
            }

            return result;
        }
        private async Task BuildAdminSiteId(IList<MatchInfo> matches)
        {
            foreach (var match in matches)
            {
                match.AdminSiteId = await getAdminSiteMatchIdService.GetAdminSiteMatchId(match.OWPMatchId,
                    match.ParentMatchId, match.MinorBettypeId);

            }
        }
        private void BuildIPTVCopy(IList<MatchInfo> matches)
        {
            foreach (var match in matches)
            {
                var tvCode = match.Televisions.Select(t => t.TVCode);
                match.IPTVCopy = IPTVBuilder.Build(new IPTVModel
                {
                    Television = string.Join("/", tvCode),
                    IpTv = match.IPTV,
                    Channel = match.Channel,
                    Stream = match.WatchBet
                });
            }
        }

    }
}