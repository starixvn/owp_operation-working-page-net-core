using OWP.PreMatchNotification.Criteria;

namespace OWP.PreMatchNotification.NewLeague.Handlers
{
  using Constants;
  using Criteria;
  using Fanex.Data.Repository;
  using MediatR;
  using Models;
  using OWP.NotificationRequests;
  using OWP.PreMatchNotification.Models;
  using OWP.PreMatchNotification.Services;
  using Services;

  public class NewLeagueNotifyHandler : IRequestHandler<PrematchNewLeagueNotifyRequest, Unit>
  {
    private readonly CacheManipulatorCommon cacheManipulator;

    private readonly IDynamicRepository dynamicRepository;

    private readonly NewLeagueValueGenerator newLeagueGenerator;

    private readonly MessageFactory messageFactory;

    private readonly CommandRepository commandRepository;

    private readonly NotifyServices notify;

    private const string OLD_PREMATCHLEAGUES_SNAPSHOT_KEY = "OWP-Old-PreMatchLeagues";

    public NewLeagueNotifyHandler(
        CacheManipulatorCommon cacheManipulator,
        IDynamicRepository dynamicRepository,
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

    public async Task<Unit> Handle(PrematchNewLeagueNotifyRequest request, CancellationToken cancellationToken)
    {
      var messages = await GenerateMessages(request);
      var messageModels = messageFactory.Transform(messages);

      await commandRepository.InsertMessage(messageModels, request.SessionId);

      await notify.Notify(messageModels);

      await cacheManipulator.RemoveData($"{OLD_PREMATCHLEAGUES_SNAPSHOT_KEY}-{request.SessionId}");

      return await Task.FromResult(Unit.Value);
    }

    private async Task<IList<ChangedValue>> GenerateMessages(PrematchNewLeagueNotifyRequest request)
    {
      var oldSnapshot = await cacheManipulator.GetMultipleData<PreMatchLeague>($"{OLD_PREMATCHLEAGUES_SNAPSHOT_KEY}-{request.SessionId}");
      var newSnapshot = await GetPreMatchLeagueNewSnapshot(request.FromDate);
      var preMatchAssignees = await commandRepository.GetEmployeesBySection(Section.PreMatch);
      var infoSiteMatches = await GetInfoSiteMatch(newSnapshot.Select(x => x.OWPLeagueId).ToList(), request.FromDate);

      var messages = newLeagueGenerator.Create(oldSnapshot, newSnapshot, preMatchAssignees, infoSiteMatches);

      return messages.ToList();
    }

    private async Task<IList<PreMatchLeague>> GetPreMatchLeagueNewSnapshot(DateTime fromDate)
    {
      var criteria = new GetPreMatchLeaguesCriteria
      {
        FromDate = fromDate
      };
      var result = await dynamicRepository.FetchAsync<PreMatchLeague>(criteria).ConfigureAwait(false);

      return result.ToList();
    }

    private async Task<IList<InfoSiteMatch>> GetInfoSiteMatch(List<int> owpLeagueIds, DateTime fromDate)
    {
      var criteria = new QueryInfoSiteMatchByOWPLeagueIdsCriteria
      {
        OWPLeagueIds = string.Join(',', owpLeagueIds),
        FromDate = fromDate.AddHours(12),
        ToDate = fromDate.AddDays(1).AddHours(11).AddMinutes(59)
      };
      var result = await dynamicRepository.FetchAsync<InfoSiteMatch>(criteria).ConfigureAwait(false);

      return result.ToList();
    }
  }
}