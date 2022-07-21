namespace OWP.PreMatchNotification.InfoSiteMatchInfo.Handlers
{
  using MediatR;
  using OWP.NotificationRequests;
  using OWP.PreMatchNotification.Constants;
  using OWP.PreMatchNotification.InfoSiteMatchInfo.Models;
  using OWP.PreMatchNotification.InfoSiteMatchInfo.Services;
  using OWP.PreMatchNotification.Models;
  using OWP.PreMatchNotification.Services;

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
      await cacheManipulator.RemoveData($"{OLD_INFOSITEMATCHES_SNAPSHOT_KEY}-{request.SessionId}");

      await commandRepository.InsertMessage(messageModels, request.SessionId);
      await notify.Notify(messageModels);

      return await Task.FromResult(Unit.Value);
    }

    public async Task<IList<ChangedValue>> GenerateMessages(MatchInfoNotifyRequest request)
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

      return assigneeMessages.Union(lstMessages).ToList();
    }
  }
}