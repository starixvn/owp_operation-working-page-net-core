namespace OWP.PreMatchNotification.InfoSiteMatchInfo.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using System;
    using OWP.PreMatchNotification.InfoSiteMatchInfo.Services;
    using OWP.PreMatchNotification.Services;
    using OWP.PreMatchNotification.Models;
    using System.Collections.Generic;
    using System.Linq;
    using OWP.PreMatchNotification.Constants;
    using OWP.PreMatchNotification.InfoSiteMatchInfo.Models;

    public class MatchInfoNotifyRequest : IRequest<Unit>
    {
        public string SessionId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }

    public class MatchInfoNotifyHandler : IRequestHandler<MatchInfoNotifyRequest, Unit>
    {
        private readonly CacheManipulatorCommon cacheManipulator;

        private readonly Repository queryRepository;

        private readonly ChangedValueGenerator changedValueGenerator;

        private readonly MessageFactory messageFactory;

        private readonly CommandRepository commandRepository;

        private readonly NotifyServices notify;

        private const string OLD_INFOSITEMATCHES_SNAPSHOT_KEY = "OWP-Old-InfoSiteMatches";

        public MatchInfoNotifyHandler(
            CacheManipulatorCommon cacheManipulator,
            Repository queryRepository,
            ChangedValueGenerator changedValueGenerator,
            MessageFactory messageFactory,
            CommandRepository commandRepository,
            NotifyServices notify)
        {
            this.cacheManipulator = cacheManipulator;
            this.queryRepository = queryRepository;
            this.changedValueGenerator = changedValueGenerator;
            this.messageFactory = messageFactory;
            this.commandRepository = commandRepository;
            this.notify = notify;
        }

        public async Task<Unit> Handle(MatchInfoNotifyRequest request, CancellationToken cancellationToken)
        {
            var messages = await GenerateMessages(request);
            var messageModels = messageFactory.Transform(messages);

            await commandRepository.InsertMessage(messageModels, request.SessionId);

            await notify.Notify(messageModels);

            return await Task.FromResult(Unit.Value);
        }

        private async Task<IList<ChangedValue>> GenerateMessages(MatchInfoNotifyRequest request)
        {
            var oldSnapshot = await cacheManipulator.GetMultipleData<InfoSiteMatch>($"{OLD_INFOSITEMATCHES_SNAPSHOT_KEY}-{request.SessionId}");
            var newSnapshot = await queryRepository.GetInfoSiteMatchNewSnapshot(request.FromDate, request.ToDate);

            var owpLeagueIds = newSnapshot.Select(x => x.OWPLeagueId).ToList();
            var preMatchAssignees = await commandRepository.GetPreMatchLeagueAssigneesByOWPLeagueIds(owpLeagueIds, request.FromDate);
            var lst = await commandRepository.GetEmployeesBySection(Section.LST);

            var assigneeMessages = changedValueGenerator.CreateAssigneeMessages(oldSnapshot, newSnapshot, preMatchAssignees);
            var infoSiteMatchIds = assigneeMessages.Select(x => x.InfoSiteMatchId).Distinct().ToList();
            var linkedMatches = await queryRepository.GetLGALinkedInfoSiteMatch(infoSiteMatchIds);
            var lstMessages = changedValueGenerator.CreateLSTMessages(assigneeMessages, lst, linkedMatches);

            var allMessages = assigneeMessages.Union(lstMessages);
            await cacheManipulator.RemoveData($"{OLD_INFOSITEMATCHES_SNAPSHOT_KEY}-{request.SessionId}");

            return allMessages.ToList();
        }
    }
}