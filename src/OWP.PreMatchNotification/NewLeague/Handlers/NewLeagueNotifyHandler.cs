namespace OWP.PreMatchNotification.NewLeague.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using MediatR;
    using OWP.PreMatchNotification.Constants;
    using OWP.PreMatchNotification.Models;
    using OWP.PreMatchNotification.NewLeague.Criteria;
    using OWP.PreMatchNotification.NewLeague.Models;
    using OWP.PreMatchNotification.NewLeague.Services;
    using OWP.PreMatchNotification.Services;

    public class NewLeagueNotifyRequest : IRequest<Unit>
    {
        public string SessionId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }

    public class NewLeagueNotifyHandler : IRequestHandler<NewLeagueNotifyRequest, Unit>
    {
        private readonly CacheManipulator cacheManipulator;

        private readonly DynamicRepository dynamicRepository;

        private readonly NewLeagueValueGenerator newLeagueGenerator;

        private readonly MessageFactory messageFactory;

        private readonly CommandRepository commandRepository;

        private readonly NotifyServices notify;

        public NewLeagueNotifyHandler(
            CacheManipulator cacheManipulator,
            DynamicRepository dynamicRepository,
            NewLeagueValueGenerator newLeagueGenerator,
            MessageFactory messageFactory,
            CommandRepository commandRepository,
            NotifyServices notify)
        {
            this.cacheManipulator = cacheManipulator;
            this.dynamicRepository = dynamicRepository;
            this.newLeagueGenerator = newLeagueGenerator;
            this.messageFactory = messageFactory;
            this.commandRepository = commandRepository;
            this.notify = notify;
        }

        public async Task<Unit> Handle(NewLeagueNotifyRequest request, CancellationToken cancellationToken)
        {
            var messages = await GenerateMessages(request);
            var messageModels = messageFactory.Transform(messages);

            await commandRepository.InsertMessage(messageModels, request.SessionId);

            await notify.Notify(messageModels);

            return await Task.FromResult(Unit.Value);
        }

        private async Task<IList<ChangedValue>> GenerateMessages(NewLeagueNotifyRequest request)
        {
            var oldSnapshot = await cacheManipulator.GetOldPreMatchLeagueSnapshot(request.SessionId);
            var newSnapshot = await GetPreMatchLeagueNewSnapshot(request.FromDate, request.ToDate);
            var preMatchAssignees = await commandRepository.GetEmployeesBySection(Section.PreMatch);
            var messages = newLeagueGenerator.Create(oldSnapshot, newSnapshot, preMatchAssignees);

            await cacheManipulator.DeleteOldPreMatchLeagueSnapshot(request.SessionId);

            return messages.ToList();
        }

        private async Task<IList<PreMatchLeague>> GetPreMatchLeagueNewSnapshot(DateTime fromDate, DateTime toDate)
        {
            var criteria = new GetPreMatchLeaguesCriteria
            {
                FromDate = fromDate,
                ToDate = toDate
            };
            var result = await dynamicRepository.FetchAsync<PreMatchLeague>(criteria).ConfigureAwait(false);

            return result.ToList();
        }
    }
}